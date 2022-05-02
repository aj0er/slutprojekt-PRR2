using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// En egen skräddarsydd ColorDialog för våra ändamål, har ingen titlebar och endast valda färger.
    /// </summary>
    public partial class CustomColorDialog : Form
    {
        
        private static readonly Color[] Colors = new Color[] {Color.Red, Color.DarkRed, Color.Orange, Color.Yellow, Color.Green, Color.Lime, 
            Color.Blue, Color.DodgerBlue, Color.Aqua, Color.Pink, Color.Purple, Color.Magenta, Color.SaddleBrown, 
            Color.Gray, Color.Black, Color.White, };

        private const int BtnSize = 30;
        private const int BtnGap = 5;
        private const int BtnPadding = 10;
        
        public event EventHandler<Color> ColorChanged;
        
        public CustomColorDialog()
        {
            InitializeComponent();
            int x = BtnPadding;
            int y = BtnPadding;
            foreach (Color color in Colors)
            {
                
                Panel panel = new Panel();
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Size = new Size(BtnSize, BtnSize);
                panel.Location = new Point(x, y);
                panel.BackColor = color;
                panel.Click += (sender, args) =>
                {
                    if (ColorChanged != null)
                        ColorChanged.Invoke(this, color);
                };

                Controls.Add(panel);
                x += (BtnSize + BtnGap);
                if (x + BtnSize > Width) // Om knappen går utanför fönstret, gå till nästa rad.
                {
                    x = BtnPadding;
                    y += BtnSize + BtnPadding;
                }
            }
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            Close();
        }
        
    }
}