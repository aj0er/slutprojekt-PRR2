using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// En egen kontroll för panelen som håller i alla chattmeddelanden.
    /// </summary>
    public class ChatMessagePanel : Panel
    {
        
        private const int ImageSize = 40;
        private const int FontSize = 9;
        private const int PanelPadding = 10;
        private const int PanelRightMargin = 5;
        private const int ServerMessagePadding = 10;

        /// <summary>
        /// Skapar en ny ChatMessagePanel och dess sub-komponenter.
        /// </summary>
        /// <param name="message">Meddelandet som ska visas.</param>
        /// <param name="avatarImage">Optional avatar-bild om meddelandet skickades av en spelare.</param>
        /// <param name="color">Färg att visa meddelandet i.</param>
        /// <param name="bold">Om meddelandet ska vara fetstilt.</param>
        /// <param name="parentWidth">Bredden på behållaren som ska hålla i denna komponent.</param>
        public ChatMessagePanel(string message, Image avatarImage, Color color, bool bold, int parentWidth)
        {
            BorderStyle = BorderStyle.FixedSingle;
            Margin = new Padding { All = 0 };
            Size = new Size(parentWidth, 80);

            bool serverMessage = avatarImage != null; // Om ingen bild finns är det ingen spelare som skickat meddelandet.
            
            Panel labelPanel = new Panel(); // Behållare som håller i labeln (behövs för text wrap)
            labelPanel.Location = new Point(serverMessage ? (ImageSize + PanelPadding * 2) : ServerMessagePadding, 0); // Gör plats för bilden om det finns en sådan.
            labelPanel.Height = serverMessage ? Size.Height - PanelPadding : Size.Height;
            labelPanel.Width = Size.Width - labelPanel.Location.X - PanelRightMargin;

            Label msgLabel = new Label(); // Meddelandets text
            msgLabel.Font = new Font("Arial", FontSize, bold ? FontStyle.Bold : FontStyle.Regular);
            msgLabel.Text = message;
            msgLabel.ForeColor = color;
            msgLabel.MaximumSize = new Size(labelPanel.Width, Size.Height);
            msgLabel.Dock = DockStyle.Fill;
            msgLabel.TextAlign = ContentAlignment.MiddleLeft;

            labelPanel.Controls.Add(msgLabel);

            PictureBox avatarBox = new PictureBox(); // Visar spelarens avatar-bild
            avatarBox.Location = new Point(PanelPadding, Size.Height / 2 - ImageSize / 2); // Centrera bilden vertikalt.
            avatarBox.Size = new Size(ImageSize, ImageSize);
            avatarBox.Image = avatarImage;
            avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;

            Controls.Add(labelPanel);
            Controls.Add(avatarBox);
        }
    }
}