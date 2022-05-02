using System;
using System.Collections.Generic;
using API;
using Newtonsoft.Json;

namespace Server.Games
{
    /// <summary>
    /// Internal klass i Server med vissa properties som endast behöver vara synliga på servern.
    /// </summary>
    internal class ServerPlayer : Player
    {

        /// <summary>
        /// Namnet som spelaren hade före det ändrades p.g.a dublett.
        /// Är oftast null.
        /// </summary>
        [JsonIgnore]
        public string OriginalName { get; set; }
        /// <summary>
        /// Lista över tidsstämplar på de senast skickade meddelandena.
        /// </summary>
        [JsonIgnore]
        public List<DateTime> LastMessageTimestamps { get; }

        /// <summary>
        /// Spelarens cooldown innan hen kan chatta igen.
        /// </summary>
        [JsonIgnore]
        public DateTime? ChatCooldown { get; set; }

        /// <summary>
        /// Skapar en ny ServerPlayer.
        /// </summary>
        /// <param name="id">Spelarens ID.</param>
        /// <param name="name">Spelarens namn.</param>
        /// <param name="avatar">Spelarens avatar.</param>
        /// <param name="score">Spelarens poäng.</param>
        public ServerPlayer(int id, string name, Avatar avatar, int score) : base(id, name, avatar, score){
            LastMessageTimestamps = new List<DateTime>();
        }

    }
}
