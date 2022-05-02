using API.History;
using Newtonsoft.Json;

namespace Server.History
{
    /// <summary>
    /// Historik för då ett game state startar.
    /// </summary>
    public class StateStartHistory : IHistory
    {

        [JsonProperty]
        public string StateName { get; }
        
        /// <summary>
        /// Skapar en ny StateStartHistory.
        /// </summary>
        /// <param name="stateName">Namnet på statet som startades.</param>
        public StateStartHistory(string stateName)
        {
            StateName = stateName;
        }
        
        public HistoryType HistoryType => HistoryType.StateStart;
        public string CreateSummary()
        {
            return $"State started: {StateName}";
        }
        
    }
}