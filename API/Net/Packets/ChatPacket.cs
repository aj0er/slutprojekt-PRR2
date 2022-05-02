using API.History;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas av spelare då de skriver något i chatten.
    /// </summary>
    [PacketAttribute(4)]
    public class ChatPacket : IncomingPacket, IHistory
    {

        /// <summary>
        /// Meddelandet som ska skickas.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Skapar ett nytt ChatPacket.
        /// </summary>
        /// <param name="message">Meddelandet som ska skickas.</param>
        public ChatPacket(string message)
        {
            Message = message;
        }

        public HistoryType HistoryType => HistoryType.Chat;
        public string CreateSummary()
        {
            return $"[CHAT] {Sender.Name}: {Message}";
        }
        
    }
}