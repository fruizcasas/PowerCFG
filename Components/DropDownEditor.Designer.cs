namespace PowerCFG.Components
{
    partial class DropDownEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropDownEditor));
            this.NameLabel = new System.Windows.Forms.Label();
            this.ValueComboBox = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RestoreDefaultButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            resources.ApplyResources(this.NameLabel, "NameLabel");
            this.NameLabel.Name = "NameLabel";
            // 
            // ValueComboBox
            // 
            resources.ApplyResources(this.ValueComboBox, "ValueComboBox");
            this.ValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ValueComboBox.FormattingEnabled = true;
            this.ValueComboBox.Name = "ValueComboBox";
            this.ValueComboBox.SelectedIndexChanged += new System.EventHandler(this.ValueComboBox_SelectedIndexChanged);
            this.ValueComboBox.Leave += new System.EventHandler(this.ValueComboBox_Leave);
            // 
            // RestoreDefaultButton
            // 
            resources.ApplyResources(this.RestoreDefaultButton, "RestoreDefaultButton");
            this.RestoreDefaultButton.Name = "RestoreDefaultButton";
            this.RestoreDefaultButton.UseVisualStyleBackColor = true;
            this.RestoreDefaultButton.Click += new System.EventHandler(this.RestoreDefaultButton_Click);
            // 
            // DropDownEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ValueComboBox);
            this.Controls.Add(this.RestoreDefaultButton);
            this.Name = "DropDownEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private Label NameLabel;
        private ComboBox ValueComboBox;
        private ToolTip toolTip;
        private Button RestoreDefaultButton;
    }
}
