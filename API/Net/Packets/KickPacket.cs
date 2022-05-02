namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till spelare då de tvingas att lämna, de blir utkickade.
    /// </summary>
    [PacketAttribute(8)]
    public class KickPacket : Packet
    {

        /// <summary>
        /// Anledning till varför spelaren kickas.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Skapar ett nytt KickPaket.
        /// </summary>
        /// <param name="reason">Anledningen till varför spelaren kickas.</param>
        public KickPacket(string reason)
        {
            Reason = reason;
        }

    }
}
