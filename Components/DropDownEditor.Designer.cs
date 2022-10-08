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
            this.NameLabel = new System.Windows.Forms.Label();
            this.ValueComboBox = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RestoreDefaultButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(118, 23);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "label1";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ValueComboBox
            // 
            this.ValueComboBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ValueComboBox.FormattingEnabled = true;
            this.ValueComboBox.Location = new System.Drawing.Point(118, 0);
            this.ValueComboBox.Name = "ValueComboBox";
            this.ValueComboBox.Size = new System.Drawing.Size(180, 23);
            this.ValueComboBox.TabIndex = 2;
            this.ValueComboBox.SelectedIndexChanged += new System.EventHandler(this.ValueComboBox_SelectedIndexChanged);
            this.ValueComboBox.Leave += new System.EventHandler(this.ValueComboBox_Leave);
            // 
            // RestoreDefaultButton
            // 
            this.RestoreDefaultButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.RestoreDefaultButton.Location = new System.Drawing.Point(298, 0);
            this.RestoreDefaultButton.Name = "RestoreDefaultButton";
            this.RestoreDefaultButton.Size = new System.Drawing.Size(40, 23);
            this.RestoreDefaultButton.TabIndex = 3;
            this.RestoreDefaultButton.Text = "Def.";
            this.RestoreDefaultButton.UseVisualStyleBackColor = true;
            this.RestoreDefaultButton.Click += new System.EventHandler(this.RestoreDefaultButton_Click);
            // 
            // DropDownEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ValueComboBox);
            this.Controls.Add(this.RestoreDefaultButton);
            this.Name = "DropDownEditor";
            this.Size = new System.Drawing.Size(338, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private Label NameLabel;
        private ComboBox ValueComboBox;
        private ToolTip toolTip;
        private Button RestoreDefaultButton;
    }
}
