using System;

namespace API.Net
{
    /// <summary>
    /// Ett paket som skickas på nätverksströmmen som serialiseras och deserialiseras.
    /// </summary>
    public class Packet : EventArgs
    {

        /// <summary>
        /// Paketets ID för att kunna mappas när det kommer fram.
        /// </summary>
        public uint PacketId { get; }

        private bool _cancelled;
        /// <summary>
        /// Om paketet ska ignoreras.
        /// </summary>
        public bool Cancelled
        {
            get => _cancelled;
            set {
                if (_cancelled) // Cancelled får bara muteras en gång.
                    return;

                _cancelled = value;
            }
        }

        private int _senderId;
        /// <summary>
        /// Id på klienten som skickat paketet.
        /// </summary>
        public int SenderId {
            get => _senderId;
            set {
                if (_senderId != 0) // Sender id får bara muteras en gång.
                    return;

                _senderId = value;
            } 
        } 

        /// <summary>
        /// Skapar ett nytt Packet, använder reflection för att hämta PacketId från attributen som ska placeras ovanför klassen.
        /// </summary>
        protected Packet()
        {
            PacketAttribute attribute = (PacketAttribute) 
                Attribute.GetCustomAttribute(GetType(), typeof(PacketAttribute));

            PacketId = attribute?.PacketId ?? 0;
        }

    }
}
