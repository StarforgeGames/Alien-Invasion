namespace SpaceInvaders.Controls
{
    partial class PauseScreen
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
			this.pausedLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pausedLabel
			// 
			this.pausedLabel.AutoSize = true;
			this.pausedLabel.Font = new System.Drawing.Font("Algerian", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pausedLabel.ForeColor = System.Drawing.Color.White;
			this.pausedLabel.Location = new System.Drawing.Point(9, 0);
			this.pausedLabel.Name = "pausedLabel";
			this.pausedLabel.Size = new System.Drawing.Size(208, 54);
			this.pausedLabel.TabIndex = 0;
			this.pausedLabel.Text = "PAUSED";
			// 
			// PauseScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.WindowText;
			this.Controls.Add(this.pausedLabel);
			this.Name = "PauseScreen";
			this.Size = new System.Drawing.Size(220, 55);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pausedLabel;
    }
}
