using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using API;
using API.History;
using API.Net;
using API.Net.Packets;
using Server.History;
using Server.State;
using Server.State.Games;

namespace Server.Games
{
    /// <summary>
    /// Ett spel i systemet.
    /// </summary>
    public class Game
    {

        public const int MaxRounds = 3;
        public const int PlayersRequired = 2;
        private const int MinNameLength = 2;
        private const int MaxNameLength = 16;

        public static Color DefaultMessageColor = Color.Blue;
        public static Color ErrorMessageColor = Color.Red;
        public static Color ConnectionMessageColor = Color.Purple;
        
        private const int MaxChatMessageLength = 70;
        private const int ChatSpamThreshold = 6;
        private const int ChatSpamCooldown = 2;

        public const string DefaultWord = "hus";
        private static readonly Color[] SupportedAvatarColors = new Color[] { Color.Red };

        /// <summary>
        /// Spelets unika ID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Lista över aktiva spelare i spelet.
        /// </summary>
        public List<Player> Players { get; }
        /// <summary>
        /// Spelets nuvarande runda.
        /// </summary>
        public int CurrentRound { get; set; }
        /// <summary>
        /// Spelets event subscriber för att registrera event handlers på.
        /// </summary>
        public GameEvents Events { get; }
        /// <summary>
        /// Spelets Random-instans.
        /// </summary>
        public Random GameRandom { get; }
        /// <summary>
        /// Spelets lista över ord.
        /// </summary>
        public List<string> Words { get; }
        
        private readonly List<HistoryPoint> _history;
        private readonly Server _server;
        private GameStateCollection _mainState;
        private long _timeStarted;
        private long _timeEnded;
        private Timer _timer;

        /// <summary>
        /// Skapar ett nytt Game.
        /// </summary>
        /// <param name="id">Unikt ID för spelet.</param>
        /// <param name="server">Servern som hanterar spelet.</param>
        /// <param name="wordList">Lista med ord som ska förekomma i spelet.</param>
        public Game(Guid id, Server server, List<string> wordList)
        {
            Id      = id;
            Events  = new GameEvents();
            Players = new List<Player>();
            _history = new List<HistoryPoint>();
            Words = new List<string>(wordList); // Klona listan för att kunna mutera den utan att påverka andra spel.
            GameRandom = new Random();
            _server = server;
            
            Events.Connect += OnConnect;
            Events.Disconnect += OnDisconnect;
            Events.Chat += OnChat;
        }

        /// <summary>
        /// Startar spelet. Startar timern, sätter upp state machine med states.
        /// </summary>
        private void Start()
        {
            // Uppdatera states varje sekund
            _timer = new Timer();
            _timer.Elapsed += Timer_Tick;
            _timer.Interval = 1000;
            _timer.Start();

            List<GameState> states = new List<GameState>();
            states.Add(new LobbyState(this));
            for (int i = 0; i < 3; i++)
            {
                states.Add(new RoundState(this, i + 1));
                states.Add(new PostRoundState(this));
            }

            states.Add(new PostGameState(this));
            _mainState = new GameStateCollection(this, states);
            _mainState.Start();
            
            _timeStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        private void Timer_Tick(object s, EventArgs args)
        {
            _mainState.Update();
        }
        
        /// <summary>
        /// Sänder ett paket till alla spelare i spelet.
        /// </summary>
        /// <param name="packet">Paketet som ska skickas.</param>
        public void BroadcastPacket(Packet packet)
        {
            Players.ForEach(player => SendPacket(player, packet));
        }

        /// <summary>
        /// Sänder ett paket till en enskild spelare.
        /// </summary>
        /// <param name="player">Spelare som paketet ska skickas till.</param>
        /// <param name="packet">Paketet som ska skickas.</param>
        public void SendPacket(Player player, Packet packet)
        {
            _server.SendPacket(player.Id, packet);
        }

        /// <summary>
        /// Sänder ett meddelande i chatten till en enskild spelare.
        /// </summary>
        /// <param name="player">Spelare att skicka meddelandet till.</param>
        /// <param name="message">Meddelandet som ska skickas.</param>
        /// <param name="color">Färgen på meddelandet.</param>
        public void SendMessage(Player player, string message, Color color)
        {
            SendPacket(player, new MessagePacket(0, color, message));
        }

        /// <summary>
        /// Sänder ett meddelande i chatten till alla spelare i spelet.
        /// </summary>
        /// <param name="message">Meddelandet som ska skickas.</param>
        /// <param name="color">Färgen på meddelandet.</param>
        public void BroadcastMessage(string message, Color color)
        {
            AddHistory(new BroadcastMessageHistory(message));
            Players.ForEach(p => SendMessage(p, message, color));
        }

        /// <summary>
        /// Hanterar ett inkommande paket.
        /// </summary>
        /// <param name="packet">Paketet som ska skickas.</param>
        public void HandleIncomingPacket(IncomingPacket packet)
        {
            if(packet == null)
                return;
            
            Events.DispatchEvent(packet);
            if(packet is IHistory history) // Inkommande paket ska ofta sparas i historiken.
                AddHistory(history);
        }

        /// <summary>
        /// Hittar en spelare beroende på dess ID.
        /// </summary>
        /// <param name="playerId">Spelarens id</param>
        /// <returns>Funnen spelare eller null om ingen sådan finns i spelet.</returns>
        public Player GetPlayerById(int playerId)
        {
            return Players.Find(player => player.Id == playerId);
        }

        /// <summary>
        /// Hämtar alla aktiva spelare i spelet.
        /// </summary>
        /// <returns></returns>
        public List<Player> GetActivePlayers()
        {
            return Players.FindAll(player => player.IsActive());
        }
        
        /// <summary>
        /// Lägger till en ny händelse i historiken.
        /// </summary>
        /// <param name="history">Händelsen som ska läggas till.</param>
        public void AddHistory(IHistory history)
        {
            long timeMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _history.Add(new HistoryPoint(timeMillis, history));
        }

        /// <summary>
        /// Skapar ett nytt <see cref="GameHistory"/> objekt för detta spelet.
        /// </summary>
        /// <returns></returns>
        public GameHistory CreateHistory()
        {
            return new GameHistory(Id, _timeStarted, _timeEnded, Players, _history);
        }

        /// <summary>
        /// Avslutar spelet.
        /// </summary>
        public void End()
        {
            if(_mainState == null) // Spelet är redan slut.
                return;
            
            BroadcastPacket(new KickPacket("Spelet är slut."));
            _mainState.End();
            _mainState = null;
            _timeEnded = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _server.OnGameEnd(this);
        }

        /// <summary>
        /// Om spelet har startat.
        /// </summary>
        public bool Started => _timeStarted != 0;

        /* Event Handlers */
        
        private void OnConnect(object sender, ConnectPacket connectPacket)
        {
            string cleanedName = connectPacket.PlayerName.Trim()
                .Replace("\n", "").Replace("\r", ""); // Rensar namnet från otillåtna tecken.
            
            ServerPlayer player = new ServerPlayer(connectPacket.SenderId, 
                cleanedName, connectPacket.Avatar, 0); // Skapa en ny spelare med datan från paketet.
         
            if (player.Name.Length < MinNameLength || player.Name.Length > MaxNameLength)
            {
                SendPacket(player, new KickPacket("Ogiltigt namn!"));
                connectPacket.Cancelled = true;
                return;
            }

            if (SupportedAvatarColors.All(c => connectPacket.Avatar.Color != c))
            {
                SendPacket(player, new KickPacket("Ogiltig avatar!"));
                connectPacket.Cancelled = true;
                return;
            }

            int duplicateNameCount = Players.FindAll(p => {
                ServerPlayer sp = p as ServerPlayer;
                if (sp == null)
                    return false;
                
                return sp.Name.Equals(player.Name) || sp.OriginalName != null && sp.OriginalName.Equals(player.Name); 
            }).Count;

            if (duplicateNameCount > 0) // Om två spelare valt samma namn (case sensitive), lägg till ett suffix på namnet.
            {
                player.OriginalName = player.Name;
                player.Name = $"{player.Name} {duplicateNameCount}";
            }

            BroadcastPacket(new PlayerInfoPacket(player, PlayerInfoPacket.InfoMode.Add));

            Players.Add(player);
            connectPacket.Sender = player;

            if (_mainState == null) // Det är den första spelaren som ansluter, initialisera spelet.
                Start();

            BroadcastMessage($"{player.Name} anslöt sig till spelet.", ConnectionMessageColor);

            SendPacket(player, new PlayerInfoPacket(Players.ToArray()));
            SendPacket(player, new PlayerInfoPacket(new Player[] { player }, PlayerInfoPacket.InfoMode.Self));
            SendPacket(player, new MessagePacket(0, DefaultMessageColor, "Välkommen till Guess N' Draw!"));
        }

        private void OnChat(object sender, ChatPacket chatPacket)
        {
            ServerPlayer player = chatPacket.Sender as ServerPlayer;
            if(player == null)
                return;
            
            int msgLength = chatPacket.Message.Length;
            bool tooLong = msgLength > MaxChatMessageLength;
            if(msgLength < 1 || tooLong)
            {
                chatPacket.Cancelled = true;

                if (tooLong) // Meddelande behöver bara visas om meddelandet är för långt.
                    SendMessage(player, "Ditt meddelande är för långt!", ErrorMessageColor);

                return;
            }
            
            bool shouldCancel = false;
            if (player.ChatCooldown.HasValue)
            {
                if (player.ChatCooldown.Value.CompareTo(DateTime.Now) > 0) // Om spelarens chat-cooldown inte är slut.
                {
                    shouldCancel = true;
                } else
                {
                    player.ChatCooldown = null;
                }   
            }

            // Om spelaren har skickat 3 meddelanden på högst 5 sekunder spammar hen troligen.
            // Ge spelaren en cooldown om den inte har en pågående cooldown.
            if (!shouldCancel && player.LastMessageTimestamps.Count >= 3)
            {
                if (DateTime.Now.Subtract(player.LastMessageTimestamps[0]).TotalSeconds <= ChatSpamThreshold)
                {
                    player.LastMessageTimestamps.Clear();
                    player.ChatCooldown = DateTime.Now.Add(TimeSpan.FromSeconds(ChatSpamCooldown)); // 2 sekunder cooldown

                    shouldCancel = true;
                } else
                {
                    player.LastMessageTimestamps.RemoveAt(0); // Advanca i listan genom att ta bort den första timestampen.
                }
            }

            if (shouldCancel)
            {
                SendMessage(player, "Ta det lungt med chattandet!", ErrorMessageColor);
                chatPacket.Cancelled = true;
                return;
            }

            player.LastMessageTimestamps.Add(DateTime.Now);
        }

        private void OnDisconnect(object sender, DisconnectPacket disconnectPacket)
        {
            Player player = disconnectPacket.Sender;
            if (player == null)
            {
                Console.Error.WriteLine($"Something went wrong when disconnecting player with network ID {disconnectPacket.SenderId}");
                return;
            }
            
            Players.Remove(player);
            _server.Disconnect(disconnectPacket.SenderId);

            BroadcastPacket(new PlayerInfoPacket(disconnectPacket.Sender, PlayerInfoPacket.InfoMode.Remove));
            BroadcastMessage($"{player.Name} lämnade spelet.", ConnectionMessageColor);

            if (Players.Count < 2 && Started) // Om det inte finns några spelare kvar i spelet, avsluta.
            {
                End();
            }
        }

        /// <summary>
        /// Skriver ett meddelande till konsolen.
        /// </summary>
        /// <param name="message">Meddelande att skriva.</param>
        public void WriteConsole(string message)
        {
            Server.WriteConsole($"[{Id}] {message}");
        }
        
    }
}