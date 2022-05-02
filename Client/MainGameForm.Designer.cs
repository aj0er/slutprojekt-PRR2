namespace Client
{
    partial class MainGameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGameForm));
            this.DrawBox = new System.Windows.Forms.PictureBox();
            this.ChatPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.RoundLabel = new System.Windows.Forms.Label();
            this.PlayerListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SizeBar = new System.Windows.Forms.TrackBar();
            this.ColorBtn = new System.Windows.Forms.Button();
            this.WordLabel = new System.Windows.Forms.Label();
            this.AvatarBox = new System.Windows.Forms.PictureBox();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.ToolPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.OverlayPanel = new System.Windows.Forms.Panel();
            this.OverlayMsgLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarBox)).BeginInit();
            this.OverlayPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DrawBox
            // 
            this.DrawBox.BackColor = System.Drawing.Color.White;
            this.DrawBox.Location = new System.Drawing.Point(12, 71);
            this.DrawBox.Name = "DrawBox";
            this.DrawBox.Size = new System.Drawing.Size(761, 470);
            this.DrawBox.TabIndex = 0;
            this.DrawBox.TabStop = false;
            this.DrawBox.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawBox_Paint);
            this.DrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseDown);
            this.DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
            this.DrawBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseUp);
            // 
            // ChatPanel
            // 
            this.ChatPanel.AutoScroll = true;
            this.ChatPanel.BackColor = System.Drawing.Color.White;
            this.ChatPanel.Location = new System.Drawing.Point(788, 71);
            this.ChatPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ChatPanel.Name = "ChatPanel";
            this.ChatPanel.Size = new System.Drawing.Size(248, 470);
            this.ChatPanel.TabIndex = 1;
            // 
            // ChatBox
            // 
            this.ChatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatBox.Location = new System.Drawing.Point(788, 555);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(248, 32);
            this.ChatBox.TabIndex = 0;
            // 
            // RoundLabel
            // 
            this.RoundLabel.AutoSize = true;
            this.RoundLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoundLabel.Location = new System.Drawing.Point(662, 26);
            this.RoundLabel.Name = "RoundLabel";
            this.RoundLabel.Size = new System.Drawing.Size(0, 25);
            this.RoundLabel.TabIndex = 4;
            // 
            // PlayerListPanel
            // 
            this.PlayerListPanel.Location = new System.Drawing.Point(12, 12);
            this.PlayerListPanel.Name = "PlayerListPanel";
            this.PlayerListPanel.Size = new System.Drawing.Size(400, 40);
            this.PlayerListPanel.TabIndex = 6;
            // 
            // SizeBar
            // 
            this.SizeBar.Location = new System.Drawing.Point(0, 0);
            this.SizeBar.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.SizeBar.Minimum = 1;
            this.SizeBar.Name = "SizeBar";
            this.SizeBar.Size = new System.Drawing.Size(104, 45);
            this.SizeBar.TabIndex = 7;
            this.SizeBar.Value = 1;
            // 
            // ColorBtn
            // 
            this.ColorBtn.BackColor = System.Drawing.Color.Black;
            this.ColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColorBtn.Location = new System.Drawing.Point(0, 0);
            this.ColorBtn.Name = "ColorBtn";
            this.ColorBtn.Size = new System.Drawing.Size(40, 40);
            this.ColorBtn.TabIndex = 8;
            this.ColorBtn.UseVisualStyleBackColor = false;
            this.ColorBtn.Click += new System.EventHandler(this.ColorBtn_Click);
            // 
            // WordLabel
            // 
            this.WordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordLabel.Location = new System.Drawing.Point(785, 26);
            this.WordLabel.Name = "WordLabel";
            this.WordLabel.Size = new System.Drawing.Size(251, 31);
            this.WordLabel.TabIndex = 9;
            this.WordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AvatarBox
            // 
            this.AvatarBox.Location = new System.Drawing.Point(1001, 22);
            this.AvatarBox.Name = "AvatarBox";
            this.AvatarBox.Size = new System.Drawing.Size(35, 35);
            this.AvatarBox.TabIndex = 10;
            this.AvatarBox.TabStop = false;
            // 
            // ToolPanel
            // 
            this.ToolPanel.Location = new System.Drawing.Point(12, 547);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.Size = new System.Drawing.Size(761, 45);
            this.ToolPanel.TabIndex = 7;
            this.ToolPanel.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1026, 558);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1, 1);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Chat_Submit);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLabel.Location = new System.Drawing.Point(470, 22);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(0, 29);
            this.TimeLabel.TabIndex = 13;
            this.TimeLabel.Visible = false;
            // 
            // OverlayPanel
            // 
            this.OverlayPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.OverlayPanel.Controls.Add(this.OverlayMsgLbl);
            this.OverlayPanel.Location = new System.Drawing.Point(12, 71);
            this.OverlayPanel.Name = "OverlayPanel";
            this.OverlayPanel.Size = new System.Drawing.Size(761, 470);
            this.OverlayPanel.TabIndex = 14;
            // 
            // OverlayMsgLbl
            // 
            this.OverlayMsgLbl.AutoSize = true;
            this.OverlayMsgLbl.ForeColor = System.Drawing.Color.White;
            this.OverlayMsgLbl.Location = new System.Drawing.Point(355, 95);
            this.OverlayMsgLbl.Name = "OverlayMsgLbl";
            this.OverlayMsgLbl.Size = new System.Drawing.Size(0, 13);
            this.OverlayMsgLbl.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "Visa topplistan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "F1";
            // 
            // MainGameForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 600);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ToolPanel);
            this.Controls.Add(this.OverlayPanel);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AvatarBox);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.PlayerListPanel);
            this.Controls.Add(this.RoundLabel);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.ChatPanel);
            this.Controls.Add(this.DrawBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainGameForm";
            this.Text = "Guess N\' Draw";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainGameForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainGameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainGameForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarBox)).EndInit();
            this.OverlayPanel.ResumeLayout(false);
            this.OverlayPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.FlowLayoutPanel ToolPanel;

        #endregion

        private System.Windows.Forms.PictureBox DrawBox;
        private System.Windows.Forms.FlowLayoutPanel ChatPanel;
        private System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.Label RoundLabel;
        private System.Windows.Forms.FlowLayoutPanel PlayerListPanel;
        private System.Windows.Forms.TrackBar SizeBar;
        private System.Windows.Forms.Button ColorBtn;
        private System.Windows.Forms.Label WordLabel;
        private System.Windows.Forms.PictureBox AvatarBox;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Panel OverlayPanel;
        private System.Windows.Forms.Label OverlayMsgLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}