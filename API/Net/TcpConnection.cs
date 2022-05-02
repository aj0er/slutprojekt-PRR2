using System.Net.Sockets;

namespace API.Net
{
    /// <summary>
    /// Klass som håller i en TCP-klient och dess interna ID för att kunna hantera flera unika anslutningar.
    /// </summary>
    public class TcpConnection
    {

        /// <summary>
        /// Anslutningens id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// TCP-klienten som kan läsas och skrivas till.
        /// </summary>
        public TcpClient Client { get; }
        /// <summary>
        /// Om anslutningen är avbruten.
        /// </summary>
        public bool Disconnected { get; set; }

        /// <summary>
        /// Skapar en ny TcpConnection.
        /// </summary>
        /// <param name="id">Anslutningens ID.</param>
        /// <param name="client">TCP-klienten att läsa och skriva från.</param>
        public TcpConnection(int id, TcpClient client)
        {
            Id = id;
            Client = client;
        }

    }
}
