using API;
using System;
using System.Collections.Generic;
using API.Net;
using System.Threading;
using System.IO;
using System.Linq;
using API.Net.Packets;
using Server.Command;
using Server.Games;
using Server.History;
using Server.Net;
using Server.Store;

namespace Server
{
    /// <summary>
    /// Ingångspunkten till servern. Håller i underliggande TCP-server och håller i flera olika spel.
    /// </summary>
    public class Server
    {

        private const string StartMessage = 
            "   _____                       _   _   _____                     \n" +
            "  / ____|                     | \\ | | |  __ \\                    \n" +
            " | |  __ _   _  ___  ___ ___  |  \\| | | |  | |_ __ __ ___      __\n" +
            " | | |_ | | | |/ _ \\/ __/ __| | . ` | | |  | | '__/ _` \\ \\ /\\ / /\n" +
            " | |__| | |_| |  __/\\__ \\__ \\ | |\\  | | |__| | | | (_| |\\ V  V / \n" +
            "  \\_____|\\__,_|\\___||___/___/ |_| \\_| |_____/|_|  \\__,_| \\_/\\_/  \n \n";

        public const string WordListFile = "words.txt";
        private const int MaxPlayers = 8;

        private readonly List<Game> _games;
        private readonly SimpleTcpServer _server;
        private readonly PacketSerializer _packetSerializer;
        private HashSet<string> _words;

        private readonly JsonStore<Guid, GameHistory> _historyStore;
        private readonly List<ConsoleCommand> _commands;

        public Server()
        {
            _packetSerializer = new PacketSerializer();
            _games = new List<Game>();
            _commands = new List<ConsoleCommand>();
            _historyStore = new JsonStore<Guid, GameHistory>("history.json");

            TcpEvents events = new TcpEvents();
            events.Message += OnMessage;
            events.Connect += OnConnect;
            events.Disconnect += OnDisconnect;
            _server = new SimpleTcpServer("0.0.0.0", 17532, events);
        }

        /// <summary>
        /// Startar servern, skapar ett nytt spel och lyssnar kontinuerligt efter input från konsolen.
        /// </summary>
        public void Start()
        {
            WriteConsole(StartMessage, ConsoleColor.Blue, false);
            WriteConsole("Loading history data...");
            _historyStore.LoadAll();
            
            try
            {
                _words = File.ReadAllLines(WordListFile).ToHashSet(); // Ladda in listan med ord som spelarna ska kunna gissa
            }
            catch (IOException)
            {
                WriteConsole("Unable to load wordlist, do you have a words.txt file in the working directory?", ConsoleColor.Red);
            }
            
            if (_words == null)
                return;
            
            WriteConsole("Registering commands...");
            _commands.Add(new ShutdownCommand());
            _commands.Add(new WordCommand(_words));
            _commands.Add(new HistoryCommand(_historyStore));
            _commands.Add(new HelpCommand(_commands));

            CreateGame();
            
            if (!_server.Start()) // Om TCP-servern inte kunde startas.
            {
                Thread.Sleep(5000); // Ge användaren tid att läsa meddelandet.
                return;
            }
            
            HandleCommandInput("help"); // Skicka ett fake help-kommando för att visa kommandolistan vid start.
            WriteConsole("Listening for messages...", ConsoleColor.Green);

            while (true)
            {
                // Hantera kommandon som skickas i konsolen.
                string input = Console.ReadLine();
                if (input == null)
                    break;
                
                if(!HandleCommandInput(input))
                    break;
            }
        }

        /// <summary>
        /// Hanterar ett kommando som skickas i konsolen.
        /// </summary>
        /// <param name="input">Meddelandet som skickades i konsolen.</param>
        /// <returns>Om servern ska fortsätta lyssna efter kommandon eller helt enkelt stängas.</returns>
        private bool HandleCommandInput(string input)
        {
            string[] raw  = input.Split(" ");

            string commandName = raw[0];
            string[] args = raw.Skip(1).ToArray();

            ConsoleCommand command = _commands.Find(command => command.Name.Equals(commandName));
            if (command == null)
            {
                Console.WriteLine("Unknown command, type help for help.");
                return true;
            }

            return command.OnExecute(args);
        }

        /// <summary>
        /// Skapar ett nytt spel som spelare kan gå med i.
        /// </summary>
        private Game CreateGame()
        {
            Game game = new Game(Guid.NewGuid(), this, new List<string>(_words));
            _games.Add(game);
            WriteConsole($"Created new game with id {game.Id}");
            return game;
        }

        /// <summary>
        /// Hanterar då ett spel avslutas, skapar b.la. ett nytt spel.
        /// </summary>
        /// <param name="game">Spelet som avslutas.</param>
        public void OnGameEnd(Game game)
        {
            WriteConsole( $"Game with ID {game.Id} ended.");
            _games.Remove(game);
            _historyStore.Add(game.CreateHistory());
            CreateGame();
        }

        /// <summary>
        /// Avslutar en spelares anslutning till servern.
        /// </summary>
        /// <param name="clientId">Klientens id.</param>
        public void Disconnect(int clientId)
        {
            TcpConnection connection = _server.GetConnectionById(clientId);
            if (connection == null)
                return;

            _server.Disconnect(connection);     
        }

        /// <summary>
        /// Hämtar kontext (spelarens objekt och spelet som spelaren tillhör) beroende på spelarens ID
        /// </summary>
        /// <param name="playerId">Spelarens ID.</param>
        /// <returns>En tuple med spelet som spelaren tillhör och instansen av spelaren.</returns>
        public Tuple<Game, Player> GetGamePlayerContext(int playerId)
        {
            Player result = null;
            Game game = _games.Find(game =>
            {
                Player player = game.GetPlayerById(playerId);
                if (player == null) // Kollar om spelaren finns med i spelet.
                    return false;

                result = player;
                return true;
            });

            return result == null ? null : new Tuple<Game, Player>(game, result);
        }

        /// <summary>
        /// Skickar ett paket till klienten med ett visst ID.
        /// </summary>
        /// <param name="clientId">Klientens id (spelare-id).</param>
        /// <param name="packet">Paketet som ska skickas.</param>
        public void SendPacket(int clientId, Packet packet)
        {
            TcpConnection connection = _server.GetConnectionById(clientId);
            if(connection == null)
                return;

            SendPacket(connection, packet);
        }
        
        /// <summary>
        /// Skickar ett paket till en TCP-anslutning.
        /// </summary>
        /// <param name="connection">Anslutningen som paketet ska skickas till.</param>
        /// <param name="packet">Paketet som ska skickas.</param>
        private void SendPacket(TcpConnection connection, Packet packet)
        {
            byte[] bytes = _packetSerializer.Serialize(packet);
            _server.WriteClient(connection, bytes);
        }
        
        /// <summary>
        /// Hittar ett lämpligt spel för en ny spelare att ansluta till.
        /// </summary>
        /// <returns>Ett lämpligt spel.</returns>
        private Game FindSuitableGame()
        {
            List<Game> sorted = _games
                .Where(game => game.Players.Count < MaxPlayers && game.Started)
                .OrderBy(g => g.Players.Count)
                .ToList(); // Spel som inte är fulla och som har minst spelare.
            
            return sorted.Count >= 1 ? sorted[0] : CreateGame();
        }
        
        /// <summary>
        /// Sätter egenskaper på ett inkommande paket som t.ex Sender genom att försöka hitta spelaren i ett pågående spel.
        /// </summary>
        /// <param name="packet">Det inkommande paketet.</param>
        /// <param name="senderId">Klient-id som skickat paketet.</param>
        /// <returns></returns>
        private Tuple<Game, Player> SetIncomingPacketContext(IncomingPacket packet, int senderId)
        {
            packet.SenderId = senderId;
            Tuple<Game, Player> context = GetGamePlayerContext(senderId);
            if (context != null)
            {
                packet.Sender = context.Item2;
            }

            return context;
        }
        
        /// <summary>
        /// Skriver ett meddelande till standard-konsolen.
        /// </summary>
        /// <param name="message">Meddelandet att skriva</param>
        /// <param name="color">Färgen att skriva meddelandet i.</param>
        /// <param name="showPrefix">Om meddelandet ska ha standard-prefixet.</param>
        public static void WriteConsole(string message, ConsoleColor color = ConsoleColor.White, bool showPrefix = true)
        {
            Console.ForegroundColor = color;
            string prefix = showPrefix ? "[*] " : "";

            Console.WriteLine($"{prefix}{message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* TCP Event Handlers */
        private void OnMessage(object s, TcpMessageEventArgs args)
        {
            int senderId = args.Connection.Id;
            Packet packet = _packetSerializer.Deserialize(args.Result);
            if (!(packet is IncomingPacket incomingPacket)) // Om det deserialiserade paketet inte är av rätt typ, avbryt.
                return;

            Tuple<Game, Player> context = SetIncomingPacketContext(incomingPacket, senderId);
            Game game;
            if (context == null) // Detta är en ny spelare 
            {
                game = FindSuitableGame();
            }
            else
            {
                game = context.Item1;
            }
            
            if(game == null) // Borde inte hända.
                return;

            game.HandleIncomingPacket(incomingPacket);
        }
        
        private void OnConnect(object s, TcpConnectionEventArgs args)
        {

            WriteConsole($"Client with ID {args.Connection.Id} connected.");
        }
        
        private void OnDisconnect(object s, TcpDisconnectionEventArgs args)
        {
            int senderId = args.Connection.Id;
            WriteConsole($"Client with ID {senderId} disconnected");

            DisconnectPacket packet = new DisconnectPacket();
            Tuple<Game, Player> context = SetIncomingPacketContext(packet, senderId);
            
            if (context != null)
            {
                context.Item1.HandleIncomingPacket(packet);                
            }
        }

    }
}