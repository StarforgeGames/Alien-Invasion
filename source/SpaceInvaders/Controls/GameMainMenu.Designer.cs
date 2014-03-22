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
			this.NewGameButton = new System.Windows.Forms.Button();
			this.backgroundPictureBox = new System.Windows.Forms.PictureBox();
			this.CreditsButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.backgroundPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// QuitButton
			// 
			this.QuitButton.Image = global::SpaceInvaders.Properties.Resources.exit;
			this.QuitButton.Location = new System.Drawing.Point(880, 703);
			this.QuitButton.Margin = new System.Windows.Forms.Padding(0);
			this.QuitButton.Name = "QuitButton";
			this.QuitButton.Size = new System.Drawing.Size(144, 65);
			this.QuitButton.TabIndex = 8;
			this.QuitButton.UseVisualStyleBackColor = true;
			this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
			this.QuitButton.MouseEnter += new System.EventHandler(this.QuitButton_MouseEnter);
			this.QuitButton.MouseLeave += new System.EventHandler(this.QuitButton_MouseLeave);
			// 
			// OptionsButton
			// 
			this.OptionsButton.Image = global::SpaceInvaders.Properties.Resources.options;
			this.OptionsButton.Location = new System.Drawing.Point(560, 703);
			this.OptionsButton.Margin = new System.Windows.Forms.Padding(0);
			this.OptionsButton.Name = "OptionsButton";
			this.OptionsButton.Size = new System.Drawing.Size(280, 65);
			this.OptionsButton.TabIndex = 7;
			this.OptionsButton.UseVisualStyleBackColor = true;
			this.OptionsButton.Visible = false;
			this.OptionsButton.MouseEnter += new System.EventHandler(this.OptionsButton_MouseEnter);
			this.OptionsButton.MouseLeave += new System.EventHandler(this.OptionsButton_MouseLeave);
			// 
			// HighscoreButton
			// 
			this.HighscoreButton.Image = global::SpaceInvaders.Properties.Resources.highscore;
			this.HighscoreButton.Location = new System.Drawing.Point(280, 703);
			this.HighscoreButton.Margin = new System.Windows.Forms.Padding(0);
			this.HighscoreButton.Name = "HighscoreButton";
			this.HighscoreButton.Size = new System.Drawing.Size(280, 65);
			this.HighscoreButton.TabIndex = 6;
			this.HighscoreButton.UseVisualStyleBackColor = true;
			this.HighscoreButton.Click += new System.EventHandler(this.HighscoreButton_Click);
			this.HighscoreButton.MouseEnter += new System.EventHandler(this.HighscoreButton_MouseEnter);
			this.HighscoreButton.MouseLeave += new System.EventHandler(this.HighscoreButton_MouseLeave);
			// 
			// NewGameButton
			// 
			this.NewGameButton.Image = global::SpaceInvaders.Properties.Resources.newgame;
			this.NewGameButton.Location = new System.Drawing.Point(0, 703);
			this.NewGameButton.Margin = new System.Windows.Forms.Padding(0);
			this.NewGameButton.Name = "NewGameButton";
			this.NewGameButton.Size = new System.Drawing.Size(280, 65);
			this.NewGameButton.TabIndex = 5;
			this.NewGameButton.UseVisualStyleBackColor = true;
			this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
			this.NewGameButton.MouseEnter += new System.EventHandler(this.NewGameButton_MouseEnter);
			this.NewGameButton.MouseLeave += new System.EventHandler(this.NewGameButton_MouseLeave);
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
			this.CreditsButton.Font = new System.Drawing.Font("Calibri Light", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CreditsButton.Location = new System.Drawing.Point(559, 703);
			this.CreditsButton.Name = "CreditsButton";
			this.CreditsButton.Size = new System.Drawing.Size(280, 65);
			this.CreditsButton.TabIndex = 11;
			this.CreditsButton.Text = "CREDITS";
			this.CreditsButton.UseVisualStyleBackColor = true;
			this.CreditsButton.Click += new System.EventHandler(this.CreditsButton_Click);
			// 
			// GameMainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.Controls.Add(this.CreditsButton);
			this.Controls.Add(this.backgroundPictureBox);
			this.Controls.Add(this.QuitButton);
			this.Controls.Add(this.OptionsButton);
			this.Controls.Add(this.HighscoreButton);
			this.Controls.Add(this.NewGameButton);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "GameMainMenu";
			this.Size = new System.Drawing.Size(1024, 768);
			((System.ComponentModel.ISupportInitialize)(this.backgroundPictureBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button NewGameButton;
        public System.Windows.Forms.Button OptionsButton;
        public System.Windows.Forms.Button HighscoreButton;
		public System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.PictureBox backgroundPictureBox;
		private System.Windows.Forms.Button CreditsButton;
    }
}
