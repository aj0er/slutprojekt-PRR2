using System;
using System.Collections.Generic;
using API.History;
using API.Net.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.History;

namespace Server
{
    /// <summary>
    /// Deserialiserar JSON-data till <see cref="HistoryType"/>
    /// </summary>
    public class HistoryPointDeserializer : JsonConverter<HistoryPoint>
    {

        private readonly Dictionary<HistoryType, Type> _historyTypes = new Dictionary<HistoryType, Type>();

        public HistoryPointDeserializer()
        {
            _historyTypes[HistoryType.Connect] = typeof(ConnectPacket);
            _historyTypes[HistoryType.Chat] = typeof(ChatPacket);
            _historyTypes[HistoryType.Disconnect] = typeof(DisconnectPacket);
            
            _historyTypes[HistoryType.StateStart] = typeof(StateStartHistory);
            _historyTypes[HistoryType.StateEnd] = typeof(StateEndHistory);
            _historyTypes[HistoryType.BroadcastMessage] = typeof(BroadcastMessageHistory);
        }

        public override bool CanWrite => false;  // Denna JsonConverter deserialiserar endast.

        public override void WriteJson(JsonWriter writer, HistoryPoint value, JsonSerializer serializer) { } // Denna JsonConverter deserialiserar endast.

        public override HistoryPoint ReadJson(JsonReader reader, Type objectType, 
            HistoryPoint existingValue, bool hasExistingValue, JsonSerializer serializer) 
        {
             
            JObject root = JObject.Load(reader);
            JToken timeStampToken = root["Timestamp"];
            if (timeStampToken == null)
                return new HistoryPoint(0, null);
                
            long timestamp = timeStampToken.ToObject<long>();
            JToken dataToken = root["Data"];

            if (dataToken == null)
                return new HistoryPoint(0, null);
            
            JToken historyTypeToken = dataToken["HistoryType"];
            if (historyTypeToken == null)
                return new HistoryPoint(0, null);

            HistoryType historyType = historyTypeToken.ToObject<HistoryType>();
            if (_historyTypes.TryGetValue(historyType, out Type type)) // Om typen finns.
            {
                IHistory data = (IHistory) dataToken.ToObject(type);
                return new HistoryPoint(timestamp, data);
            }

            Console.WriteLine("No history data type mapped for type \"" + Enum.GetName(typeof(HistoryType), historyType) + "\"!");
            return new HistoryPoint(0, null);

        }
    }
}