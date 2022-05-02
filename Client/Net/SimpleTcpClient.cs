using API.Net;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Client.Net
{
    /// <summary>
    /// Implementation av en simpel TCP-klient. 
    /// Kan ansluta till en server, skicka och ta emot meddelanden.
    /// </summary>
    public class SimpleTcpClient
    {
        
        private TcpConnection _connection;
        private readonly TcpEvents _events;
        private bool _connecting;

        /// <summary>
        /// Skapar en ny SimpleTCPClient.
        /// </summary>
        /// <param name="events">Event subscriber att anropa events på.</param>
        public SimpleTcpClient(TcpEvents events)
        {
            _events = events;
        }

        /// <summary>
        /// Ansluter till en TCP-server.
        /// </summary>
        /// <param name="host">Serverns hostaddress.</param>
        /// <param name="port">Serverns port.</param>
        public async void Connect(string host, int port)
        {
            if (_connecting)
                return;
           
            TcpClient client = new TcpClient();
            client.NoDelay = true;

            _connection = new TcpConnection(-1, client);
            _connecting = true;

            try
            {
                await _connection.Client.ConnectAsync(IPAddress.Parse(host), port);
            }
            catch (Exception)
            {
                _connecting = false;
                Disconnect();
                return;
            }
            
            _connecting = false;
            Read();
            _events.DispatchEvent(new TcpConnectionEventArgs(_connection));
        }

        /// <summary>
        /// Avbryter anslutningen till servern om den inte redan är avslutad.
        /// </summary>
        private void Disconnect()
        {
            if(_connection != null && _connection.Client != null)
                _connection.Client.Close();

            _events.DispatchEvent(new TcpDisconnectionEventArgs(_connection));
        }

        /// <summary>
        /// Läser meddelanden som skickas från servern till klienten.
        /// </summary>
        private async void Read()
        {
            if (_connection == null)
                return;

            while (true)
            {
                TcpMessage result = await _connection.Client.ReadData();
                if (result.IsInvalid())
                {
                    Disconnect(); // Om meddelandet inte kunde läsas har klienten förlorat anslutning.
                    return;
                }

                _events.DispatchEvent(new TcpMessageEventArgs(_connection, result));
            }
        }

        /// <summary>
        /// Returnerar om klienten är ansluten till servern.
        /// </summary>
        /// <returns>Om klienten är ansluten till servern.</returns>
        private bool IsConnected()
        {
            if (_connection == null)
                return false;
            try
            {
                return _connection.Client.Connected;
            }
            catch (NullReferenceException) // Det finns inget sätt att kolla om TcpClient är disposed utan att anropa något.
            {
                return false;
            }
        }

        /// <summary>
        /// Skriver data till servern.
        /// </summary>
        /// <param name="data">Data att skriva.</param>
        public async void SendData(byte[] data)
        {
            if (!IsConnected())
                return;

            await _connection.Client.WriteData(data);
        }

    }
}
