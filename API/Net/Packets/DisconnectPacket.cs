using API.History;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas av spelare då de lämnar spelet, eller av servern för att internt hantera en disconnect.
    /// </summary>
    [PacketAttribute(6)]
    public class DisconnectPacket : IncomingPacket, IHistory
    {
        
        public HistoryType HistoryType => HistoryType.Disconnect;
        public string CreateSummary()
        {
            return $"{Sender.Name} disconnected.";
        }
        
    }
}