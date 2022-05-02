using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace API.Net
{
    /// <summary>
    /// Hjälp-extensioner för <see cref="TcpClient"/>
    /// </summary>
    public static class TcpClientExtensions
    {

        private static readonly Func<TcpMessage> EmptyMessageProvider = () => new TcpMessage(Array.Empty<byte>(), 0); // Delegate som skapar en nytt tomt TcpMessage.

        /// <summary>
        /// Läser data från en TCP-klient.
        /// Skriver till standard error vid fel.
        /// </summary>
        /// <returns>Async task med resultatet från läsningen.</returns>
        public static async Task<TcpMessage> ReadData(this TcpClient client)
        {
            if (!client.Connected)
                return EmptyMessageProvider.Invoke();

            byte[] buffer;
            int length;
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] sizeBuffer = new byte[sizeof(int)];

                // Alla paket skickas med en int först som berättar hur stort paketet är.
                // Om en int inte kan läsas (4 bytes) är paketet felaktigt formatterat.
                if(await stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length) != sizeBuffer.Length)
                {
                    await Console.Error.WriteLineAsync("Malformed Packet data sent!");
                    return EmptyMessageProvider.Invoke();
                }

                int size = BitConverter.ToInt32(sizeBuffer, 0);
                
                // Skapar en buffer med den medskickade storleken och kopierar datan från nätverksströmmen.
                buffer = new byte[size];
                length = await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception exception)
            {
                await Console.Error.WriteLineAsync(exception.Message);
                return EmptyMessageProvider.Invoke();
            }
            
            if (length == 0) // Då längden är 0 har något gått fel och vi bör avsluta anslutningen.
            {
                client.Close();
                return EmptyMessageProvider.Invoke();
            }

            return new TcpMessage(buffer, length);
        }

        /// <summary>
        /// Skriver data till en klient.
        /// Skriver till standard error vid fel.
        /// </summary>
        /// <param name="client">Klienten som datan ska skrivas till.</param>
        /// <param name="data">Data som ska skrivas.</param>
        public static async Task WriteData(this TcpClient client, byte[] data)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                byte[] size = BitConverter.GetBytes(data.Length);
                await stream.WriteAsync(size, 0, size.Length); // Skriver paketets datalängd till strömmen.
                await stream.WriteAsync(data, 0, data.Length); // Skriver paketets data till strömmen.
            }
            catch (Exception ex) {
                await Console.Error.WriteLineAsync(ex.Message);
            }
        }

    }
}
