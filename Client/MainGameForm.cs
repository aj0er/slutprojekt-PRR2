using API;
using Client.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using API.Drawing;
using API.Net.Packets;
using Client.Avatars;
using System.Linq;
using Timer = System.Timers.Timer;

namespace Client
{
    /// <summary>
    /// Det huvudsakliga formuläret som håller i spelet.
    /// Visas endast då spelaren är ansluten till spelet. 
    /// </summary>
    public partial class MainGameForm : Form, IGameForm
    {
        
        private const int ChatPanelWidth = 248;
        private const int LeaderboardEntryMargin = 50;
        private const int LeaderboardAvatarSize = 40;
        private const int ToolBtnSize = 40;

        private static readonly Font LeaderboardEntryFont = new Font("Arial", 18);

        private readonly Client _client;
        private readonly Form _startForm;
        private int _timeRemaining = -1;
        private bool _lockServerMessage;
        private readonly AvatarManager _avatarManager;

        private readonly List<IDrawTool> _tools;
        private DirectBitmap _bitmap;
        private readonly List<Point> _drawQueue;
        private readonly List<DrawAction> _elements;
        private DrawAction _currentAction;
        private PictureBox _prevToolBtn;
        private IDrawTool _selectedTool;

        private readonly Timer _roundTimer;

        /// <summary>
        /// Skapar en ny MainGameForm.
        /// </summary>
        /// <param name="startForm">Startformuläret</param>
        /// <param name="client">Nätverksklienten att interagera med.</param>
        /// <param name="avatarManager">AvatarManager för att generera avatarer.</param>
        public MainGameForm(Form startForm, Client client, AvatarManager avatarManager)
        {
            _drawQueue = new List<Point>();
            _elements = new List<DrawAction>();
            _tools = new List<IDrawTool>();
            _client = client;
            _startForm = startForm;
            _avatarManager = avatarManager;

            _roundTimer = new Timer(1000);
            _roundTimer.Elapsed += RoundTimer_Tick;
            _roundTimer.AutoReset = true;

            // Timern används för att inte spamma servern med paket då vi t.ex. ritat något med en pensel.
            // Istället lägger vi till punkterna som musen varit vid i en kö och skickar alla dessa på ett interval.
            Timer drawTimer = new Timer(33); // 1000/30 för 30 fps
            drawTimer.Elapsed += DrawTimer_Tick;
            drawTimer.AutoReset = true;
            drawTimer.Start();
        }

        private void DrawTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_drawQueue.Count < 1) // Vi behöver inte skicka något om inget finns att skicka.
                return;

            _client.SendPacket(new DrawPacket(new DrawAction(_drawQueue, _currentAction.Type, _currentAction.Color,
                _currentAction.Size, _currentAction.Instant), _elements.Count));

            _drawQueue.Clear();
        }

        /// <summary>
        /// Initialiserar formuläret, lägger till ritverktyg och annat.
        /// </summary>
        private void Init()
        {
            InitializeComponent();
            Show();
            InitDrawTools();

            SetOverlayVisible(true);
            DisplayCenteredMessage("Väntar på att spelare ska ansluta...", new Font("Arial", 26));
            _lockServerMessage = true;

            ChatPanel.AutoScroll = false;
            ChatPanel.HorizontalScroll.Enabled = false;
            ChatPanel.AutoScroll = true;

            _bitmap = new DirectBitmap(DrawBox.Width, DrawBox.Height);
            DrawBox.Image = _bitmap.Bitmap;
        }

        /// <summary>
        /// Initialiserar alla ritverktyg och lägger till deras komponent på formuläret.
        /// </summary>
        private void InitDrawTools()
        {
            _tools.Add(new BrushTool());
            _tools.Add(new EraserTool(DrawBox.BackColor));
            _tools.Add(new RectangleTool());
            _tools.Add(new EllipseTool());
            _tools.Add(new BucketTool());
            _tools.Add(new ClearTool());

            for (int i = 0; i < _tools.Count; i++)
            {
                IDrawTool tool = _tools[i];
                PictureBox pictureBox = new PictureBox();

                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.Margin = new Padding
                {
                    Left = i == 0 ? 0 : 10
                }; // Om det är det första verktyget i panelen ger vi det en margin till vänster. 
                pictureBox.Size = new Size(ToolBtnSize, ToolBtnSize);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image =
                    Image.FromFile("resources/tools/" + Enum.GetName(typeof(ToolType), tool.Type) + ".png");
                pictureBox.Cursor = Cursors.Hand;
                pictureBox.Click += (sender, args) =>
                {
                    if (tool.NoInteraction) // Om verktyget inte kräver någon punkt på ritytan, utför direkt när det väljs.
                    {
                        DrawAction action = new DrawAction(tool.Type);
                        Draw(action);
                        _client.SendPacket(new DrawPacket(action, -1));
                        return;
                    }

                    if (_prevToolBtn != null) // Återställ det tidigare verktygets knapp.
                        _prevToolBtn.BorderStyle = BorderStyle.FixedSingle;

                    _selectedTool = tool;
                    _prevToolBtn = pictureBox;
                    pictureBox.BorderStyle = BorderStyle.Fixed3D;
                };

                ToolPanel.Controls.Add(pictureBox);
            }

            ToolPanel.Controls.Add(SizeBar);
            ToolPanel.Controls.Add(ColorBtn);

            _selectedTool = _tools[0];
        }

        /// <see cref="IGameForm.AddChatMessage(Player, string, bool, Color)"/>
        public void AddChatMessage(Player sender, string message, bool bold, Color color)
        {
            if (IsDisposed)
                return;

            bool playerMessage = sender != null;

            string prefix = playerMessage ? $"{sender.Name}: " : "";
            Image avatar = playerMessage ? _avatarManager.GetOrCreateAvatar(sender) : null;

            ChatPanel.Invoke(new Action(() =>
            {
                ChatMessagePanel msgPanel = new ChatMessagePanel(prefix + message,
                    avatar, color, true, ChatPanelWidth);

                ChatPanel.Controls.Add(msgPanel);
                ChatPanel.Width = ChatPanelWidth +
                                  (ChatPanel.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0); // Uppdatera meddelarlistans bredd så att scrollbaren får plats.
                ChatPanel.ScrollControlIntoView(msgPanel);
            }));
        }

        /// <summary>
        /// Hittar ett ritverktyg med typen.
        /// </summary>
        /// <param name="type">Typ att leta efter.</param>
        /// <returns>Funnet verktyg eller null om inget finns.</returns>
        private IDrawTool GetToolByType(ToolType type)
        {
            return _tools.Find(tool => tool.Type == type);
        }

        /// <summary>
        /// Ritar på ritytan med valt verktyg, enligt de uppgifter som finns i rithändelsen.
        /// </summary>
        /// <param name="tool">Verktyg att rita med.</param>
        /// <param name="action">Händelse att utföra.</param>
        /// <param name="graphics">Grafikinstans att rita med.</param>
        private void Draw(IDrawTool tool, DrawAction action, Graphics graphics)
        {
            tool.Draw(graphics, action, _bitmap, _elements);
        }

        /// <summary>
        /// Ritar en händelse på ritytan. Använder Graphics-instansen från ritytans Bitmap.
        /// </summary>
        /// <param name="action">Händelse att läsa data från och rita med.</param>
        private void Draw(DrawAction action)
        {
            using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
            {
                Draw(action, graphics);
            }
        }

        /// <summary>
        /// Ritar en händelse på rityan, med den specificerade Graphics-instansen.
        /// </summary>
        /// <param name="action">Händelse att rita.</param>
        /// <param name="graphics">Graphics-instansen att rita med.</param>
        private void Draw(DrawAction action, Graphics graphics)
        {
            IDrawTool tool = GetToolByType(action.Type);
            if (tool == null)
                return;

            Draw(tool, action, graphics);
        }

        /// <summary>
        /// <see cref="IGameForm.Draw(DrawAction, int)"/>
        /// </summary>
        public void Draw(DrawAction action, int elementId)
        {
            int id = elementId - 1;
            if (id >= 0 && id < _elements.Count) // Om elementet finns i listan och ska uppdateras
            {
                _elements[id].Points.AddRange(action.Points);
                return;
            }

            UpdateBitmap();

            if (action.Instant) // Om händelsen är instant kan vi rita den direkt, den behöver aldrig uppdateras.
            {
                Draw(action);
            }
            else
            {
                _elements.Add(action);
            }
        }
        
        /// <see cref="IGameForm.ShowLeaderboard"/>
        public void ShowLeaderboard()
        {
            SetOverlayVisible(true);
            Player[] sorted = _client.GetLeaderboard().ToArray();
            Graphics g = Graphics.FromImage(new Bitmap(1, 1)); // Vi behöver ett Graphics-objekt för att kunna använda MeasureString

            for (int i = 0; i < sorted.Length; i++)
            {
                Player player = sorted[i];
                int place = i + 1;
                int y = 40 + LeaderboardEntryMargin * i; // Börja topplistan vid y=30, varje resultat har LeaderboardEntryMargin som margin

                string text = $"{place}. {player.Name} - {player.Score}";

                PictureBox image = new PictureBox();
                image.Size = new Size(LeaderboardAvatarSize, LeaderboardAvatarSize);
                image.SizeMode = PictureBoxSizeMode.StretchImage;
                image.Image = _avatarManager.GetOrCreateAvatar(player);

                SizeF textSize = g.MeasureString(text, LeaderboardEntryFont);
                // Den totala bredden för texten och bilden
                float totalWidth = textSize.Width + image.Width;

                // Centrerar avataren i mitten
                image.Location = new Point((int) (OverlayPanel.Width / 2 - totalWidth / 2), y);

                Label scoreLabel = new Label();
                scoreLabel.Text = text;
                scoreLabel.Font = LeaderboardEntryFont;
                scoreLabel.Size = new Size(OverlayPanel.Width, 40);
                scoreLabel.ForeColor = Color.White;
                // Placera texten i mitten av bilden
                scoreLabel.Location = new Point(image.Location.X + image.Size.Width, (int) (y + textSize.Height / 4));

                OverlayPanel.Controls.Add(image);
                OverlayPanel.Controls.Add(scoreLabel);
            }
        }

        /// <summary>
        /// Visar ett meddelande centrerat på overlayen.
        /// </summary>
        /// <param name="message">Meddelande att visa.</param>
        /// <param name="font">Typsnitt att rendera meddelandet i.</param>
        private void DisplayCenteredMessage(string message, Font font)
        {
            Label messageLabel = new Label();
            messageLabel.Size = new Size(OverlayPanel.Width, OverlayPanel.Height); // Gör labeln lika stor som overlayen så att texten enkelt alignas i mitten av overlayen.
            messageLabel.Location = new Point(0, 0);
            
            messageLabel.Text = message;
            messageLabel.Font = font;
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            messageLabel.ForeColor = Color.White;

            OverlayPanel.Controls.Add(messageLabel);
        }

        /// <summary>
        /// Hantera då klienten förlorar anslutning till servern.
        /// </summary>
        /// <param name="reason">Anledning från server eller default meddelande.</param>
        /// <param name="kick"></param>
        public void Disconnect(string reason, bool kick)
        {
            _startForm.Show();
            Hide();

            string prefix = kick ? "Du blev kickad: " : "";
            MessageBox.Show($"{prefix}{reason}");
        }

        /// <see cref="IGameForm.UpdatePlayerList(List{Player})"/>
        public void UpdatePlayerList(List<Player> players)
        {
            IOrderedEnumerable<Player> sorted = players.OrderByDescending(p => p.Score);
            ExecuteOnFormThread(() =>
            {
                PlayerListPanel.Controls.Clear();

                int i = 0;
                foreach (Player player in sorted)
                {
                    PlayerListPanel.Controls.Add(CreatePlayerAvatarBox(player, i == 0));
                    i++;
                }
            });
        }

        /// <summary>
        /// Konstruerar en PictureBox med spelarens avatar som bild.
        /// </summary>
        /// <param name="player">Spelare att konstruera för.</param>
        /// <param name="first">Om det är den första avataren och därför inte kräver margin.</param>
        /// <returns></returns>
        private PictureBox CreatePlayerAvatarBox(Player player, bool first)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Margin = new Padding {Left = first ? 0 : 10}; // Alla avatarer förutom den första ska få en margin.
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Size = new Size(40, 40);

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = _avatarManager.GetOrCreateAvatar(player);

            return pictureBox;
        }

        /// <see cref="IGameForm.UpdateGameFormState"/>
        public void UpdateGameFormState(int round, int timeRemaining, bool showTimer)
        {
            if (_timeRemaining == -1)
                SetOverlayVisible(false);

            _timeRemaining = timeRemaining;

            ExecuteOnFormThread(() =>
            {
                TimeLabel.Visible = showTimer;
                RoundLabel.Text = $"Runda {round}/3";
            });
        }

        /// <see cref="IGameForm.UpdateWord(string)"/>
        public void UpdateWord(string word)
        {
            ExecuteOnFormThread(() => { WordLabel.Text = word; });
        }

        /// <summary>
        /// Exekverar kod på formulärets tråd vilket är ett krav då vi hanterar komponenter.
        /// </summary>
        /// <param name="action"></param>
        private void ExecuteOnFormThread(Action action)
        {
            if (IsDisposed)
                return;

            Invoke(action);
        }

        /// <see cref="IGameForm.SetOverlayVisible(bool)" />
        public void SetOverlayVisible(bool visible)
        {
            if (!visible)
                _lockServerMessage = false;

            ExecuteOnFormThread(() =>
            {
                OverlayPanel.Controls.Clear();
                OverlayPanel.Visible = visible;
            });
        }

        /// <see cref="IGameForm.OnConnect" />
        public void OnConnect()
        {
            Init();
            _roundTimer.Start();
        }

        /// <see cref="IGameForm.ShowDrawerNotice(string)" />
        public void ShowDrawerNotice(string word)
        {
            _lockServerMessage = true; // Detta meddelandet ska inte försvinna om spelaren försöker visa leaderboarden med F1.
            SetOverlayVisible(true);

            Label wordLbl = new Label();
            wordLbl.Text = word;
            wordLbl.Font = new Font("Arial", 20, FontStyle.Bold);
            wordLbl.ForeColor = Color.White;
            wordLbl.Size = new Size(OverlayPanel.Width, 30);
            wordLbl.TextAlign = ContentAlignment.MiddleCenter;
            wordLbl.Location = new Point(0, OverlayPanel.Height / 4);
            OverlayPanel.Controls.Add(wordLbl);

            DisplayCenteredMessage($"Du ska rita ordet ovan. \r\nKlicka här på ritytan för att börja.",
                new Font("Arial", 10));

            EventHandler callback = (sender, args) => { SetOverlayVisible(false); };

            foreach (Control c in OverlayPanel.Controls)
            {
                // Lägger till click event på alla kontroller, när overlayen visas på nytt rensas dessa kontroller och vi behöver därför inte avregistrera event handlern.
                // Detta krävs då vi inte kan lägga till click event endast på overlayen eftersom vissa meddelanden täcker hela overlayen.
                c.Click += callback;
            }
        }

        /// <summary>
        /// Uppdaterar den interna Bitmapen för ritningen.
        /// </summary>
        private void UpdateBitmap()
        {
            DrawBox.DrawToBitmap(_bitmap.Bitmap,
                new Rectangle(0, 0, DrawBox.Width, DrawBox.Height)); 
        }
        
        /// <see cref="IGameForm.SetDrawToolsVisible"/>
        public void SetDrawToolsVisible(bool visible)
        {
            ExecuteOnFormThread(() => { ToolPanel.Visible = visible; });
        }

        /// <see cref="IGameForm.ClearCanvas"/>
        public void ClearCanvas()
        {
            Draw(new DrawAction(ToolType.Clear));
        }

        /// <see cref="IGameForm.ShowLeaderboard"/>
        public void ShowPostGameScreen()
        {
            _lockServerMessage = true;
            SetOverlayVisible(true);

            Label titleLabel = new Label();
            titleLabel.Text = "Spelet är slut!";
            titleLabel.Font = new Font("Arial", 20);
            titleLabel.Location = new Point(0, 40);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Size = new Size(OverlayPanel.Width, 30);
            titleLabel.ForeColor = Color.White;

            Player[] sorted = _client.GetLeaderboard().ToArray();
            for (int i = 0; i < sorted.Length && i < 3; i++)
            {
                int panelWidth = 250;
                int panelX = OverlayPanel.Width / 2 - panelWidth / 2; // Utgå från mitten av overlayen

                Color textColor;
                string prefix;
                int panelY;
                switch (i)
                {
                    case 0:
                    {
                        prefix = "Första";
                        panelY = 100;
                        textColor = Color.Gold;

                        break;
                    }
                    case 1:
                    {
                        prefix = "Andra";
                        panelY = 200;
                        panelX -= 150;
                        textColor = Color.Silver;
                        break;
                    }
                    default:
                    {
                        prefix = "Tredje";
                        panelY = 200;
                        panelX += 150;
                        textColor = ColorTranslator.FromHtml("#CD7F32"); // Brons
                        break;
                    }
                }

                Player player = sorted[i];

                Panel panel = new Panel();
                panel.Size = new Size(panelWidth, 100);
                panel.Location = new Point(panelX, panelY);

                PictureBox avatarBox = new PictureBox();
                avatarBox.Size = new Size(40, 40);
                avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;
                avatarBox.Image = _avatarManager.GetOrCreateAvatar(player);
                avatarBox.Location = new Point(panel.Width / 2 - avatarBox.Width / 2);

                Label placeLabel = new Label();
                placeLabel.Text = $"{prefix} plats";
                placeLabel.Font = new Font("Arial", 12, FontStyle.Bold);
                placeLabel.Location = new Point(0, 50);
                placeLabel.ForeColor = textColor;
                placeLabel.Size = new Size(panel.Width, 20);
                placeLabel.TextAlign = ContentAlignment.MiddleCenter;
                
                Label scoreLabel = new Label();
                scoreLabel.Text = $"{player.Name} - {player.Score} poäng";
                scoreLabel.Location = new Point(0, 80);
                scoreLabel.Font = new Font("Arial", 10);
                scoreLabel.ForeColor = Color.White;
                scoreLabel.Size = new Size(panel.Width, 20);
                scoreLabel.TextAlign = ContentAlignment.MiddleCenter;

                panel.Controls.Add(avatarBox);
                panel.Controls.Add(placeLabel);
                panel.Controls.Add(scoreLabel);
                OverlayPanel.Controls.Add(panel);
            }
            
            OverlayPanel.Controls.Add(titleLabel);
        }

        /* Event Handlers */

        private void MainGameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && !_lockServerMessage && OverlayPanel.Visible) // Göm endast om inte overlayen är låst och panelen redan visas.
            {
                SetOverlayVisible(false);
            }
        }

        private void MainGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && !_lockServerMessage && !OverlayPanel.Visible) // Visa endast om inte overlayen är låst och panelen inte visas.
            {
                ShowLeaderboard();
            }
        }

        private void MainGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _startForm.Show();
            _client.Disconnect();
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_client.IsDrawing())
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    List<Point> points = new List<Point>();
                    if (_selectedTool.Instant) // Om den är instant kräver den endast en punkt på ritytan.
                    {
                        points.Add(e.Location);
                        DrawAction instant = new DrawAction(points, _selectedTool.Type,
                            ColorBtn.BackColor, SizeBar.Value, true);

                        _client.SendPacket(new DrawPacket(instant, -1));
                        Draw(instant);
                        return;
                    }

                    points.Add(e.Location);
                    DrawAction action = new DrawAction(points, _selectedTool.Type, 
                        ColorBtn.BackColor, SizeBar.Value, false); // Instant är false sedan ovan

                    if (_selectedTool is SubTool subTool)
                    {
                        action = subTool.ModifyAction(action); // En subtool vidarebefordrar ritningen med andra inställningar
                    }
                
                    if (!(_selectedTool is SelectionDrawTool)) // SelectionDrawTools kräver ingen drawQueue
                    {
                        _drawQueue.AddRange(points);
                    }

                    _elements.Add(action);
                    _currentAction = action;
                    break;
                }
                
                case MouseButtons.Right:
                {
                    if (_currentAction != null && _selectedTool is SelectionDrawTool)
                    {
                        _elements.Remove(_currentAction);
                        _currentAction = null;
                    }

                    break;
                }
            }
        }

        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _currentAction == null)
                return;

            if (_selectedTool is SelectionDrawTool)
            {
                // Markering är klar på klienten och kan skickas till servern.
                _client.SendPacket(new DrawPacket(_currentAction, _elements.Count));
            }
            else
            {
                DrawTimer_Tick(this, null); // Skicka paketen direkt nu när vi släpper musen 
            }

            _currentAction = null;
            UpdateBitmap();
        }
        
        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentAction == null)
                return;

            Point point = e.Location;
            if (point.X < 0 || point.X > DrawBox.Width || point.Y > DrawBox.Height || point.Y < 0) // Se till att användaren inte går utanför ritytan.
            {
                return;
            }

            bool selectionTool = _selectedTool is SelectionDrawTool;
            if (selectionTool && _currentAction.Points.Count == 2) 
            {
                // Uppdatera den andra punkten när vi rör musen.
                _currentAction.Points[_currentAction.Points.Count - 1] = point;
            }
            else
            {
                _currentAction.Points.Add(point);

                if (!selectionTool)
                {
                    _drawQueue.Add(point);
                }
            }
        }

        private void ColorBtn_Click(object sender, EventArgs e)
        {
            CustomColorDialog dialog = new CustomColorDialog();
            dialog.ColorChanged += ColorDialog_ColorChanged;
            dialog.Show();
            dialog.SetDesktopLocation(MousePosition.X, MousePosition.Y);
        }

        private void ColorDialog_ColorChanged(object sender, Color color)
        {
            ColorBtn.BackColor = color;
            (sender as Form)?.Dispose(); // Stäng dialogen när färgen valts
        }

        private void Chat_Submit(object sender, EventArgs e)
        {
            _client.SendPacket(new ChatPacket(ChatBox.Text));
            ChatBox.Text = "";
        }

        private void RoundTimer_Tick(object sender, EventArgs e)
        {
            ExecuteOnFormThread(() =>
            {
                if (_timeRemaining <= 0)
                    return;

                _timeRemaining--;
                TimeSpan time = TimeSpan.FromSeconds(_timeRemaining);
                TimeLabel.Text = time.ToString(@"mm\:ss");
            });
        }

        private void DrawBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (DrawAction action in _elements)
            {
                if (action.Points.Count >= 2) // Alla händelser som sparas i _elements kräver minst två punkter.
                {
                    Draw(action, e.Graphics);
                }
            }

            DrawBox.Invalidate();
            System.Threading.Thread.Sleep(1); // 1 ms leder till mycket bättre prestanda eftersom Paint kan vara ganska dyrt.
        }
        
    }
}