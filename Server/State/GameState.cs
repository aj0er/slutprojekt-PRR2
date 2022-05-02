using System;
using Server.Games;
using Server.History;

namespace Server.State
{
    /// <summary>
    /// Abstrakt klass för ett state som spelet befinner sig i. 
    /// Under detta state kan vissa specifika events lyssnas på.
    /// 
    /// </summary>
    public abstract class GameState
    {
        private DateTime? _timeEnd;
        /// <summary>
        /// Hur lång tid som statet ska vara.
        /// </summary>
        protected TimeSpan Duration { get; }

        /// <summary>
        /// Spelet som håller i statet.
        /// </summary>
        protected readonly Game Game;

        /// <summary>
        /// Om spelet har startat.
        /// </summary>
        public bool Started => _timeEnd.HasValue; // Om _timeEnd har ett värde har Start() metoden anropats.

        /// <summary>
        /// Om spelet har avslutats.
        /// </summary>
        public bool Ended { get; private set; }

        /// <summary>
        /// Skapar ett nytt state.
        /// </summary>
        /// <param name="game">Spelet som detta state tillhör.</param>
        protected GameState(Game game) : this(game, TimeSpan.Zero) { }

        /// <summary>
        /// Skapar ett nytt state, med en viss tidslängd.
        /// </summary>
        /// <param name="game">Spelet som detta state tillhör.</param>
        /// <param name="duration">Tidslängden av hur långt detta state ska vara.</param>
        protected GameState(Game game, TimeSpan duration)
        {
            Game = game;
            Duration = duration;
        }

        /// <summary>
        /// Virtuell metod som overridas av child-klasser, har ingen default implementation i denna klassen.
        /// Registrerar events som kommer att vara aktiva under detta State.
        /// </summary>
        /// <param name="events">Events subscriber med events.</param>
        protected virtual void RegisterEvents(GameEvents events) { }

        /// <summary>
        /// Virtuell metod som overridas av child-klasser, har ingen default implementation i denna klassen.
        /// Avregistrerar events som tidigare registrerats.
        /// </summary>
        /// <param name="events"></param>
        protected virtual void UnregisterEvents(GameEvents events) { }

        /// <summary>
        /// Startar detta state, <see cref="OnStart"/> anropas i child-klassen om det lyckades.
        /// </summary>
        public void Start()
        {
            if (Started || Ended)
                return;

            string stateName = GetType().Name;
            Game.WriteConsole($"State started: {stateName}");
            Game.AddHistory(new StateStartHistory(stateName));
            
            RegisterEvents(Game.Events);
            _timeEnd = DateTime.Now.Add(Duration);
            OnStart();
        }

        /// <summary>
        /// Uppdaterar detta state. Kollar om statet bör avslutas och gör det i så fall.
        /// </summary>
        public void Update()
        {
            if (!Started || Ended)
                return;

            if (ShouldEnd())
            {
                End();
                return;
            }

            OnUpdate();
        }

        /// <summary>
        /// Avslutar detta state, avregistrerar eventuella events.
        /// </summary>
        public void End()
        {
            if (!Started || Ended)
                return;

            string stateName = GetType().Name;
            Game.WriteConsole($"State ended: {stateName}");
            Game.AddHistory(new StateEndHistory(stateName));
            UnregisterEvents(Game.Events);
            Ended = true;
            OnEnd();
        }

        /// <summary>
        /// Abstrakt metod som anropas (endast en gång), då statet startar.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Abstrakt metod som anropas då statet ska uppdateras.
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// Abstrakt metod som anropas (endast en gång), då statet avslutas.
        /// </summary>
        protected abstract void OnEnd();

        /// <summary>
        /// Virtuell metod som kan overridas för att ha en egen implementation om statet borde avslutas.
        /// Default implementation kollar om tiden tagit slut.
        /// </summary>
        /// <returns>Om statet bör avslutas.</returns>
        protected virtual bool ShouldEnd()
        {
            return RemainingTime == TimeSpan.Zero;
        }

        /// <summary>
        /// Hur mycket tid som finns kvar innan statet avslutas.
        /// </summary>
        public TimeSpan RemainingTime
        {
            get
            {
                if (!_timeEnd.HasValue)
                    return Duration;

                TimeSpan span = _timeEnd.Value.Subtract(DateTime.Now);
                return span.TotalMilliseconds > 0 ? span : TimeSpan.Zero;
            }
        }
    }
}