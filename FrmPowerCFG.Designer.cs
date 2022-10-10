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
            this.HiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.RangeEditor = new PowerCFG.Components.RangeEditor();
            this.DropDownEditor = new PowerCFG.Components.DropDownEditor();
            this.ShowGuidsCheckBox = new System.Windows.Forms.CheckBox();
            this.ExcelExportButton = new System.Windows.Forms.Button();
            this.PowerSchemeSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.PowerSchemeOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BatteryReportButton = new System.Windows.Forms.Button();
            this.EnergyReportButton = new System.Windows.Forms.Button();
            this.EnergyProgressBar = new System.Windows.Forms.ProgressBar();
            this.PowerReportButton = new System.Windows.Forms.Button();
            this.PowerReportBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.EnergyReportBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.PowerProgressBar = new System.Windows.Forms.ProgressBar();
            this.BatteryBackgroundWorker = new System.ComponentModel.BackgroundWorker();
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
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // ExpandToolStripMenuItem
            // 
            this.ExpandToolStripMenuItem.Name = "ExpandToolStripMenuItem";
            resources.ApplyResources(this.ExpandToolStripMenuItem, "ExpandToolStripMenuItem");
            this.ExpandToolStripMenuItem.Click += new System.EventHandler(this.ExpandToolStripMenuItem_Click);
            // 
            // CollapseToolStripMenuItem
            // 
            this.CollapseToolStripMenuItem.Name = "CollapseToolStripMenuItem";
            resources.ApplyResources(this.CollapseToolStripMenuItem, "CollapseToolStripMenuItem");
            this.CollapseToolStripMenuItem.Click += new System.EventHandler(this.CollapseToolStripMenuItem_Click);
            // 
            // CopyGUIDToolStripMenuItem
            // 
            this.CopyGUIDToolStripMenuItem.Name = "CopyGUIDToolStripMenuItem";
            resources.ApplyResources(this.CopyGUIDToolStripMenuItem, "CopyGUIDToolStripMenuItem");
            this.CopyGUIDToolStripMenuItem.Click += new System.EventHandler(this.CopyGUIDToolStripMenuItem_Click);
            // 
            // HiddenToolStripMenuItem
            // 
            this.HiddenToolStripMenuItem.Name = "HiddenToolStripMenuItem";
            resources.ApplyResources(this.HiddenToolStripMenuItem, "HiddenToolStripMenuItem");
            this.HiddenToolStripMenuItem.Click += new System.EventHandler(this.HiddenToolStripMenuItem_Click);
            // 
            // ShowToolStripMenuItem
            // 
            this.ShowToolStripMenuItem.Name = "ShowToolStripMenuItem";
            resources.ApplyResources(this.ShowToolStripMenuItem, "ShowToolStripMenuItem");
            this.ShowToolStripMenuItem.Click += new System.EventHandler(this.ShowToolStripMenuItem_Click);
            // 
            // ImportSchemeToolStripMenuItem
            // 
            this.ImportSchemeToolStripMenuItem.Name = "ImportSchemeToolStripMenuItem";
            resources.ApplyResources(this.ImportSchemeToolStripMenuItem, "ImportSchemeToolStripMenuItem");
            this.ImportSchemeToolStripMenuItem.Click += new System.EventHandler(this.ImportSchemeToolStripMenuItem_Click);
            // 
            // ActivateSchemeToolStripMenuItem
            // 
            this.ActivateSchemeToolStripMenuItem.Name = "ActivateSchemeToolStripMenuItem";
            resources.ApplyResources(this.ActivateSchemeToolStripMenuItem, "ActivateSchemeToolStripMenuItem");
            this.ActivateSchemeToolStripMenuItem.Click += new System.EventHandler(this.ActivateSchemeToolStripMenuItem_Click);
            // 
            // SaveSchemeToolStripMenuItem
            // 
            this.SaveSchemeToolStripMenuItem.Name = "SaveSchemeToolStripMenuItem";
            resources.ApplyResources(this.SaveSchemeToolStripMenuItem, "SaveSchemeToolStripMenuItem");
            this.SaveSchemeToolStripMenuItem.Click += new System.EventHandler(this.SaveSchemeToolStripMenuItem_Click);
            // 
            // DuplicateSchemeToolStripMenuItem
            // 
            this.DuplicateSchemeToolStripMenuItem.Name = "DuplicateSchemeToolStripMenuItem";
            resources.ApplyResources(this.DuplicateSchemeToolStripMenuItem, "DuplicateSchemeToolStripMenuItem");
            this.DuplicateSchemeToolStripMenuItem.Click += new System.EventHandler(this.DuplicateSchemeToolStripMenuItem_Click);
            // 
            // DeleteSchemeToolStripMenuItem
            // 
            this.DeleteSchemeToolStripMenuItem.Name = "DeleteSchemeToolStripMenuItem";
            resources.ApplyResources(this.DeleteSchemeToolStripMenuItem, "DeleteSchemeToolStripMenuItem");
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
            // 
            // HiddenCheckBox
            // 
            resources.ApplyResources(this.HiddenCheckBox, "HiddenCheckBox");
            this.HiddenCheckBox.Name = "HiddenCheckBox";
            this.HiddenCheckBox.UseVisualStyleBackColor = true;
            this.HiddenCheckBox.CheckedChanged += new System.EventHandler(this.HiddenCheckBox_CheckedChanged);
            // 
            // PictureBox
            // 
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // RangeEditor
            // 
            this.RangeEditor.BackColor = System.Drawing.SystemColors.Window;
            this.RangeEditor.DCMode = false;
            resources.ApplyResources(this.RangeEditor, "RangeEditor");
            this.RangeEditor.Name = "RangeEditor";
            this.RangeEditor.Node = null;
            this.RangeEditor.Setting = null;
            this.RangeEditor.RestoreDefaultClick += new PowerCFG.Components.RestoreDefaultClickHandler(this.RangeEditor_RestoreDefaultClick);
            // 
            // DropDownEditor
            // 
            this.DropDownEditor.BackColor = System.Drawing.SystemColors.Window;
            this.DropDownEditor.DCMode = false;
            resources.ApplyResources(this.DropDownEditor, "DropDownEditor");
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
            this.ShowGuidsCheckBox.Click += new System.EventHandler(this.ShowGuidsCheckBox_CheckedChanged);
            // 
            // ExcelExportButton
            // 
            resources.ApplyResources(this.ExcelExportButton, "ExcelExportButton");
            this.ExcelExportButton.BackColor = System.Drawing.SystemColors.Control;
            this.ExcelExportButton.Image = global::PowerCFG.Properties.Resources.excel;
            this.ExcelExportButton.Name = "ExcelExportButton";
            this.ExcelExportButton.UseVisualStyleBackColor = false;
            this.ExcelExportButton.Click += new System.EventHandler(this.ExcelExportButton_Click);
            // 
            // PowerSchemeSaveFileDialog
            // 
            this.PowerSchemeSaveFileDialog.DefaultExt = "pow";
            // 
            // PowerSchemeOpenFileDialog
            // 
            this.PowerSchemeOpenFileDialog.DefaultExt = "pow";
            // 
            // BatteryReportButton
            // 
            resources.ApplyResources(this.BatteryReportButton, "BatteryReportButton");
            this.BatteryReportButton.BackColor = System.Drawing.SystemColors.Control;
            this.BatteryReportButton.Image = global::PowerCFG.Properties.Resources.battery_charged;
            this.BatteryReportButton.Name = "BatteryReportButton";
            this.BatteryReportButton.UseVisualStyleBackColor = false;
            this.BatteryReportButton.Click += new System.EventHandler(this.BatteryReportButton_Click);
            // 
            // EnergyReportButton
            // 
            resources.ApplyResources(this.EnergyReportButton, "EnergyReportButton");
            this.EnergyReportButton.BackColor = System.Drawing.SystemColors.Control;
            this.EnergyReportButton.Image = global::PowerCFG.Properties.Resources.Status_battery_charging;
            this.EnergyReportButton.Name = "EnergyReportButton";
            this.EnergyReportButton.UseVisualStyleBackColor = false;
            this.EnergyReportButton.Click += new System.EventHandler(this.EnergyReportButton_Click);
            // 
            // EnergyProgressBar
            // 
            resources.ApplyResources(this.EnergyProgressBar, "EnergyProgressBar");
            this.EnergyProgressBar.Maximum = 70;
            this.EnergyProgressBar.Name = "EnergyProgressBar";
            this.EnergyProgressBar.Step = 1;
            this.EnergyProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.EnergyProgressBar.Value = 23;
            // 
            // PowerReportButton
            // 
            resources.ApplyResources(this.PowerReportButton, "PowerReportButton");
            this.PowerReportButton.BackColor = System.Drawing.SystemColors.Control;
            this.PowerReportButton.Image = global::PowerCFG.Properties.Resources.power_icon;
            this.PowerReportButton.Name = "PowerReportButton";
            this.PowerReportButton.UseVisualStyleBackColor = false;
            this.PowerReportButton.Click += new System.EventHandler(this.PowerReportButton_Click);
            // 
            // PowerReportBackgroundWorker
            // 
            this.PowerReportBackgroundWorker.WorkerReportsProgress = true;
            this.PowerReportBackgroundWorker.WorkerSupportsCancellation = true;
            this.PowerReportBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PowerReportBackgroundWorker_DoWork);
            this.PowerReportBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.PowerReportBackgroundWorker_ProgressChanged);
            this.PowerReportBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PowerReportBackgroundWorker_RunWorkerCompleted);
            // 
            // EnergyReportBackgroundWorker
            // 
            this.EnergyReportBackgroundWorker.WorkerReportsProgress = true;
            this.EnergyReportBackgroundWorker.WorkerSupportsCancellation = true;
            this.EnergyReportBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.EnergyBackgroundWorker_DoWork);
            this.EnergyReportBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.EnergyBackgroundWorker_ProgressChanged);
            this.EnergyReportBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.EnergyBackgroundWorker_RunWorkerCompleted);
            // 
            // PowerProgressBar
            // 
            resources.ApplyResources(this.PowerProgressBar, "PowerProgressBar");
            this.PowerProgressBar.Maximum = 14;
            this.PowerProgressBar.Name = "PowerProgressBar";
            this.PowerProgressBar.Step = 1;
            this.PowerProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PowerProgressBar.Value = 8;
            // 
            // BatteryBackgroundWorker
            // 
            this.BatteryBackgroundWorker.WorkerReportsProgress = true;
            this.BatteryBackgroundWorker.WorkerSupportsCancellation = true;
            this.BatteryBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BatteryBackgroundWorker_DoWork);
            this.BatteryBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BatteryBackgroundWorker_RunWorkerCompleted);
            // 
            // FrmPowerCFG
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PowerProgressBar);
            this.Controls.Add(this.EnergyProgressBar);
            this.Controls.Add(this.EnergyReportButton);
            this.Controls.Add(this.PowerReportButton);
            this.Controls.Add(this.BatteryReportButton);
            this.Controls.Add(this.ExcelExportButton);
            this.Controls.Add(this.ShowGuidsCheckBox);
            this.Controls.Add(this.DropDownEditor);
            this.Controls.Add(this.RangeEditor);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.HiddenCheckBox);
            this.Controls.Add(this.tv);
            this.Name = "FrmPowerCFG";
            this.Load += new System.EventHandler(this.FrmPowerCFG_Load);
            this.Shown += new System.EventHandler(this.FrmPowerCFG_Shown);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TreeView tv;
        public CheckBox HiddenCheckBox;
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
        private Button BatteryReportButton;
        private Button EnergyReportButton;
        private ProgressBar EnergyProgressBar;
        private Button PowerReportButton;
        private System.ComponentModel.BackgroundWorker PowerReportBackgroundWorker;
        private System.ComponentModel.BackgroundWorker EnergyReportBackgroundWorker;
        private ProgressBar PowerProgressBar;
        private System.ComponentModel.BackgroundWorker BatteryBackgroundWorker;
    }
}