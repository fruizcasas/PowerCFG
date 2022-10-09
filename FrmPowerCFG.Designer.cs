namespace PowerCFG
{
    partial class FrmPowerCFG
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPowerCFG));
            this.tv = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ExpandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CollapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyGUIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActivateSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.UnhiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.RangeEditor = new PowerCFG.Components.RangeEditor();
            this.DropDownEditor = new PowerCFG.Components.DropDownEditor();
            this.ShowGuidsCheckBox = new System.Windows.Forms.CheckBox();
            this.ExcelExportButton = new System.Windows.Forms.Button();
            this.PowerSchemeSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.PowerSchemeOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tv
            // 
            resources.ApplyResources(this.tv, "tv");
            this.tv.ContextMenuStrip = this.contextMenuStrip;
            this.tv.ImageList = this.imageList;
            this.tv.LabelEdit = true;
            this.tv.Name = "tv";
            this.tv.ShowNodeToolTips = true;
            this.tv.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tv_BeforeLabelEdit);
            this.tv.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tv_AfterLabelEdit);
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.DoubleClick += new System.EventHandler(this.tv_DoubleClick);
            this.tv.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tv_MouseClick);
            this.tv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_MouseDown);
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExpandToolStripMenuItem,
            this.CollapseToolStripMenuItem,
            this.CopyGUIDToolStripMenuItem,
            this.HiddenToolStripMenuItem,
            this.ShowToolStripMenuItem,
            this.ImportSchemeToolStripMenuItem,
            this.ActivateSchemeToolStripMenuItem,
            this.SaveSchemeToolStripMenuItem,
            this.DuplicateSchemeToolStripMenuItem,
            this.DeleteSchemeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // ExpandToolStripMenuItem
            // 
            resources.ApplyResources(this.ExpandToolStripMenuItem, "ExpandToolStripMenuItem");
            this.ExpandToolStripMenuItem.Name = "ExpandToolStripMenuItem";
            this.ExpandToolStripMenuItem.Click += new System.EventHandler(this.ExpandToolStripMenuItem_Click);
            // 
            // CollapseToolStripMenuItem
            // 
            resources.ApplyResources(this.CollapseToolStripMenuItem, "CollapseToolStripMenuItem");
            this.CollapseToolStripMenuItem.Name = "CollapseToolStripMenuItem";
            this.CollapseToolStripMenuItem.Click += new System.EventHandler(this.CollapseToolStripMenuItem_Click);
            // 
            // CopyGUIDToolStripMenuItem
            // 
            resources.ApplyResources(this.CopyGUIDToolStripMenuItem, "CopyGUIDToolStripMenuItem");
            this.CopyGUIDToolStripMenuItem.Name = "CopyGUIDToolStripMenuItem";
            this.CopyGUIDToolStripMenuItem.Click += new System.EventHandler(this.CopyGUIDToolStripMenuItem_Click);
            // 
            // HiddenToolStripMenuItem
            // 
            resources.ApplyResources(this.HiddenToolStripMenuItem, "HiddenToolStripMenuItem");
            this.HiddenToolStripMenuItem.Name = "HiddenToolStripMenuItem";
            this.HiddenToolStripMenuItem.Click += new System.EventHandler(this.HiddenToolStripMenuItem_Click);
            // 
            // ShowToolStripMenuItem
            // 
            resources.ApplyResources(this.ShowToolStripMenuItem, "ShowToolStripMenuItem");
            this.ShowToolStripMenuItem.Name = "ShowToolStripMenuItem";
            this.ShowToolStripMenuItem.Click += new System.EventHandler(this.ShowToolStripMenuItem_Click);
            // 
            // ImportSchemeToolStripMenuItem
            // 
            resources.ApplyResources(this.ImportSchemeToolStripMenuItem, "ImportSchemeToolStripMenuItem");
            this.ImportSchemeToolStripMenuItem.Name = "ImportSchemeToolStripMenuItem";
            this.ImportSchemeToolStripMenuItem.Click += new System.EventHandler(this.ImportSchemeToolStripMenuItem_Click);
            // 
            // ActivateSchemeToolStripMenuItem
            // 
            resources.ApplyResources(this.ActivateSchemeToolStripMenuItem, "ActivateSchemeToolStripMenuItem");
            this.ActivateSchemeToolStripMenuItem.Name = "ActivateSchemeToolStripMenuItem";
            this.ActivateSchemeToolStripMenuItem.Click += new System.EventHandler(this.ActivateSchemeToolStripMenuItem_Click);
            // 
            // SaveSchemeToolStripMenuItem
            // 
            resources.ApplyResources(this.SaveSchemeToolStripMenuItem, "SaveSchemeToolStripMenuItem");
            this.SaveSchemeToolStripMenuItem.Name = "SaveSchemeToolStripMenuItem";
            this.SaveSchemeToolStripMenuItem.Click += new System.EventHandler(this.SaveSchemeToolStripMenuItem_Click);
            // 
            // DuplicateSchemeToolStripMenuItem
            // 
            resources.ApplyResources(this.DuplicateSchemeToolStripMenuItem, "DuplicateSchemeToolStripMenuItem");
            this.DuplicateSchemeToolStripMenuItem.Name = "DuplicateSchemeToolStripMenuItem";
            this.DuplicateSchemeToolStripMenuItem.Click += new System.EventHandler(this.DuplicateSchemeToolStripMenuItem_Click);
            // 
            // DeleteSchemeToolStripMenuItem
            // 
            resources.ApplyResources(this.DeleteSchemeToolStripMenuItem, "DeleteSchemeToolStripMenuItem");
            this.DeleteSchemeToolStripMenuItem.Name = "DeleteSchemeToolStripMenuItem";
            this.DeleteSchemeToolStripMenuItem.Click += new System.EventHandler(this.DeleteSchemeToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "512");
            this.imageList.Images.SetKeyName(2, "513");
            this.imageList.Images.SetKeyName(3, "514");
            this.imageList.Images.SetKeyName(4, "610");
            this.imageList.Images.SetKeyName(5, "node");
            // 
            // UnhiddenCheckBox
            // 
            resources.ApplyResources(this.UnhiddenCheckBox, "UnhiddenCheckBox");
            this.UnhiddenCheckBox.Checked = true;
            this.UnhiddenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UnhiddenCheckBox.Name = "UnhiddenCheckBox";
            this.UnhiddenCheckBox.UseVisualStyleBackColor = true;
            this.UnhiddenCheckBox.CheckedChanged += new System.EventHandler(this.UnhiddenCheckBox_CheckedChanged);
            // 
            // PictureBox
            // 
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // RangeEditor
            // 
            resources.ApplyResources(this.RangeEditor, "RangeEditor");
            this.RangeEditor.BackColor = System.Drawing.SystemColors.Window;
            this.RangeEditor.DCMode = false;
            this.RangeEditor.Name = "RangeEditor";
            this.RangeEditor.Node = null;
            this.RangeEditor.Setting = null;
            this.RangeEditor.RestoreDefaultClick += new PowerCFG.Components.RestoreDefaultClickHandler(this.RangeEditor_RestoreDefaultClick);
            // 
            // DropDownEditor
            // 
            resources.ApplyResources(this.DropDownEditor, "DropDownEditor");
            this.DropDownEditor.BackColor = System.Drawing.SystemColors.Window;
            this.DropDownEditor.DCMode = false;
            this.DropDownEditor.Name = "DropDownEditor";
            this.DropDownEditor.Node = null;
            this.DropDownEditor.Setting = null;
            this.DropDownEditor.RestoreDefaultClick += new PowerCFG.Components.RestoreDefaultClickHandler(this.DropDownEditor_RestoreDefaultClick);
            // 
            // ShowGuidsCheckBox
            // 
            resources.ApplyResources(this.ShowGuidsCheckBox, "ShowGuidsCheckBox");
            this.ShowGuidsCheckBox.Name = "ShowGuidsCheckBox";
            this.ShowGuidsCheckBox.UseVisualStyleBackColor = true;
            this.ShowGuidsCheckBox.CheckedChanged += new System.EventHandler(this.ShowGuidsCheckBox_CheckedChanged);
            // 
            // ExcelExportButton
            // 
            resources.ApplyResources(this.ExcelExportButton, "ExcelExportButton");
            this.ExcelExportButton.Image = global::PowerCFG.Properties.Resources.excel;
            this.ExcelExportButton.Name = "ExcelExportButton";
            this.ExcelExportButton.UseVisualStyleBackColor = true;
            this.ExcelExportButton.Click += new System.EventHandler(this.ExcelExportButton_Click);
            // 
            // PowerSchemeSaveFileDialog
            // 
            this.PowerSchemeSaveFileDialog.DefaultExt = "pow";
            resources.ApplyResources(this.PowerSchemeSaveFileDialog, "PowerSchemeSaveFileDialog");
            // 
            // PowerSchemeOpenFileDialog
            // 
            this.PowerSchemeOpenFileDialog.DefaultExt = "pow";
            resources.ApplyResources(this.PowerSchemeOpenFileDialog, "PowerSchemeOpenFileDialog");
            // 
            // FrmPowerCFG
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ExcelExportButton);
            this.Controls.Add(this.ShowGuidsCheckBox);
            this.Controls.Add(this.DropDownEditor);
            this.Controls.Add(this.RangeEditor);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.UnhiddenCheckBox);
            this.Controls.Add(this.tv);
            this.Name = "FrmPowerCFG";
            this.Load += new System.EventHandler(this.FrmPowerCFG_Load);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TreeView tv;
        public CheckBox UnhiddenCheckBox;
        public ContextMenuStrip contextMenuStrip;
        public ToolStripMenuItem HiddenToolStripMenuItem;
        public ToolStripMenuItem ShowToolStripMenuItem;
        public ImageList imageList;
        public PictureBox PictureBox;
        public Components.RangeEditor RangeEditor;
        public Components.DropDownEditor DropDownEditor;
        public ToolStripMenuItem ActivateSchemeToolStripMenuItem;
        public ToolStripMenuItem ExpandToolStripMenuItem;
        public ToolStripMenuItem CollapseToolStripMenuItem;
        public CheckBox ShowGuidsCheckBox;
        public ToolStripMenuItem CopyGUIDToolStripMenuItem;
        public Button ExcelExportButton;
        public ToolStripMenuItem SaveSchemeToolStripMenuItem;
        public ToolStripMenuItem ImportSchemeToolStripMenuItem;
        public ToolStripMenuItem DeleteSchemeToolStripMenuItem;
        public ToolStripMenuItem DuplicateSchemeToolStripMenuItem;
        public SaveFileDialog PowerSchemeSaveFileDialog;
        private OpenFileDialog PowerSchemeOpenFileDialog;
    }
}