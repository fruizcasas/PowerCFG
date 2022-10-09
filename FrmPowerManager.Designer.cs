namespace PowerCFG
{
    partial class FrmPowerManager
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
            this.tv = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tv
            // 
            this.tv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv.Location = new System.Drawing.Point(23, 12);
            this.tv.Name = "tv";
            this.tv.ShowNodeToolTips = true;
            this.tv.Size = new System.Drawing.Size(512, 426);
            this.tv.TabIndex = 0;
            // 
            // FrmPowerManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tv);
            this.Name = "FrmPowerManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Power Manager";
            this.Load += new System.EventHandler(this.FrmPowerManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView tv;
    }
}