using API.Net.Packets;
using System;
using Server.Games;

namespace Server.State.Games
{
    /// <summary>
    /// State för att ha lite väntetid mellan rundor.
    /// </summary>
    class PostRoundState : GameState
    {

        /// <summary>
        /// Skapar ett nytt PostRoundState.
        /// </summary>
        /// <param name="game">Spelet som statet tillhör.</param>
        public PostRoundState(Game game) : base(game, TimeSpan.FromSeconds(1)) { }
        
        protected override void OnEnd()
        {
            if (Game.CurrentRound != Game.MaxRounds) // Om vi nått slutet av spelet påbörjas ingen ny runda efter detta.
            {
                Game.BroadcastMessage($"Påbörjar runda {Game.CurrentRound + 1}...", Game.DefaultMessageColor);
            }
        }

        protected override void OnStart()
        {
            Game.BroadcastPacket(new GameStatePacket(API.State.GameStateType.PostRound, 0, Game.CurrentRound));
        }

        protected override void OnUpdate() { }

    }
}