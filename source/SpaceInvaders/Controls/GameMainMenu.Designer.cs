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
            this.titlePictureBox = new System.Windows.Forms.PictureBox();
            this.QuitButton = new System.Windows.Forms.Button();
            this.OptionsButton = new System.Windows.Forms.Button();
            this.HighscoreButton = new System.Windows.Forms.Button();
            this.NewGameButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.titlePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePictureBox
            // 
            this.titlePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("titlePictureBox.Image")));
            this.titlePictureBox.Location = new System.Drawing.Point(0, 0);
            this.titlePictureBox.Name = "titlePictureBox";
            this.titlePictureBox.Size = new System.Drawing.Size(400, 100);
            this.titlePictureBox.TabIndex = 9;
            this.titlePictureBox.TabStop = false;
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(125, 240);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(150, 23);
            this.QuitButton.TabIndex = 8;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // OptionsButton
            // 
            this.OptionsButton.Location = new System.Drawing.Point(125, 210);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(150, 23);
            this.OptionsButton.TabIndex = 7;
            this.OptionsButton.Text = "Options";
            this.OptionsButton.UseVisualStyleBackColor = true;
            // 
            // HighscoreButton
            // 
            this.HighscoreButton.Location = new System.Drawing.Point(125, 180);
            this.HighscoreButton.Name = "HighscoreButton";
            this.HighscoreButton.Size = new System.Drawing.Size(150, 23);
            this.HighscoreButton.TabIndex = 6;
            this.HighscoreButton.Text = "Highscore";
            this.HighscoreButton.UseVisualStyleBackColor = true;
            // 
            // NewGameButton
            // 
            this.NewGameButton.Location = new System.Drawing.Point(125, 150);
            this.NewGameButton.Name = "NewGameButton";
            this.NewGameButton.Size = new System.Drawing.Size(150, 23);
            this.NewGameButton.TabIndex = 5;
            this.NewGameButton.Text = "New Game";
            this.NewGameButton.UseVisualStyleBackColor = true;
            this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
            // 
            // GameMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.Controls.Add(this.titlePictureBox);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.OptionsButton);
            this.Controls.Add(this.HighscoreButton);
            this.Controls.Add(this.NewGameButton);
            this.Name = "GameMainMenu";
            this.Size = new System.Drawing.Size(400, 300);
            ((System.ComponentModel.ISupportInitialize)(this.titlePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox titlePictureBox;
        public System.Windows.Forms.Button NewGameButton;
        public System.Windows.Forms.Button OptionsButton;
        public System.Windows.Forms.Button HighscoreButton;
        public System.Windows.Forms.Button QuitButton;
    }
}
