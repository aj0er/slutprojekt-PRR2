using System;
using System.Collections.Generic;
using API;
using Server.Store;

namespace Server.History
{
    /// <summary>
    /// Historik för ett spel.
    /// </summary>
    public class GameHistory : IStoreEntity<Guid>
    {
       
        /// <summary>
        /// Spelets ID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Tidsstämpel för då spelet startade.
        /// </summary>
        public long TimeStarted { get; }
        /// <summary>
        /// Tidsstämpel för då spelet avslutades.
        /// </summary>
        public long TimeEnded { get; }
        /// <summary>
        /// Lista över spelarna i spelet
        /// </summary>
        private List<Player> Players { get; }
        /// <summary>
        /// Kollektion med händelserna i historiken.
        /// </summary>
        public ICollection<HistoryPoint> History { get; }

        /// <summary>
        /// Skapar en ny GameHistory.
        /// </summary>
        /// <param name="id">Spelets id.</param>
        /// <param name="timeStarted">Tidpunkt i millisekunder sedan epoch då spelet startades.</param>
        /// <param name="timeEnded">Tidpunkt i millisekunder sedan epoch då spelet avslutades.</param>
        /// <param name="players">Lista över spelarnas data.</param>
        /// <param name="history">Lista över specifika händelser under spelets gång.</param>
        public GameHistory(Guid id, long timeStarted, long timeEnded, List<Player> players, 
            ICollection<HistoryPoint> history)
        {
            Id = id;
            TimeStarted = timeStarted;
            TimeEnded = timeEnded;
            Players = players;
            History = history;
        }

    }
}