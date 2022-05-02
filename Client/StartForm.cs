using API;
using Client.Avatars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Start-formuläret som tillåter spelaren att välja avatar, server och ansluta till servern.
    /// </summary>
    public partial class StartForm : Form, IStartForm
    {
        
        private readonly AvatarManager _avatarManager;
        private readonly Dictionary<AvatarPartType, int> _boundaries;
        private readonly Avatar _avatar;
        private readonly Client _client;

        private const int MinNameLength = 2;
        private const int MaxNameLength = 16;

        public StartForm()
        {
            _avatarManager = new AvatarManager();
            _boundaries = _avatarManager.GetPartBoundaries();
            _avatar = new Avatar(0, 0, Color.Red);
            _client = new Client(this);

            InitializeComponent();
        }
        
        /// <see cref="IStartForm.HandleConnect"/>
        public void HandleConnect()
        {
            Hide();
        }
        
        /// <see cref="IStartForm.HandleFailedConnect"/>
        public void HandleFailedConnect()
        {
            MessageBox.Show("Kunde inte ansluta till servern. Fungerar din nätverksanslutning?");
        }
        
        /// <summary>
        /// Genererar om avataren i formuläret.
        /// </summary>
        private void UpdateAvatar()
        {
            Image img = _avatarManager.GenerateAvatar(_avatar);
            if (img == null)
                return;

            AvatarBox.Image = img;
        }
        
        /// <summary>
        /// Kollar om avatar-delen som spelaren navigerar till är giltig.
        /// </summary>
        /// <param name="type">Delens typ.</param>
        /// <param name="val">Delens unika ID.</param>
        /// <returns>Om delen finns att välja.</returns>
        private bool IsPartValid(AvatarPartType type, int val)
        {
            return val >= 0 && val < _boundaries[type];
        }

        /* Event Handlers */
        private void StartForm_Load(object sender, EventArgs e)
        {
            UpdateAvatar();
        }

        private void HatNextBtn_Click(object s, EventArgs e)
        {
            int change = _avatar.Hat + 1;
            _avatar.Hat = IsPartValid(AvatarPartType.Hat, change) ? change : 0; // Hoppa till början om vi nått slutet av listan. 
            UpdateAvatar();
        }

        private void EyesNextBtn_Click(object sender, EventArgs e)
        {
            int change = _avatar.Eyes + 1;
            _avatar.Eyes = IsPartValid(AvatarPartType.Eyes, change) ? change : 0; // Hoppa till början om vi nått slutet av listan. 
            
            UpdateAvatar();
        }

        private void EyesPrevBtn_Click(object sender, EventArgs e)
        {
            if (_avatar.Eyes == 0)
            {
                _avatar.Eyes = _boundaries[AvatarPartType.Eyes] - 1; // Hoppa till slutet av "karusellen".
            }
            else
            {
                _avatar.Eyes--;
            }

            UpdateAvatar();
        }

        private void HatPrevBtn_Click(object sender, EventArgs e)
        {
            if (_avatar.Hat == 0)
            {
                _avatar.Hat = _boundaries[AvatarPartType.Hat] - 1; // Hoppa till slutet av "karusellen".
            } else
            {
                _avatar.Hat--;
            }

            UpdateAvatar();
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            string name = NameBox.Text.Trim();
            if(name.Length < MinNameLength)
            {
                MessageBox.Show("Ditt namn är för kort!");
                return;
            }

            if(name.Length > MaxNameLength)
            {
                MessageBox.Show("Ditt namn är för långt!");
                return;
            }

            // Tidigare instanser av MainGameForm disposas när fönstret stängs, därför kan vi skapa en ny instans varje gång utan problem.
            MainGameForm gameForm = new MainGameForm(this, _client, _avatarManager);
            _client.Connect(HostNameBox.Text, (int)PortBox.Value, gameForm, _avatar, name);
        }

    }
}