using System.Drawing;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till spelare då ett meddelande ska visas i chatten.
    /// </summary>
    [PacketAttribute(3)]
    public class MessagePacket : Packet
    {

        /// <summary>
        /// Id för spelaren som skickat meddelandet, eller 0 om det är servern.
        /// </summary>
        public int MessageSenderId { get; }
        /// <summary>
        /// Meddelandets färg.
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Meddelandets innehåll.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Skapar ett nytt MessagePacket.
        /// </summary>
        /// <param name="messageSenderId">Id för spelaren som skickat meddelandet, eller 0 om det är servern.</param>
        /// <param name="color">Färg på meddelandet.</param>
        /// <param name="message">Meddelandets innehåll.</param>
        public MessagePacket(int messageSenderId, Color color, string message)
        {
            MessageSenderId = messageSenderId;
            Color = color;
            Message = message;
        }

    }
}
