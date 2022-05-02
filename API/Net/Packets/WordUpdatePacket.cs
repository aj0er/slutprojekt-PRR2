namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till alla spelare då ordet ska ändras.
    /// Detta inkluderar det fulla ordet, men även partiella ledtrådar.
    /// </summary>
    [PacketAttribute(7)]
    public class WordUpdatePacket : Packet
    {
        
        /// <summary>
        /// Det uppdaterade ordet.
        /// </summary>
        public string Word { get; }

        /// <summary>
        /// Skapar ett nytt WordUpdatePacket.
        /// </summary>
        /// <param name="word">Det nya ordet.</param>
        public WordUpdatePacket(string word)
        {
            Word = word;
        }
        
    }
}
