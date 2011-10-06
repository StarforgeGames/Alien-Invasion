namespace SpaceInvaders.Controls
{
    partial class Hud
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scoreLabel = new System.Windows.Forms.Label();
            this.scoreValueLabel = new System.Windows.Forms.Label();
            this.lifesLabel = new System.Windows.Forms.Label();
            this.lifesValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.ForeColor = System.Drawing.Color.White;
            this.scoreLabel.Location = new System.Drawing.Point(3, 2);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(60, 19);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "Score:";
            // 
            // scoreValueLabel
            // 
            this.scoreValueLabel.AutoSize = true;
            this.scoreValueLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreValueLabel.ForeColor = System.Drawing.Color.White;
            this.scoreValueLabel.Location = new System.Drawing.Point(69, 2);
            this.scoreValueLabel.Name = "scoreValueLabel";
            this.scoreValueLabel.Size = new System.Drawing.Size(19, 19);
            this.scoreValueLabel.TabIndex = 1;
            this.scoreValueLabel.Text = "0";
            // 
            // lifesLabel
            // 
            this.lifesLabel.AutoSize = true;
            this.lifesLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lifesLabel.ForeColor = System.Drawing.Color.White;
            this.lifesLabel.Location = new System.Drawing.Point(197, 2);
            this.lifesLabel.Name = "lifesLabel";
            this.lifesLabel.Size = new System.Drawing.Size(53, 19);
            this.lifesLabel.TabIndex = 2;
            this.lifesLabel.Text = "Lifes:";
            // 
            // lifesValueLabel
            // 
            this.lifesValueLabel.AutoSize = true;
            this.lifesValueLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lifesValueLabel.ForeColor = System.Drawing.Color.White;
            this.lifesValueLabel.Location = new System.Drawing.Point(256, 2);
            this.lifesValueLabel.Name = "lifesValueLabel";
            this.lifesValueLabel.Size = new System.Drawing.Size(19, 19);
            this.lifesValueLabel.TabIndex = 3;
            this.lifesValueLabel.Text = "0";
            // 
            // Hud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.lifesValueLabel);
            this.Controls.Add(this.lifesLabel);
            this.Controls.Add(this.scoreValueLabel);
            this.Controls.Add(this.scoreLabel);
            this.Name = "Hud";
            this.Size = new System.Drawing.Size(300, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label scoreValueLabel;
        private System.Windows.Forms.Label lifesLabel;
        private System.Windows.Forms.Label lifesValueLabel;
    }
}
