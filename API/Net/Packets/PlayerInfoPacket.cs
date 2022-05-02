using Newtonsoft.Json;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till spelare för att de ska få information om vilka andra spelare som finns med i spelet.
    /// </summary>
    [PacketAttribute(5)]
    public class PlayerInfoPacket : Packet
    {
        
        /// <summary>
        /// Array med spelare som något ska göras med.
        /// </summary>
        public Player[] Info { get; }
        /// <summary>
        /// Vad som ska göras med spelarna.
        /// </summary>
        public InfoMode Mode { get; }

        /// <summary>
        /// Skapar ett nytt PlayerInfoPacket.
        /// </summary>
        /// <param name="info">Spelarnas data.</param>
        /// <param name="mode">Vad som ska göras med spelarna.</param>
        [JsonConstructor]
        public PlayerInfoPacket(Player[] info, InfoMode mode = InfoMode.Initial)
        {
            Info = info;
            Mode = mode;
        }

        /// <summary>
        /// Skapar ett nytt PlayerInfoPacket med en enskild spelare.
        /// </summary>
        /// <param name="info">Spelarens data.</param>
        /// <param name="mode">Vad som ska göras med spelarna.</param>
        public PlayerInfoPacket(Player info, InfoMode mode) : this(new Player[] { info }, mode) { }

        /// <summary>
        /// Enumeration över händelser som kan ske med spelare.
        /// </summary>
        public enum InfoMode
        {
            /// <summary>
            /// Skickas till spelare då de ansluter, för att de ska veta vad de själva har för ID och potentiellt modifierade egenskaper.
            /// </summary>
            Self,

            /// <summary>
            /// Skickas till nya spelare då de ansluter, får listan över alla nuvarande spelare i spelet.
            /// </summary>
            Initial,

            /// <summary>
            /// Skickas till alla spelare då en spelare ska läggas till.
            /// </summary>
            Add,

            /// <summary>
            /// Skickas till alla spelare då en spelare ska tas bort.
            /// </summary>
            Remove
        }
    
    }
}