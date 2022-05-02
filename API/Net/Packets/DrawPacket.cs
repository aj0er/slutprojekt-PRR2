using API.Drawing;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas av spelare då de ritat något, eller till spelare från servern när ritandet ska vidarebefordras till alla spelare.
    /// </summary>
    [PacketAttribute(2)]
    public class DrawPacket : IncomingPacket
    {

        /// <summary>
        /// Händelsen som ska ritas.
        /// </summary>
        public DrawAction Action { get; }
        /// <summary>
        /// Händelsens element-ID, vilket element som ska uppdateras eller ett större tal om det gäller ett nytt element.
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Skapar en ny DrawPacket.
        /// </summary>
        /// <param name="action"><see cref="DrawAction" /> med information om ritandet.</param>
        /// <param name="elementId">Händelsens element-ID, vilket element som ska uppdateras med händelsen eller om händelsen ska skapa ett nytt element på ritytan.</param>
        public DrawPacket(DrawAction action, int elementId)
        {
            Action = action;
            ElementId = elementId;
        }

    }
}
