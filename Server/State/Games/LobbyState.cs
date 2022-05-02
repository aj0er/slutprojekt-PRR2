using System.Drawing;
using API.Net.Packets;
using API.State;
using Server.Games;

namespace Server.State.Games
{
    /// <summary>
    /// Ett state som väntar på att spelarantalet uppnår kravet.
    /// </summary>
    class LobbyState : GameState
    {

        /// <summary>
        /// Skapar ett nytt LobbyState.
        /// </summary>
        /// <param name="game">Spelet som statet tillhör.</param>
        public LobbyState(Game game) : base(game){}

        protected override void OnEnd()
        {
            Game.BroadcastPacket(new MessagePacket(0, Color.Black, "Spelet börjar..."));
        }
        
        private void OnConnect(object sender, ConnectPacket e)
        {
            if(e.Cancelled)
                return;
            
            Game.SendPacket(e.Sender, new GameStatePacket(GameStateType.Lobby, 0, 1));   
        }

        protected override void RegisterEvents(GameEvents events)
        {
            events.Connect += OnConnect;
        }
        
        protected override void UnregisterEvents(GameEvents events)
        {
            events.Connect -= OnConnect;
        }

        protected override void OnStart(){}

        protected override void OnUpdate(){}

        protected override bool ShouldEnd()
        {
            return Game.Players.Count >= Game.PlayersRequired; // Börja spelet när spelarantalkravet är uppnått.
        }

    }
}
