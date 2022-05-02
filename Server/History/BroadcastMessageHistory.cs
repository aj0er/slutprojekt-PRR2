using API.History;
using Newtonsoft.Json;

namespace Server.History
{
    /// <summary>
    /// Historik för då ett meddelande skickas till alla spelare i servern (ej meddelande från spelare).
    /// </summary>
    public class BroadcastMessageHistory : IHistory
    {

        [JsonProperty]
        public string Message { get; }
        
        public BroadcastMessageHistory(string message)
        {
            Message = message;
        }
        
        public HistoryType HistoryType => HistoryType.BroadcastMessage;
        public string CreateSummary()
        {
            return $"[SERVER] {Message}";
        }
        
    }
}