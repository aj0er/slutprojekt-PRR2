using API.History;
using Newtonsoft.Json;

namespace Server.History
{
    /// <summary>
    /// Historik för då ett game state avslutas.
    /// </summary>
    public class StateEndHistory : IHistory
    {
        
        [JsonProperty]
        public string StateName { get; }
        
        /// <summary>
        /// Skapar en ny StateEndHistory.
        /// </summary>
        /// <param name="stateName">Namnet på statet som avslutades.</param>
        public StateEndHistory(string stateName)
        {
            StateName = stateName;
        }
        
        public HistoryType HistoryType => HistoryType.StateEnd;
        public string CreateSummary()
        {
            return $"State ended: {StateName}";
        }
        
    }
}