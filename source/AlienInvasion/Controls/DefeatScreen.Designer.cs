namespace AlienInvasion.Controls
{
    partial class DefeatScreen
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
			this.gameOverLabel = new System.Windows.Forms.Label();
			this.continueButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// gameOverLabel
			// 
			this.gameOverLabel.AutoSize = true;
			this.gameOverLabel.Font = new System.Drawing.Font("Algerian", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gameOverLabel.ForeColor = System.Drawing.Color.White;
			this.gameOverLabel.Location = new System.Drawing.Point(170, 25);
			this.gameOverLabel.Name = "gameOverLabel";
			this.gameOverLabel.Size = new System.Drawing.Size(290, 71);
			this.gameOverLabel.TabIndex = 0;
			this.gameOverLabel.Text = "Defeat!";
			// 
			// continueButton
			// 
			this.continueButton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.continueButton.Location = new System.Drawing.Point(200, 160);
			this.continueButton.Name = "continueButton";
			this.continueButton.Size = new System.Drawing.Size(200, 30);
			this.continueButton.TabIndex = 1;
			this.continueButton.Text = "Continue";
			this.continueButton.UseVisualStyleBackColor = true;
			this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
			// 
			// DefeatScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Firebrick;
			this.Controls.Add(this.continueButton);
			this.Controls.Add(this.gameOverLabel);
			this.Name = "DefeatScreen";
			this.Size = new System.Drawing.Size(600, 200);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label gameOverLabel;
		private System.Windows.Forms.Button continueButton;
    }
}
