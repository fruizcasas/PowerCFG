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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeEditor));
            this.NameLabel = new System.Windows.Forms.Label();
            this.ValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RestoreDefaultButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ValueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            resources.ApplyResources(this.NameLabel, "NameLabel");
            this.NameLabel.Name = "NameLabel";
            // 
            // ValueNumericUpDown
            // 
            resources.ApplyResources(this.ValueNumericUpDown, "ValueNumericUpDown");
            this.ValueNumericUpDown.Name = "ValueNumericUpDown";
            this.ValueNumericUpDown.ValueChanged += new System.EventHandler(this.ValueNumericUpDown_ValueChanged);
            this.ValueNumericUpDown.Leave += new System.EventHandler(this.ValueNumericUpDown_Leave);
            // 
            // RestoreDefaultButton
            // 
            resources.ApplyResources(this.RestoreDefaultButton, "RestoreDefaultButton");
            this.RestoreDefaultButton.Name = "RestoreDefaultButton";
            this.RestoreDefaultButton.UseVisualStyleBackColor = true;
            this.RestoreDefaultButton.Click += new System.EventHandler(this.RestoreDefaultButton_Click);
            // 
            // RangeEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ValueNumericUpDown);
            this.Controls.Add(this.RestoreDefaultButton);
            this.Name = "RangeEditor";
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
