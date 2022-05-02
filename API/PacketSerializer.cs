using API.Net;
using Newtonsoft.Json;
using System;
using System.Text;
using API.Net.Packets;

namespace API
{
    /// <summary>
    /// Ansvarar för att serialisera (till unicode JSON-sträng) och deserialisera <see cref="Packet"/>.
    /// </summary>
    public class PacketSerializer
    {

        private const char PacketIdDelim = '@';

        private readonly JsonSerializerSettings _jsonSettings;
        private readonly Type[] _packetTypes;

        /// <summary>
        /// Skapar en ny PacketSerializer och initialiserar arrayen med packet-id -> typ mappning. 
        /// </summary>
        public PacketSerializer()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore // Ignorera värden som är null i JSON-datan.
            };
            
            // Mappning med packet ID -> paketets typ.
            // Packet ID läses från nätverkssdatan för att sedan deserialisera datan till lämpligt objekt.
            _packetTypes = new Type[15];
            _packetTypes[1]  = typeof(ConnectPacket);
            _packetTypes[2]  = typeof(DrawPacket);
            _packetTypes[3]  = typeof(MessagePacket);
            _packetTypes[4]  = typeof(ChatPacket);
            _packetTypes[5]  = typeof(PlayerInfoPacket);
            _packetTypes[6]  = typeof(DisconnectPacket);
            _packetTypes[7]  = typeof(WordUpdatePacket);
            _packetTypes[8]  = typeof(KickPacket);
            _packetTypes[9]  = typeof(GameStatePacket);
            _packetTypes[10] = typeof(PlayerStatePacket);
            _packetTypes[11] = typeof(PlayerScorePacket);
        }

        /// <summary>
        /// Serialiserar paketet till en JSON unicode-sträng.
        /// </summary>
        /// <param name="packet">Paket att serialisera.</param>
        /// <returns>Serialiserad data.</returns>
        public byte[] Serialize(Packet packet)
        {
            string serializedPacket = JsonConvert.SerializeObject(packet, _jsonSettings);

            // Format: <packetid>@<packetdata>
            StringBuilder builder = new StringBuilder();
            builder.Append(packet.PacketId);
            builder.Append(PacketIdDelim);
            builder.Append(serializedPacket);

            return Encoding.Unicode.GetBytes(builder.ToString());
        }

        /// <summary>
        /// Deserialiserar ett paket läst från nätverksströmmen.
        /// </summary>
        /// <param name="message">TCP-datan som ska läsas.</param>
        /// <returns>Deserialiserat paket eller null om det inte lyckades.</returns>
        public Packet Deserialize(TcpMessage message)
        {
            string str;
            try
            {
                byte[] data = message.Data;
                int length  = message.Length;

                // Läs endast så långt som meddelandet säger att vi ska läsa.
                str = Encoding.Unicode.GetString(data, 0, length);
            } catch(Exception ex) {
                Console.Error.WriteLine("Unable to deserialize incoming message: " + ex.Message);
                return null;
            }
            
            int packetIdIdx = str.IndexOf(PacketIdDelim);
            if (str.Length < 6 || (packetIdIdx != 1 && packetIdIdx != 2)) // Kollar om strängen följer formatet för hur b.la. packet-id ska placeras.
                return null;

            if(!uint.TryParse(str.Substring(0, packetIdIdx), out uint packetId)) // Paket-id måste kunna parsas till uint.
                return null;

            if (packetId >= _packetTypes.Length) // Paketets id måste vara innanför gränserna.
                return null;

            Type type = _packetTypes[packetId]; 
            if (type == null)
                return null;
            
            try
            {
                string json = str.Substring(packetIdIdx + 1); // Själva datan börjar efter @ tecknet
                return JsonConvert.DeserializeObject(json, type) as Packet;
            } catch(JsonException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
