namespace Client
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.HatNextBtn = new System.Windows.Forms.Button();
            this.AvatarBox = new System.Windows.Forms.PictureBox();
            this.EyesNextBtn = new System.Windows.Forms.Button();
            this.EyesPrevBtn = new System.Windows.Forms.Button();
            this.HatPrevBtn = new System.Windows.Forms.Button();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.NameLbl = new System.Windows.Forms.Label();
            this.HostNameBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.NumericUpDown();
            this.ServerLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).BeginInit();
            this.SuspendLayout();
            // 
            // HatNextBtn
            // 
            this.HatNextBtn.Location = new System.Drawing.Point(454, 76);
            this.HatNextBtn.Name = "HatNextBtn";
            this.HatNextBtn.Size = new System.Drawing.Size(75, 23);
            this.HatNextBtn.TabIndex = 0;
            this.HatNextBtn.Text = ">";
            this.HatNextBtn.UseVisualStyleBackColor = true;
            this.HatNextBtn.Click += new System.EventHandler(this.HatNextBtn_Click);
            // 
            // AvatarBox
            // 
            this.AvatarBox.Location = new System.Drawing.Point(348, 76);
            this.AvatarBox.Name = "AvatarBox";
            this.AvatarBox.Size = new System.Drawing.Size(100, 100);
            this.AvatarBox.TabIndex = 1;
            this.AvatarBox.TabStop = false;
            // 
            // EyesNextBtn
            // 
            this.EyesNextBtn.Location = new System.Drawing.Point(454, 114);
            this.EyesNextBtn.Name = "EyesNextBtn";
            this.EyesNextBtn.Size = new System.Drawing.Size(75, 23);
            this.EyesNextBtn.TabIndex = 2;
            this.EyesNextBtn.Text = ">";
            this.EyesNextBtn.UseVisualStyleBackColor = true;
            this.EyesNextBtn.Click += new System.EventHandler(this.EyesNextBtn_Click);
            // 
            // EyesPrevBtn
            // 
            this.EyesPrevBtn.Location = new System.Drawing.Point(267, 114);
            this.EyesPrevBtn.Name = "EyesPrevBtn";
            this.EyesPrevBtn.Size = new System.Drawing.Size(75, 23);
            this.EyesPrevBtn.TabIndex = 5;
            this.EyesPrevBtn.Text = "<";
            this.EyesPrevBtn.UseVisualStyleBackColor = true;
            this.EyesPrevBtn.Click += new System.EventHandler(this.EyesPrevBtn_Click);
            // 
            // HatPrevBtn
            // 
            this.HatPrevBtn.Location = new System.Drawing.Point(267, 76);
            this.HatPrevBtn.Name = "HatPrevBtn";
            this.HatPrevBtn.Size = new System.Drawing.Size(75, 23);
            this.HatPrevBtn.TabIndex = 4;
            this.HatPrevBtn.Text = "<";
            this.HatPrevBtn.UseVisualStyleBackColor = true;
            this.HatPrevBtn.Click += new System.EventHandler(this.HatPrevBtn_Click);
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectBtn.Location = new System.Drawing.Point(292, 300);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(212, 39);
            this.ConnectBtn.TabIndex = 6;
            this.ConnectBtn.Text = "Anslut";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(333, 217);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(130, 20);
            this.NameBox.TabIndex = 7;
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(330, 201);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(35, 13);
            this.NameLbl.TabIndex = 8;
            this.NameLbl.Text = "Namn";
            // 
            // HostNameBox
            // 
            this.HostNameBox.Location = new System.Drawing.Point(292, 265);
            this.HostNameBox.Name = "HostNameBox";
            this.HostNameBox.Size = new System.Drawing.Size(156, 20);
            this.HostNameBox.TabIndex = 9;
            this.HostNameBox.Text = "127.0.0.1";
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(454, 266);
            this.PortBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.PortBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(50, 20);
            this.PortBox.TabIndex = 10;
            this.PortBox.Value = new decimal(new int[] {
            17532,
            0,
            0,
            0});
            // 
            // ServerLbl
            // 
            this.ServerLbl.AutoSize = true;
            this.ServerLbl.Location = new System.Drawing.Point(289, 249);
            this.ServerLbl.Name = "ServerLbl";
            this.ServerLbl.Size = new System.Drawing.Size(38, 13);
            this.ServerLbl.TabIndex = 11;
            this.ServerLbl.Text = "Server";
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ServerLbl);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.HostNameBox);
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.EyesPrevBtn);
            this.Controls.Add(this.HatPrevBtn);
            this.Controls.Add(this.EyesNextBtn);
            this.Controls.Add(this.AvatarBox);
            this.Controls.Add(this.HatNextBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StartForm";
            this.Text = "Guess N\' Draw";
            this.Load += new System.EventHandler(this.StartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AvatarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button HatNextBtn;

        #endregion

        private System.Windows.Forms.PictureBox AvatarBox;
        private System.Windows.Forms.Button EyesNextBtn;
        private System.Windows.Forms.Button EyesPrevBtn;
        private System.Windows.Forms.Button HatPrevBtn;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label NameLbl;
        private System.Windows.Forms.TextBox HostNameBox;
        private System.Windows.Forms.NumericUpDown PortBox;
        private System.Windows.Forms.Label ServerLbl;
    }
}

