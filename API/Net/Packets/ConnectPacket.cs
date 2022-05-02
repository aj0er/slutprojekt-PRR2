using API.History;

namespace API.Net.Packets
{
    /// <summary>
    /// Packet som skickas av spelare då de anslutit till en server och ska berätta vad de har för namn och avatar.
    /// </summary>
    [PacketAttribute(1)]
    public class ConnectPacket : IncomingPacket, IHistory
    {
        /// <summary>
        /// Spelarens namn.
        /// </summary>
        public string PlayerName { get; }
        /// <summary>
        /// Spelarens avatar.
        /// </summary>
        public Avatar Avatar { get; }

        /// <summary>
        /// Skapar ett nytt ConnectPacket.
        /// </summary>
        /// <param name="playerName">Spelarens namn.</param>
        /// <param name="avatar">Spelarens avatar.</param>
        public ConnectPacket(string playerName, Avatar avatar)
        {
            PlayerName = playerName;
            Avatar = avatar;
        }

        public HistoryType HistoryType => HistoryType.Connect;
        public string CreateSummary()
        {
            return $"{PlayerName} connected. Avatar: {Avatar}";
        }
        
    }
}
