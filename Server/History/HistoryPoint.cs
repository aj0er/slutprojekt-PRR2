using API.History;
using Newtonsoft.Json;

namespace Server.History
{
    /// <summary>
    /// En händelse i historiken.
    /// </summary>
    [JsonConverter(typeof(HistoryPointDeserializer))]
    public class HistoryPoint
    {

        /// <summary>
        /// Tidsstämpel i millisekunder sedan epoch för händelsen.
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Datan för händelsen.
        /// </summary>
        public IHistory Data { get; }

        /// <summary>
        /// Skapar en ny HistoryPoint.
        /// </summary>
        /// <param name="timestamp">Tidpunkt i millisekunder sedan epoch.</param>
        /// <param name="data">Data för händelsen.</param>
        public HistoryPoint(long timestamp, IHistory data)
        {
            Timestamp = timestamp;
            Data = data;
        }

    }
}