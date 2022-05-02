using System;
using System.Collections.Generic;
using System.Drawing;
using API;
using API.Drawing;
using API.Net.Packets;
using Server.Games;

namespace Server.State.Games
{
    /// <summary>
    /// Huvudstatet då spelare ritar och gissar.
    /// </summary>
    public class DrawState : GameState
    {
        
        private const int DrawBoxWidth = 761;
        private const int DrawBoxHeight = 470;
        private const int MaxDrawSize = 10;
        
        private const int MaxGuessScore = 100;
        private const double MaxDrawBonus = 200;
        private const int MinPlayersForBonus = 4;
        private const int MinGuessedForBonus = 2;

        private const char WordHintHiddenChar = '_';
        
        private readonly int _round;
        private readonly Player _drawer;
        private readonly string _word; // Ordet som ska gissas

        private readonly char[] _wordHint;

        private readonly List<DrawPacket> _drawHistory = new List<DrawPacket>(); 

        /// <summary>
        /// Skapar ett nytt DrawState.
        /// </summary>
        /// <param name="game">Spelet som statet tillhör</param>
        /// <param name="round">Den nuvarande rundan.</param>
        /// <param name="drawer">Spelaren som ska rita denna gången.</param>
        /// <param name="word">Ordet som ska ritas denna gången.</param>
        public DrawState(Game game, int round, Player drawer, string word) : base(game, TimeSpan.FromSeconds(80))
        {
            _round = round;
            _drawer = drawer;
            _word = word;

            // Initialisera ledtrådssträngen med bokstäverna gömda
            _wordHint = new char[_word.Length];
            for (int i = 0; i < _wordHint.Length; i++)
            {
                _wordHint[i] = WordHintHiddenChar;
            }
        }

        /* State kod */
        
        protected override void RegisterEvents(GameEvents events)
        {
            events.Chat += OnChat;
            events.Draw += OnDraw;
            events.Connect += OnConnect;
            events.Disconnect += OnDisconnect;
        }

        protected override void UnregisterEvents(GameEvents events)
        {
            events.Chat -= OnChat;
            events.Draw -= OnDraw;
            events.Connect -= OnConnect;
            events.Disconnect -= OnDisconnect;
        }

        protected override void OnStart()
        {
            Game.CurrentRound = _round;

            Game.Players.ForEach(Init);
            Game.BroadcastPacket(CreatePlayerStatePacket());
            Game.SendPacket(_drawer, new WordUpdatePacket(_word));
        }

        protected override void OnUpdate()
        {
            int remainingTime = (int) Math.Floor(RemainingTime.TotalSeconds);
            double timeStep = Duration.TotalSeconds / _word.Length;
            
            if(remainingTime % (int) timeStep == 0) // Avslöja mer av ledtråden relativt till ordets längd, vid sekund 0 är hela ordet visat.
            {
                List<int> hidden = new List<int>(); // Skapa en lista över bokstäver index som fortfarande är gömda
                for (var i = 0; i < _wordHint.Length; i++)
                {
                    if (_wordHint[i] == WordHintHiddenChar)
                    {
                        hidden.Add(i);
                    }
                }

                int idx = hidden[Game.GameRandom.Next(hidden.Count)]; // Random index på en bokstav som är gömd
                _wordHint[idx] = _word[idx]; // Visa bokstaven

                Game.Players
                    .FindAll(p => p.State == PlayerState.Guessing)
                    .ForEach(SendWordHint);
            }
        }

        protected override void OnEnd()
        {
            Game.BroadcastMessage($"Ordet var \"{_word}\"!", Color.Chocolate);
            
            // Bonus då ritaren ritat en bra bild som många kunde gissa.
            if (Game.Players.Count >= MinPlayersForBonus) // Det måste finnas minst 4 spelare i spelet för att den som ritar ska få bonus.
            {
                (int guessing, int guessed) = GetGuessedStatistics();
                int total = guessing + guessed;
                int bonus = (int) (guessed / (double) total * MaxDrawBonus); // Ritaren får en del av bonusen beroende på hur många som gissat rätt, om alla gissade rätt får hen hela bonusen.

                if (guessed >= MinGuessedForBonus) // Flera måste ha gissat för att bonusen ska träda i kraft
                {
                    _drawer.Score += bonus;
                    Game.BroadcastMessage($"{_drawer.Name} får en bonus eftersom hen ritade så fint!",
                        Game.DefaultMessageColor);
                    Game.BroadcastPacket(new PlayerScorePacket(_drawer.Id, _drawer.Score));
                }
            }

            // Återställ ritarens state
            _drawer.State = PlayerState.Guessing;
            Game.BroadcastPacket(new PlayerStatePacket(_drawer.Id, _drawer.State));
            Game.BroadcastPacket(CreateGameStatePacket(false));
        }

        protected override bool ShouldEnd()
        {
            return base.ShouldEnd() || // Om tiden är slut
                   Game.GetActivePlayers().FindAll(p => p.State == PlayerState.Guessing).Count == 0 || // Om alla gissat
                   !Game.Players.Contains(_drawer); // Om ritaren har lämnat
        }

        /// <summary>
        /// Kalkylerar hur många som gissar och som gissat ordet.
        /// </summary>
        /// <returns>Tuple med (guessing, guessed).</returns>
        private Tuple<int, int> GetGuessedStatistics()
        {
            int guessing = 0;
            int guessed = 0;
            foreach (Player player in Game.Players)
            {
                if (player.State == PlayerState.Guessed)
                {
                    guessed++;
                } else if (player.State == PlayerState.Guessing)
                {
                    guessing++;
                }
            }

            return new Tuple<int, int>(guessing, guessed);
        }

        /// <summary>
        /// Skickar den nuvarande ledtråden till spelaren.
        /// </summary>
        /// <param name="player">Spelaren att skicka paketet till.</param>
        private void SendWordHint(Player player)
        {
            Game.SendPacket(player, new WordUpdatePacket(new string(_wordHint)));
        }

        /// <summary>
        /// Skapar ett GameStatePacket med information om spelets nuvarande skick.
        /// </summary>
        /// <param name="start">Om vi är i början eller slutet av statet.</param>
        /// <returns>Nytt GameStatePacket.</returns>
        private GameStatePacket CreateGameStatePacket(bool start)
        {
            return new GameStatePacket(start ? API.State.GameStateType.Draw : API.State.GameStateType.PostDraw,
                (int) Math.Floor(RemainingTime.TotalSeconds), _round);
        }
        
        /// <summary>
        /// Skapar ett paket med information om state för alla spelare i spelet.
        /// </summary>
        /// <returns>Nytt PlayerStatePacket.</returns>
        private PlayerStatePacket CreatePlayerStatePacket()
        {
            Dictionary<int, PlayerState> states = new Dictionary<int, PlayerState>();
            Game.Players.ForEach(p => states[p.Id] = p.State);
            return new PlayerStatePacket(states);
        }

        /// <summary>
        /// Initialiserar statet för spelaren, sätter player states och skickar paket för
        /// att spelarens klient ska ha kontext kring ritandet.
        /// </summary>
        /// <param name="player">Spelare att initialisera statet för.</param>
        private void Init(Player player)
        {
            player.State = player.Equals(_drawer) ? PlayerState.Drawing : PlayerState.Guessing;

            Game.SendPacket(player, CreateGameStatePacket(true));
            Game.SendMessage(player, $"{_drawer.Name} är ritaren!", Game.DefaultMessageColor);
            SendWordHint(player);
        }

        /* Event handlers */

        private void OnConnect(object s, ConnectPacket packet)
        {
            if(packet.Cancelled)
                return;
            
            // Om en spelare ansluter under tiden som någon ritar, skicka Start-informationen enskilt till den spelaren.        

            Player player = packet.Sender;
            Init(player);

            _drawHistory.ForEach(drawPacket => Game.SendPacket(player, drawPacket)); // Skicka alla tidigare DrawPackets direkt så att spelaren får se ritningen.
            
            Game.SendPacket(player, CreatePlayerStatePacket());
            Game.BroadcastPacket(new PlayerStatePacket(player.Id, player.State));
            Game.SendMessage(player, "Du kan först rita nästa runda eftersom spelet redan pågår.", Game.DefaultMessageColor);
        }

        private void OnDisconnect(object sender, DisconnectPacket packet)
        {
            if (packet.Sender.Equals(_drawer))
            {
                Game.BroadcastMessage("Ritandet avslutades eftersom ritaren lämnade spelet.", Game.DefaultMessageColor);
            }
        }
        
        private void OnChat(object s, ChatPacket packet)
        {
            if (packet.Cancelled)
                return;

            Player sender = packet.Sender;
            string message = packet.Message;

            if (sender.State != PlayerState.Guessing) // Endast spelare som gissar ska kunna skicka meddelanden.
            {
                Game.SendMessage(sender, "Du kan bara skicka meddelanden när du ska gissa!", Game.ErrorMessageColor);
                return;
            }

            if (message.Equals(_word, StringComparison.OrdinalIgnoreCase)) // När spelaren gissade rätt
            {
                (int guessing, int guessed) = GetGuessedStatistics();
                int total = guessing + guessed;
                
                sender.State = PlayerState.Guessed;
                sender.Score += MaxGuessScore - (int)(MaxGuessScore * guessed / (double) total); // Ge bonus beroende på hur många som redan gissat ordet, om man är först är guessed / total 0 vilket ger spelaren max-poäng.
                
                Game.BroadcastMessage($"{sender.Name} gissade rätt!", Color.Lime);
                Game.SendPacket(sender, new WordUpdatePacket(_word)); // När spelaren gissat rätt kan vi visa hela ordet för hen.
                Game.BroadcastPacket(new PlayerScorePacket(sender.Id, sender.Score));
                Game.BroadcastPacket(new PlayerStatePacket(sender.Id, sender.State));
            }
            else
            {
                Game.BroadcastPacket(new MessagePacket(packet.SenderId, Color.Black, packet.Message));
            }
        }

        private void OnDraw(object s, DrawPacket packet)
        {
            // Bara spelaren som ritar ska kunna rita.
            if (!packet.Sender.Equals(_drawer))
                return;

            DrawAction action = packet.Action;
            if (action == null)
                return;
            
            if (action.Points != null)
            {
                // Ta bort rithändelser som är utanför de giltiga gränserna.
                action.Points.RemoveAll(p => p.X < 0 || p.X > DrawBoxWidth || p.Y < 0 || p.Y > DrawBoxHeight);
            }

            if (!action.Instant && (action.Size < 1 || action.Size > MaxDrawSize)) // Avbryt om storleken är ogiltig
                return;

            _drawHistory.Add(packet);
            Game.GetActivePlayers()
                .FindAll(p => !p.Equals(_drawer)) // Ritaren ritar detta själv på klientsidan.
                .ForEach(player => Game.SendPacket(player, packet));
        }
        
    }
}