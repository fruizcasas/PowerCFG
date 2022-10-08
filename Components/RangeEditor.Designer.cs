namespace PowerCFG.Components
{
    partial class RangeEditor
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RestoreDefaultButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ValueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(171, 23);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "label1";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ValueNumericUpDown
            // 
            this.ValueNumericUpDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.ValueNumericUpDown.Location = new System.Drawing.Point(171, 0);
            this.ValueNumericUpDown.Name = "ValueNumericUpDown";
            this.ValueNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.ValueNumericUpDown.TabIndex = 2;
            this.ValueNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ValueNumericUpDown.ValueChanged += new System.EventHandler(this.ValueNumericUpDown_ValueChanged);
            this.ValueNumericUpDown.Leave += new System.EventHandler(this.ValueNumericUpDown_Leave);
            // 
            // RestoreDefaultButton
            // 
            this.RestoreDefaultButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.RestoreDefaultButton.Location = new System.Drawing.Point(291, 0);
            this.RestoreDefaultButton.Name = "RestoreDefaultButton";
            this.RestoreDefaultButton.Size = new System.Drawing.Size(40, 23);
            this.RestoreDefaultButton.TabIndex = 4;
            this.RestoreDefaultButton.Text = "Def.";
            this.RestoreDefaultButton.UseVisualStyleBackColor = true;
            this.RestoreDefaultButton.Click += new System.EventHandler(this.RestoreDefaultButton_Click);
            // 
            // RangeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ValueNumericUpDown);
            this.Controls.Add(this.RestoreDefaultButton);
            this.Name = "RangeEditor";
            this.Size = new System.Drawing.Size(331, 23);
            ((System.ComponentModel.ISupportInitialize)(this.ValueNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label NameLabel;
        private NumericUpDown ValueNumericUpDown;
        private Button RestoreDefaultButton;
        private ToolTip toolTip;
    }
}
