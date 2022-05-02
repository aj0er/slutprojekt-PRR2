using System.Collections.Generic;
using Server.Games;

namespace Server.State
{
    /// <summary>
    /// En kollektion av <see cref="GameState"/>. 
    /// Håller i logik för att advancera till nästa state då det tidigare avslutats.
    /// </summary>
    public class GameStateCollection : GameState
    {

        /// <summary>
        /// Lista över alla states som ingår i kollektionen.
        /// </summary>
        public List<GameState> States { get; }
        
        private int _idx; // Internt index för att hålla koll på det nuvarande statet.

        /// <summary>
        /// Skapar en ny kollektion med förvalda states.
        /// </summary>
        /// <param name="game">Spel som statet tillhör.</param>
        /// <param name="states">Lista med state som ska ingå i kollektionen.</param>
        public GameStateCollection(Game game, List<GameState> states) : base(game)
        {
            States = states;
        }

        /// <summary>
        /// Skapar en ny kollektion utan några förvalda states.
        /// </summary>
        /// <param name="game">Spel som statet tillhör.</param>
        protected GameStateCollection(Game game) : this(game, new List<GameState>()) {}

        /// <see cref="GameState.OnEnd"/>
        protected override void OnEnd()
        {
            if (_idx < States.Count)
                States[_idx].End();
        }

        /// <see cref="GameState.OnStart"/>
        protected override void OnStart()
        {
            OnCollectionStart();

            if (States.Count < 1) // Kollektionen har inga states
            {
                End();
                return;
            }

            States[_idx].Start();
        }

        /// <see cref="GameState.OnUpdate"/>
        protected override void OnUpdate()
        {
            GameState current = States[_idx];
            current.Update();

            if (current.Ended)
            {
                _idx++;
                if (_idx < States.Count && _idx >= 0) // Påbörja nästa state i kollektionen
                    States[_idx].Start();
            }
        }

        /// <see cref="GameState.ShouldEnd"/>
        protected override bool ShouldEnd()
        {
            return _idx >= States.Count; // Om vi nått slutet av kollektionen
        }

        /// <summary>
        /// Virtuell metod som kan overridas av child-klasser då kollektionen startas.
        /// </summary>
        protected virtual void OnCollectionStart() { }

    }
}
