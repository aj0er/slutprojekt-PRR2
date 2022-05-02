using System;
using System.Drawing;
using API.Net.Packets;
using Server.Games;

namespace Server.State.Games
{
    /// <summary>
    /// State för då spelet är slut och klienten ska visa den slutgiltiga topplistan.
    /// </summary>
    class PostGameState : GameState
    {

        /// <summary>
        /// Skapar ett nytt PostGameState.
        /// </summary>
        /// <param name="game">Spelet som statet tillhör.</param>
        public PostGameState(Game game) : base(game, TimeSpan.FromSeconds(20)) { }

        protected override void OnEnd()
        {
            Game.End();
        }

        protected override void OnStart()
        {
            Game.BroadcastMessage("Spelet är slut, tack för att ni spelade!", Game.DefaultMessageColor);
            Game.BroadcastPacket(new GameStatePacket(API.State.GameStateType.PostGame, 0, Game.CurrentRound));
        }

        protected override void OnUpdate() { }
        
    }
}
