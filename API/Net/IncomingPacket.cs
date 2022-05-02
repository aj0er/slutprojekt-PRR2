namespace API.Net
{
    public abstract class IncomingPacket : Packet
    {

        private Player _sender;
        
        /// <summary>
        /// Spelaren som skickat paketet.
        /// </summary>
        public Player Sender
        {
            get => _sender;
            set
            {
                if (_sender == null) // Sender får bara muteras en gång.
                    _sender = value;
            }
        }

    }
}
