namespace SpaceInvaders.Controls
{
    partial class VictoryScreen
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
			this.victoryLabel = new System.Windows.Forms.Label();
			this.continueButton = new System.Windows.Forms.Button();
			this.newHighscoreGroupBox = new System.Windows.Forms.GroupBox();
			this.playerNameTextBox = new System.Windows.Forms.TextBox();
			this.playerNameLabel = new System.Windows.Forms.Label();
			this.newHighscoreGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// victoryLabel
			// 
			this.victoryLabel.AutoSize = true;
			this.victoryLabel.Font = new System.Drawing.Font("Algerian", 48F);
			this.victoryLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.victoryLabel.Location = new System.Drawing.Point(150, 25);
			this.victoryLabel.Name = "victoryLabel";
			this.victoryLabel.Size = new System.Drawing.Size(307, 71);
			this.victoryLabel.TabIndex = 0;
			this.victoryLabel.Text = "Victory!";
			// 
			// continueButton
			// 
			this.continueButton.Font = new System.Drawing.Font("Century Gothic", 12F);
			this.continueButton.Location = new System.Drawing.Point(200, 160);
			this.continueButton.Name = "continueButton";
			this.continueButton.Size = new System.Drawing.Size(200, 30);
			this.continueButton.TabIndex = 2;
			this.continueButton.Text = "Continue";
			this.continueButton.UseVisualStyleBackColor = true;
			this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
			// 
			// newHighscoreGroupBox
			// 
			this.newHighscoreGroupBox.Controls.Add(this.playerNameTextBox);
			this.newHighscoreGroupBox.Controls.Add(this.playerNameLabel);
			this.newHighscoreGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.newHighscoreGroupBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newHighscoreGroupBox.ForeColor = System.Drawing.Color.Red;
			this.newHighscoreGroupBox.Location = new System.Drawing.Point(200, 95);
			this.newHighscoreGroupBox.Name = "newHighscoreGroupBox";
			this.newHighscoreGroupBox.Size = new System.Drawing.Size(200, 55);
			this.newHighscoreGroupBox.TabIndex = 3;
			this.newHighscoreGroupBox.TabStop = false;
			this.newHighscoreGroupBox.Text = "New Highscore";
			this.newHighscoreGroupBox.Visible = false;
			// 
			// playerNameTextBox
			// 
			this.playerNameTextBox.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.playerNameTextBox.Location = new System.Drawing.Point(63, 22);
			this.playerNameTextBox.Name = "playerNameTextBox";
			this.playerNameTextBox.Size = new System.Drawing.Size(130, 23);
			this.playerNameTextBox.TabIndex = 1;
			this.playerNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.playerNameTextBox_KeyUp);
			// 
			// playerNameLabel
			// 
			this.playerNameLabel.AutoSize = true;
			this.playerNameLabel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.playerNameLabel.ForeColor = System.Drawing.Color.White;
			this.playerNameLabel.Location = new System.Drawing.Point(7, 26);
			this.playerNameLabel.Name = "playerNameLabel";
			this.playerNameLabel.Size = new System.Drawing.Size(52, 17);
			this.playerNameLabel.TabIndex = 0;
			this.playerNameLabel.Text = "Name:";
			// 
			// VictoryScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.HotTrack;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.newHighscoreGroupBox);
			this.Controls.Add(this.continueButton);
			this.Controls.Add(this.victoryLabel);
			this.Name = "VictoryScreen";
			this.Size = new System.Drawing.Size(600, 200);
			this.VisibleChanged += new System.EventHandler(this.VictoryScreen_VisibleChanged);
			this.newHighscoreGroupBox.ResumeLayout(false);
			this.newHighscoreGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label victoryLabel;
		private System.Windows.Forms.Button continueButton;
		private System.Windows.Forms.GroupBox newHighscoreGroupBox;
		private System.Windows.Forms.TextBox playerNameTextBox;
		private System.Windows.Forms.Label playerNameLabel;
    }
}
