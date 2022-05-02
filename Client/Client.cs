using System;
using API;
using System.Collections.Generic;
using API.Net;
using API.Net.Packets;
using Client.Net;
using System.Threading.Tasks;
using System.Linq;
using API.State;

namespace Client
{
    /// <summary>
    /// Spelets klient. Håller i TCP-klienten men även information om spelarna i spelet.
    /// Den är mellanhanden mellan den råa TCP-datan och user interfacet.
    /// </summary>
    public class Client
    {

        private IGameForm _form;
        private readonly SimpleTcpClient _tcpClient;
        private List<Player> _players;
        private readonly PacketSerializer _packetSerializer;

        private GameStateType _lastGameState;

        private Avatar _avatar;
        private string _name;

        private readonly IStartForm _startForm;
        private Player _self;

        private bool _connected;
        private bool _gracefulDisconnect;

        /// <summary>
        /// Skapar en ny Client, sätter upp TCP-client och event handlers.
        /// </summary>
        /// <param name="startForm">Start-formuläret som vi övergår till när spelet är slut eller om ett fel inträffade.</param>
        public Client(IStartForm startForm)
        {
            _startForm = startForm;
            _packetSerializer = new PacketSerializer();
            _players = new List<Player>();

            TcpEvents events = new TcpEvents();
            _tcpClient = new SimpleTcpClient(events);
            events.Message += OnMessage;
            events.Connect += OnConnect;
            events.Disconnect += OnDisconnect;
        }

        /// <summary>
        /// Ansluter till en server.
        /// </summary>
        /// <param name="host">Värd att ansluta till.</param>
        /// <param name="port">Port att ansluta till.</param>
        /// <param name="gameForm">Formuläret som ska hantera spelet.</param>
        /// <param name="avatar">Avataren att ansluta med.</param>
        /// <param name="name">Namnet att ansluta med.</param>
        public void Connect(string host, int port, IGameForm gameForm, Avatar avatar, string name)
        {
            _form = gameForm;
            _avatar = avatar;
            _name = name;
            _gracefulDisconnect = false;
            _connected = false;
            _tcpClient.Connect(host, port);
        }
        
        /// <summary>
        /// Avbryter anslutningen till servern.
        /// </summary>
        public void Disconnect()
        {
            SendPacket(new DisconnectPacket());
            _gracefulDisconnect = true;
        }
        
        /// <summary>
        /// Skickar ett paket till servern.
        /// </summary>
        /// <param name="packet">Paket att skicka.</param>
        public void SendPacket(Packet packet)
        {
            _tcpClient.SendData(_packetSerializer.Serialize(packet));
        }

        /// <summary>
        /// Hittar en spelare med det specifierade ID:et.
        /// </summary>
        /// <param name="id">Spelarens ID.</param>
        /// <returns>Spelaren eller null.</returns>
        private Player GetPlayerById(int id)
        {
            return _players.Find(player => player.Id == id);
        }
        
        /// <summary>
        /// Skapar en rangordnad lista med alla spelare efter poäng.
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<Player> GetLeaderboard()
        {
            return _players.OrderByDescending(p => p.Score);
        }

        /// <summary>
        /// Kollar om spelaren som klienten själv har är ritaren.
        /// </summary>
        /// <returns>Om klienten är ritaren.</returns>
        public bool IsDrawing()
        {
            return _self.State == PlayerState.Drawing;
        }

        // TCP Event Handlers
        private void OnConnect(object s, TcpConnectionEventArgs args)
        {
            _form.OnConnect();
            _startForm.HandleConnect();
            SendPacket(new ConnectPacket(_name, _avatar)); // När en anslutning till TCP-servern upprättats, skicka information om spelarens valda egenskaper.

            _avatar = null;
            _name = null;
            _connected = true;
        }

        private void OnDisconnect(object s, TcpDisconnectionEventArgs args)
        {
            if (_gracefulDisconnect)
                return;

            if (_connected) // Om spelet faktiskt haft en anslutning till servern, annars har klienten förmodligen nätverksproblem.
            {
                _form.Disconnect("Förlorade anslutning till servern.", false);
            } else
            {
                _startForm.HandleFailedConnect();
            }
        }

        private void OnMessage(object s, TcpMessageEventArgs args)
        {
            Packet packet = _packetSerializer.Deserialize(args.Result);
            if (packet == null)
                return;

            switch (packet) // Bästa valet, en dictionary med delegate skulle se bättre ut men ge ganska mycket overhead.
            {
                case KickPacket p:
                {
                    OnKick(p);
                    break;
                }
                case MessagePacket p:
                {
                    OnMessage(p);
                    break;
                }
                case PlayerScorePacket p:
                {
                    OnPlayerScore(p);
                    break;
                }
                case WordUpdatePacket p:
                {
                    OnWordUpdate(p);
                    break;
                }
                case GameStatePacket p:
                {
                    OnGameStateUpdate(p);
                    break;
                }
                case PlayerStatePacket p:
                {
                    OnPlayerStateUpdate(p);
                    break;
                }
                case DrawPacket p:
                {
                    OnDraw(p);
                    break;
                }
                case PlayerInfoPacket p:
                {
                    OnPlayerInfo(p);
                    break;
                }
            }
        }

        // Packet Event Handlers
        private void OnDraw(DrawPacket p)
        {
            _form.Draw(p.Action, p.ElementId);
        }
        
        private void OnMessage(MessagePacket packet)
        {
            Player player = GetPlayerById(packet.MessageSenderId); // Spelaren kan vara null om det är servern som skickat meddelandet.
            _form.AddChatMessage(player, packet.Message, false, packet.Color);
        }

        private void OnPlayerScore(PlayerScorePacket packet)
        {
            Player player = GetPlayerById(packet.PlayerId);
            if (player == null)
                return;

            player.Score = packet.Score;
            _form.UpdatePlayerList(_players);
        }

        private void OnKick(KickPacket packet)
        {
            _connected = false;
            _form.Disconnect(packet.Reason, true);
        }

        private void OnPlayerStateUpdate(PlayerStatePacket packet)
        {
            foreach (KeyValuePair<int, PlayerState> entry in packet.States)
            {
                Player player = GetPlayerById(entry.Key);
                if (player != null)
                {
                    player.State = entry.Value;
                }
            }
            
            // Nu har states uppdaterats, toggla ritverktyen beroende på om spelaren själv är ritaren.
            _form.SetDrawToolsVisible(_self.State == PlayerState.Drawing);
        }

        private void OnGameStateUpdate(GameStatePacket packet)
        {
            bool showTimer = packet.Type == GameStateType.Draw; // Visa bara timern om det är ritdags.
            _form.UpdateGameFormState(packet.Round, packet.TimeLeft, showTimer);

            switch (packet.Type)
            {
                case GameStateType.PostDraw: // Ritandet är slut, göm ordet och rensa ritytan.
                    {
                        _form.UpdateWord(""); 
                        _form.ClearCanvas();
                        
                        // Visa topplistan i 3 sekunder
                        _form.ShowLeaderboard();
                        Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith(t =>
                        {
                            // Endast topplistan ska försvinna, andra overlays kan visas direkt efter eller utan topplistan.
                            if (_self.State == PlayerState.Guessing && _lastGameState != GameStateType.PostGame)
                            {
                                _form.SetOverlayVisible(false);
                            }
                        });
                        
                        break;
                    }

                case GameStateType.PostGame:
                    {
                        _form.ShowPostGameScreen();
                        break;
                    }
            }

            _lastGameState = packet.Type;
        }

        private void OnWordUpdate(WordUpdatePacket packet)
        {
            _form.UpdateWord(packet.Word);
            
            if(_self.State == PlayerState.Drawing) 
            {
                _form.ShowDrawerNotice(packet.Word); // Nu är ordet redo och spelaren är ritaren, visa overlayen.
            }
        }

        private void OnPlayerInfo(PlayerInfoPacket packet)
        {
            Player[] players = packet.Info;
            switch (packet.Mode)
            {
                case PlayerInfoPacket.InfoMode.Add:
                    {
                        _players.AddRange(players);
                        break;
                    }
                case PlayerInfoPacket.InfoMode.Remove:
                    {
                        int playerId = players.Length > 0 ? players[0].Id : -1; // Servern tar aldrig bort mer än en spelare, och ingen kommer att ha ID -1
                        _players.RemoveAll(pl => pl.Id == playerId);
                        break;
                    }
                case PlayerInfoPacket.InfoMode.Initial:
                    {
                        _players = new List<Player>(players);
                        break;
                    }
                case PlayerInfoPacket.InfoMode.Self:
                    {
                        _self = GetPlayerById(players[0].Id);
                        break;
                    }
            }

            _form.UpdatePlayerList(_players);
        }

    }
}
