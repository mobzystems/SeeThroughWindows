namespace SeeThroughWindows
{
  partial class SeeThrougWindowsForm
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.Label label3;
      System.Windows.Forms.Label label4;
      System.Windows.Forms.Label label1;
      System.Windows.Forms.Label label2;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeeThrougWindowsForm));
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.shiftCheckBox = new System.Windows.Forms.CheckBox();
      this.hotKeyComboBox = new System.Windows.Forms.ComboBox();
      this.controlCheckBox = new System.Windows.Forms.CheckBox();
      this.altCheckBox = new System.Windows.Forms.CheckBox();
      this.windowsCheckBox = new System.Windows.Forms.CheckBox();
      this.transparencyTrackBar = new System.Windows.Forms.TrackBar();
      this.previewCheckBox = new System.Windows.Forms.CheckBox();
      this.helpLink = new System.Windows.Forms.LinkLabel();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.enableChangeTransparencyCheckbox = new System.Windows.Forms.CheckBox();
      this.topMostCheckBox = new System.Windows.Forms.CheckBox();
      this.clickThroughCheckBox = new System.Windows.Forms.CheckBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.sendToMonitorEnabledCheckBox = new System.Windows.Forms.CheckBox();
      this.minMaxEnabledCheckBox = new System.Windows.Forms.CheckBox();
      label3 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.contextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.transparencyTrackBar)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(110, 104);
      label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(68, 15);
      label3.TabIndex = 8;
      label3.Text = "Transparent";
      label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label4
      // 
      label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(412, 104);
      label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(49, 15);
      label4.TabIndex = 9;
      label4.Text = "Opaque";
      label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(55, 24);
      label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(48, 15);
      label1.TabIndex = 0;
      label1.Text = "&Hotkey:";
      label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(24, 78);
      label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(79, 15);
      label2.TabIndex = 6;
      label2.Text = "&Transparency:";
      label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(122, 6);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(13, 12);
      this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(52, 45);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 12;
      this.pictureBox1.TabStop = false;
      // 
      // notifyIcon
      // 
      this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
      this.notifyIcon.Text = "See Through Windows";
      this.notifyIcon.Visible = true;
      this.notifyIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.Size = new System.Drawing.Size(126, 54);
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
      this.optionsToolStripMenuItem.Text = "Options...";
      this.optionsToolStripMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // shiftCheckBox
      // 
      this.shiftCheckBox.AutoSize = true;
      this.shiftCheckBox.Location = new System.Drawing.Point(110, 45);
      this.shiftCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.shiftCheckBox.Name = "shiftCheckBox";
      this.shiftCheckBox.Size = new System.Drawing.Size(50, 19);
      this.shiftCheckBox.TabIndex = 2;
      this.shiftCheckBox.Text = "&Shift";
      this.shiftCheckBox.UseVisualStyleBackColor = true;
      this.shiftCheckBox.CheckedChanged += new System.EventHandler(this.UpdateHotKey);
      // 
      // hotKeyComboBox
      // 
      this.hotKeyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.hotKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.hotKeyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.hotKeyComboBox.FormattingEnabled = true;
      this.hotKeyComboBox.Location = new System.Drawing.Point(110, 22);
      this.hotKeyComboBox.Margin = new System.Windows.Forms.Padding(2);
      this.hotKeyComboBox.Name = "hotKeyComboBox";
      this.hotKeyComboBox.Size = new System.Drawing.Size(366, 23);
      this.hotKeyComboBox.TabIndex = 1;
      this.hotKeyComboBox.SelectedIndexChanged += new System.EventHandler(this.UpdateHotKey);
      // 
      // controlCheckBox
      // 
      this.controlCheckBox.AutoSize = true;
      this.controlCheckBox.Location = new System.Drawing.Point(166, 45);
      this.controlCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.controlCheckBox.Name = "controlCheckBox";
      this.controlCheckBox.Size = new System.Drawing.Size(66, 19);
      this.controlCheckBox.TabIndex = 3;
      this.controlCheckBox.Text = "&Control";
      this.controlCheckBox.UseVisualStyleBackColor = true;
      this.controlCheckBox.CheckedChanged += new System.EventHandler(this.UpdateHotKey);
      // 
      // altCheckBox
      // 
      this.altCheckBox.AutoSize = true;
      this.altCheckBox.Location = new System.Drawing.Point(239, 45);
      this.altCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.altCheckBox.Name = "altCheckBox";
      this.altCheckBox.Size = new System.Drawing.Size(41, 19);
      this.altCheckBox.TabIndex = 4;
      this.altCheckBox.Text = "&Alt";
      this.altCheckBox.UseVisualStyleBackColor = true;
      this.altCheckBox.CheckedChanged += new System.EventHandler(this.UpdateHotKey);
      // 
      // windowsCheckBox
      // 
      this.windowsCheckBox.AutoSize = true;
      this.windowsCheckBox.Location = new System.Drawing.Point(285, 45);
      this.windowsCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.windowsCheckBox.Name = "windowsCheckBox";
      this.windowsCheckBox.Size = new System.Drawing.Size(75, 19);
      this.windowsCheckBox.TabIndex = 5;
      this.windowsCheckBox.Text = "&Windows";
      this.windowsCheckBox.UseVisualStyleBackColor = true;
      this.windowsCheckBox.CheckedChanged += new System.EventHandler(this.UpdateHotKey);
      // 
      // transparencyTrackBar
      // 
      this.transparencyTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.transparencyTrackBar.LargeChange = 32;
      this.transparencyTrackBar.Location = new System.Drawing.Point(110, 78);
      this.transparencyTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.transparencyTrackBar.Maximum = 250;
      this.transparencyTrackBar.Minimum = 10;
      this.transparencyTrackBar.Name = "transparencyTrackBar";
      this.transparencyTrackBar.Size = new System.Drawing.Size(365, 45);
      this.transparencyTrackBar.SmallChange = 8;
      this.transparencyTrackBar.TabIndex = 7;
      this.transparencyTrackBar.TickFrequency = 16;
      this.transparencyTrackBar.Value = 10;
      this.transparencyTrackBar.ValueChanged += new System.EventHandler(this.transparencyTrackBar_ValueChanged);
      // 
      // previewCheckBox
      // 
      this.previewCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.previewCheckBox.AutoSize = true;
      this.previewCheckBox.Location = new System.Drawing.Point(9, 193);
      this.previewCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.previewCheckBox.Name = "previewCheckBox";
      this.previewCheckBox.Size = new System.Drawing.Size(138, 19);
      this.previewCheckBox.TabIndex = 12;
      this.previewCheckBox.Text = "&Preview transparency";
      this.previewCheckBox.UseVisualStyleBackColor = true;
      this.previewCheckBox.CheckedChanged += new System.EventHandler(this.previewCheckBox_CheckedChanged);
      // 
      // helpLink
      // 
      this.helpLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.helpLink.Location = new System.Drawing.Point(98, 347);
      this.helpLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new System.Drawing.Size(459, 15);
      this.helpLink.TabIndex = 2;
      this.helpLink.TabStop = true;
      this.helpLink.Text = "See Through Windows (#3264#-bit) v#V# by MOBZystems";
      this.helpLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.helpLink.VisitedLinkColor = System.Drawing.Color.Blue;
      this.helpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLink_LinkClicked);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.enableChangeTransparencyCheckbox);
      this.groupBox1.Controls.Add(this.topMostCheckBox);
      this.groupBox1.Controls.Add(this.clickThroughCheckBox);
      this.groupBox1.Controls.Add(this.hotKeyComboBox);
      this.groupBox1.Controls.Add(this.shiftCheckBox);
      this.groupBox1.Controls.Add(this.previewCheckBox);
      this.groupBox1.Controls.Add(label4);
      this.groupBox1.Controls.Add(label1);
      this.groupBox1.Controls.Add(this.controlCheckBox);
      this.groupBox1.Controls.Add(label3);
      this.groupBox1.Controls.Add(this.altCheckBox);
      this.groupBox1.Controls.Add(this.windowsCheckBox);
      this.groupBox1.Controls.Add(label2);
      this.groupBox1.Controls.Add(this.transparencyTrackBar);
      this.groupBox1.Location = new System.Drawing.Point(78, 12);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
      this.groupBox1.Size = new System.Drawing.Size(479, 247);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Make window transparent";
      // 
      // enableChangeTransparencyCheckbox
      // 
      this.enableChangeTransparencyCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.enableChangeTransparencyCheckbox.AutoSize = true;
      this.enableChangeTransparencyCheckbox.Location = new System.Drawing.Point(9, 214);
      this.enableChangeTransparencyCheckbox.Margin = new System.Windows.Forms.Padding(2);
      this.enableChangeTransparencyCheckbox.Name = "enableChangeTransparencyCheckbox";
      this.enableChangeTransparencyCheckbox.Size = new System.Drawing.Size(399, 19);
      this.enableChangeTransparencyCheckbox.TabIndex = 13;
      this.enableChangeTransparencyCheckbox.Text = "Enable Control+Windows+PageUp/PageDown to change &transparency";
      this.enableChangeTransparencyCheckbox.UseVisualStyleBackColor = true;
      this.enableChangeTransparencyCheckbox.CheckedChanged += new System.EventHandler(this.enableChangeTransparencyCheckbox_CheckedChanged);
      // 
      // topMostCheckBox
      // 
      this.topMostCheckBox.AutoSize = true;
      this.topMostCheckBox.Enabled = false;
      this.topMostCheckBox.Location = new System.Drawing.Point(138, 156);
      this.topMostCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.topMostCheckBox.Name = "topMostCheckBox";
      this.topMostCheckBox.Size = new System.Drawing.Size(219, 19);
      this.topMostCheckBox.TabIndex = 11;
      this.topMostCheckBox.Text = "... and keep &on top of other windows";
      this.topMostCheckBox.UseVisualStyleBackColor = true;
      // 
      // clickThroughCheckBox
      // 
      this.clickThroughCheckBox.AutoSize = true;
      this.clickThroughCheckBox.Location = new System.Drawing.Point(110, 135);
      this.clickThroughCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.clickThroughCheckBox.Name = "clickThroughCheckBox";
      this.clickThroughCheckBox.Size = new System.Drawing.Size(183, 19);
      this.clickThroughCheckBox.TabIndex = 10;
      this.clickThroughCheckBox.Text = "Make window \'C&lick-through\'";
      this.clickThroughCheckBox.UseVisualStyleBackColor = true;
      this.clickThroughCheckBox.CheckedChanged += new System.EventHandler(this.clickThroughCheckBox_CheckedChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.sendToMonitorEnabledCheckBox);
      this.groupBox2.Controls.Add(this.minMaxEnabledCheckBox);
      this.groupBox2.Location = new System.Drawing.Point(78, 270);
      this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
      this.groupBox2.Size = new System.Drawing.Size(479, 67);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Move window";
      // 
      // sendToMonitorEnabledCheckBox
      // 
      this.sendToMonitorEnabledCheckBox.AutoSize = true;
      this.sendToMonitorEnabledCheckBox.Location = new System.Drawing.Point(9, 41);
      this.sendToMonitorEnabledCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.sendToMonitorEnabledCheckBox.Name = "sendToMonitorEnabledCheckBox";
      this.sendToMonitorEnabledCheckBox.Size = new System.Drawing.Size(410, 19);
      this.sendToMonitorEnabledCheckBox.TabIndex = 1;
      this.sendToMonitorEnabledCheckBox.Text = "Enable Control+Windows+Left/Right to &send windows to other monitors";
      this.sendToMonitorEnabledCheckBox.UseVisualStyleBackColor = true;
      this.sendToMonitorEnabledCheckBox.CheckedChanged += new System.EventHandler(this.sendToMonitorEnabledCheckBox_CheckedChanged);
      // 
      // minMaxEnabledCheckBox
      // 
      this.minMaxEnabledCheckBox.AutoSize = true;
      this.minMaxEnabledCheckBox.Location = new System.Drawing.Point(9, 20);
      this.minMaxEnabledCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.minMaxEnabledCheckBox.Name = "minMaxEnabledCheckBox";
      this.minMaxEnabledCheckBox.Size = new System.Drawing.Size(396, 19);
      this.minMaxEnabledCheckBox.TabIndex = 0;
      this.minMaxEnabledCheckBox.Text = "Enable Control+Windows+Up/Down to &maximize/mimimize windows";
      this.minMaxEnabledCheckBox.UseVisualStyleBackColor = true;
      this.minMaxEnabledCheckBox.CheckedChanged += new System.EventHandler(this.minMaxEnabledCheckBox_CheckedChanged);
      // 
      // SeeThrougWindowsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(572, 372);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.helpLink);
      this.Controls.Add(this.pictureBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SeeThrougWindowsForm";
      this.Text = "See Through Windows Options";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.contextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.transparencyTrackBar)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.CheckBox shiftCheckBox;
    private System.Windows.Forms.ComboBox hotKeyComboBox;
    private System.Windows.Forms.CheckBox controlCheckBox;
    private System.Windows.Forms.CheckBox altCheckBox;
    private System.Windows.Forms.CheckBox windowsCheckBox;
    private System.Windows.Forms.TrackBar transparencyTrackBar;
    private System.Windows.Forms.CheckBox previewCheckBox;
    private System.Windows.Forms.LinkLabel helpLink;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox sendToMonitorEnabledCheckBox;
    private System.Windows.Forms.CheckBox minMaxEnabledCheckBox;
    private System.Windows.Forms.CheckBox clickThroughCheckBox;
    private System.Windows.Forms.CheckBox topMostCheckBox;
    private System.Windows.Forms.CheckBox enableChangeTransparencyCheckbox;
    private ToolStripSeparator toolStripSeparator1;
    private PictureBox pictureBox1;
  }
}

