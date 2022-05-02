using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using API.Net;

namespace Server.Net
{
    /// <summary>
    /// En implementation av en nätverksserver som tar emot data med TCP och dispatchar till event handlers.
    /// </summary>
    class SimpleTcpServer
    {

        private readonly TcpListener _listener;
        private readonly List<TcpConnection> _connections;
        private readonly TcpEvents _eventSubscriber;
        
        private int _idInc;

        /// <summary>
        /// Skapar en ny SimpleTcpServer.
        /// </summary>
        /// <param name="host">Host att lyssna på.</param>
        /// <param name="port">Port att lyssna på.</param>
        /// <param name="eventSubscriber">Event subscriber att anropa TCP-events på.</param>
        public SimpleTcpServer(string host, int port, TcpEvents eventSubscriber)
        {
            _listener = new TcpListener(IPAddress.Parse(host), port);
            _connections = new List<TcpConnection>();
            _eventSubscriber = eventSubscriber;
        }

        /// <summary>
        /// Startar servern, börjar lyssna efter nya klienter och läser från dem.
        /// </summary>
        public bool Start()
        {
            try
            {
                _listener.Start();
            } catch(SocketException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not bind to port, is another server already running on this port? {ex.Message}");
                return false;
            }

            Read();
            return true;
        }

        /// <summary>
        /// Hämtar en anslutning beroende av dess klientid.
        /// </summary>
        /// <param name="clientId">Klientens ID.</param>
        /// <returns>Funnen anslutning eller null om ingen fanns.</returns>
        public TcpConnection GetConnectionById(int clientId)
        {
            return _connections.Find(c => c.Id == clientId);
        }

        /// <summary>
        /// Avslutar en anslutning.
        /// </summary>
        /// <param name="connection">Anslutning att avsluta.</param>
        public void Disconnect(TcpConnection connection)
        {
            connection.Disconnected = true;
            connection.Client.Close();
            _connections.Remove(connection);
            _eventSubscriber.DispatchEvent(new TcpDisconnectionEventArgs(connection));
        }

        /// <summary>
        /// Börjar läsa data från inkommande klienter.
        /// </summary>
        private async void Read()
        {
            while (true)
            {
                TcpClient client;
                try
                {
                    client = await _listener.AcceptTcpClientAsync();
                }
                catch (Exception exception)
                {
                    await Console.Error.WriteAsync(exception.Message);
                    return;
                }

                _idInc++;
                TcpConnection connection = new TcpConnection(_idInc, client);
                _connections.Add(connection);
                _eventSubscriber.DispatchEvent(new TcpConnectionEventArgs(connection));

                ReadClient(connection);
            }
        }

        /// <summary>
        /// Skriver data till en klient.
        /// </summary>
        /// <param name="client">Klient att skriva till.</param>
        /// <param name="bytes">Data att skriva.</param>
        public async void WriteClient(TcpConnection client, byte[] bytes)
        {
            await client.Client.WriteData(bytes);
        }

        /// <summary>
        /// Läser data från en klient.
        /// </summary>
        /// <param name="connection">Anslutning att läsa data från.</param>
        private async void ReadClient(TcpConnection connection)
        {
            while (true)
            {
                TcpMessage result = await connection.Client.ReadData();
                if (result.IsInvalid())
                {
                    if (!connection.Disconnected) // Om läsningen misslyckades, avsluta anslutningen.
                    {
                        Console.Error.WriteLine($"Unable to read from client with ID {connection.Id} disconnecting..");
                        if (!connection.Disconnected)
                            Disconnect(connection);

                    }

                    return;
                }

                _eventSubscriber.DispatchEvent(new TcpMessageEventArgs(connection, result));
            }
        }

    }
}
