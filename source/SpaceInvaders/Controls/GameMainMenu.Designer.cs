namespace SpaceInvaders.Controls
{
    partial class GameMainMenu
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameMainMenu));
			this.QuitButton = new System.Windows.Forms.Button();
			this.OptionsButton = new System.Windows.Forms.Button();
			this.HighscoreButton = new System.Windows.Forms.Button();
			this.backgroundPictureBox = new System.Windows.Forms.PictureBox();
			this.CreditsButton = new System.Windows.Forms.Button();
			this.NewGameButton = new System.Windows.Forms.Button();
			this.gapPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.backgroundPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// QuitButton
			// 
			this.QuitButton.FlatAppearance.BorderSize = 0;
			this.QuitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.QuitButton.Image = global::SpaceInvaders.Properties.Resources.exit;
			this.QuitButton.Location = new System.Drawing.Point(880, 703);
			this.QuitButton.Margin = new System.Windows.Forms.Padding(0);
			this.QuitButton.Name = "QuitButton";
			this.QuitButton.Size = new System.Drawing.Size(144, 65);
			this.QuitButton.TabIndex = 4;
			this.QuitButton.UseVisualStyleBackColor = true;
			this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
			this.QuitButton.MouseEnter += new System.EventHandler(this.QuitButton_MouseEnter);
			this.QuitButton.MouseLeave += new System.EventHandler(this.QuitButton_MouseLeave);
			// 
			// OptionsButton
			// 
			this.OptionsButton.Location = new System.Drawing.Point(0, 0);
			this.OptionsButton.Name = "OptionsButton";
			this.OptionsButton.Size = new System.Drawing.Size(75, 23);
			this.OptionsButton.TabIndex = 12;
			// 
			// HighscoreButton
			// 
			this.HighscoreButton.FlatAppearance.BorderSize = 0;
			this.HighscoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.HighscoreButton.Image = global::SpaceInvaders.Properties.Resources.highscore;
			this.HighscoreButton.Location = new System.Drawing.Point(280, 703);
			this.HighscoreButton.Margin = new System.Windows.Forms.Padding(0);
			this.HighscoreButton.Name = "HighscoreButton";
			this.HighscoreButton.Size = new System.Drawing.Size(280, 65);
			this.HighscoreButton.TabIndex = 2;
			this.HighscoreButton.UseVisualStyleBackColor = true;
			this.HighscoreButton.Click += new System.EventHandler(this.HighscoreButton_Click);
			this.HighscoreButton.MouseEnter += new System.EventHandler(this.HighscoreButton_MouseEnter);
			this.HighscoreButton.MouseLeave += new System.EventHandler(this.HighscoreButton_MouseLeave);
			// 
			// backgroundPictureBox
			// 
			this.backgroundPictureBox.BackColor = System.Drawing.SystemColors.Desktop;
			this.backgroundPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("backgroundPictureBox.Image")));
			this.backgroundPictureBox.Location = new System.Drawing.Point(0, 0);
			this.backgroundPictureBox.Margin = new System.Windows.Forms.Padding(0);
			this.backgroundPictureBox.Name = "backgroundPictureBox";
			this.backgroundPictureBox.Size = new System.Drawing.Size(1024, 703);
			this.backgroundPictureBox.TabIndex = 10;
			this.backgroundPictureBox.TabStop = false;
			// 
			// CreditsButton
			// 
			this.CreditsButton.FlatAppearance.BorderSize = 0;
			this.CreditsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CreditsButton.Font = new System.Drawing.Font("Calibri Light", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CreditsButton.Image = global::SpaceInvaders.Properties.Resources.credits;
			this.CreditsButton.Location = new System.Drawing.Point(560, 703);
			this.CreditsButton.Name = "CreditsButton";
			this.CreditsButton.Size = new System.Drawing.Size(280, 65);
			this.CreditsButton.TabIndex = 3;
			this.CreditsButton.UseVisualStyleBackColor = true;
			this.CreditsButton.Click += new System.EventHandler(this.CreditsButton_Click);
			this.CreditsButton.MouseEnter += new System.EventHandler(this.CreditsButton_MouseEnter);
			this.CreditsButton.MouseLeave += new System.EventHandler(this.CreditsButton_MouseLeave);
			// 
			// NewGameButton
			// 
			this.NewGameButton.FlatAppearance.BorderSize = 0;
			this.NewGameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.NewGameButton.Image = global::SpaceInvaders.Properties.Resources.newgame;
			this.NewGameButton.Location = new System.Drawing.Point(0, 703);
			this.NewGameButton.Margin = new System.Windows.Forms.Padding(0);
			this.NewGameButton.Name = "NewGameButton";
			this.NewGameButton.Size = new System.Drawing.Size(280, 65);
			this.NewGameButton.TabIndex = 1;
			this.NewGameButton.UseVisualStyleBackColor = true;
			this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
			this.NewGameButton.MouseEnter += new System.EventHandler(this.NewGameButton_MouseEnter);
			this.NewGameButton.MouseLeave += new System.EventHandler(this.NewGameButton_MouseLeave);
			// 
			// gapPanel
			// 
			this.gapPanel.BackgroundImage = global::SpaceInvaders.Properties.Resources.gap;
			this.gapPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.gapPanel.Location = new System.Drawing.Point(840, 703);
			this.gapPanel.Name = "gapPanel";
			this.gapPanel.Size = new System.Drawing.Size(41, 65);
			this.gapPanel.TabIndex = 13;
			// 
			// GameMainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.Controls.Add(this.gapPanel);
			this.Controls.Add(this.NewGameButton);
			this.Controls.Add(this.CreditsButton);
			this.Controls.Add(this.backgroundPictureBox);
			this.Controls.Add(this.QuitButton);
			this.Controls.Add(this.OptionsButton);
			this.Controls.Add(this.HighscoreButton);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "GameMainMenu";
			this.Size = new System.Drawing.Size(1024, 768);
			((System.ComponentModel.ISupportInitialize)(this.backgroundPictureBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		public System.Windows.Forms.Button OptionsButton;
        public System.Windows.Forms.Button HighscoreButton;
		public System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.PictureBox backgroundPictureBox;
		private System.Windows.Forms.Button CreditsButton;
		public System.Windows.Forms.Button NewGameButton;
		private System.Windows.Forms.Panel gapPanel;
    }
}
