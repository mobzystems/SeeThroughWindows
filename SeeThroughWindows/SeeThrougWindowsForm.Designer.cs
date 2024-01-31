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
      components = new System.ComponentModel.Container();
      Label label3;
      Label label4;
      Label label1;
      Label label2;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeeThrougWindowsForm));
      toolStripSeparator1 = new ToolStripSeparator();
      pictureBox1 = new PictureBox();
      notifyIcon = new NotifyIcon(components);
      contextMenuStrip = new ContextMenuStrip(components);
      optionsToolStripMenuItem = new ToolStripMenuItem();
      exitToolStripMenuItem = new ToolStripMenuItem();
      shiftCheckBox = new CheckBox();
      hotKeyComboBox = new ComboBox();
      controlCheckBox = new CheckBox();
      altCheckBox = new CheckBox();
      windowsCheckBox = new CheckBox();
      transparencyTrackBar = new TrackBar();
      previewCheckBox = new CheckBox();
      helpLink = new LinkLabel();
      groupBox1 = new GroupBox();
      restoreAllButton = new Button();
      enableChangeTransparencyCheckbox = new CheckBox();
      topMostCheckBox = new CheckBox();
      clickThroughCheckBox = new CheckBox();
      groupBox2 = new GroupBox();
      sendToMonitorEnabledCheckBox = new CheckBox();
      minMaxEnabledCheckBox = new CheckBox();
      label3 = new Label();
      label4 = new Label();
      label1 = new Label();
      label2 = new Label();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      contextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)transparencyTrackBar).BeginInit();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(110, 104);
      label3.Margin = new Padding(2, 0, 2, 0);
      label3.Name = "label3";
      label3.Size = new Size(68, 15);
      label3.TabIndex = 8;
      label3.Text = "Transparent";
      label3.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // label4
      // 
      label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      label4.AutoSize = true;
      label4.Location = new Point(416, 104);
      label4.Margin = new Padding(2, 0, 2, 0);
      label4.Name = "label4";
      label4.Size = new Size(49, 15);
      label4.TabIndex = 9;
      label4.Text = "Opaque";
      label4.TextAlign = ContentAlignment.MiddleRight;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(55, 24);
      label1.Margin = new Padding(2, 0, 2, 0);
      label1.Name = "label1";
      label1.Size = new Size(48, 15);
      label1.TabIndex = 0;
      label1.Text = "&Hotkey:";
      label1.TextAlign = ContentAlignment.MiddleRight;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(24, 78);
      label2.Margin = new Padding(2, 0, 2, 0);
      label2.Name = "label2";
      label2.Size = new Size(79, 15);
      label2.TabIndex = 6;
      label2.Text = "&Transparency:";
      label2.TextAlign = ContentAlignment.MiddleRight;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(122, 6);
      // 
      // pictureBox1
      // 
      pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
      pictureBox1.Location = new Point(11, 11);
      pictureBox1.Margin = new Padding(2);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(52, 45);
      pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      pictureBox1.TabIndex = 12;
      pictureBox1.TabStop = false;
      // 
      // notifyIcon
      // 
      notifyIcon.ContextMenuStrip = contextMenuStrip;
      notifyIcon.Text = "See Through Windows";
      notifyIcon.Visible = true;
      notifyIcon.MouseUp += notifyIcon_MouseUp;
      // 
      // contextMenuStrip
      // 
      contextMenuStrip.ImageScalingSize = new Size(24, 24);
      contextMenuStrip.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
      contextMenuStrip.Name = "contextMenuStrip";
      contextMenuStrip.Size = new Size(126, 54);
      // 
      // optionsToolStripMenuItem
      // 
      optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      optionsToolStripMenuItem.Size = new Size(125, 22);
      optionsToolStripMenuItem.Text = "Options...";
      optionsToolStripMenuItem.MouseUp += notifyIcon_MouseUp;
      // 
      // exitToolStripMenuItem
      // 
      exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      exitToolStripMenuItem.Size = new Size(125, 22);
      exitToolStripMenuItem.Text = "E&xit";
      exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
      // 
      // shiftCheckBox
      // 
      shiftCheckBox.AutoSize = true;
      shiftCheckBox.Location = new Point(110, 49);
      shiftCheckBox.Margin = new Padding(2);
      shiftCheckBox.Name = "shiftCheckBox";
      shiftCheckBox.Size = new Size(50, 19);
      shiftCheckBox.TabIndex = 2;
      shiftCheckBox.Text = "&Shift";
      shiftCheckBox.UseVisualStyleBackColor = true;
      shiftCheckBox.CheckedChanged += UpdateHotKey;
      // 
      // hotKeyComboBox
      // 
      hotKeyComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      hotKeyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      hotKeyComboBox.FlatStyle = FlatStyle.System;
      hotKeyComboBox.FormattingEnabled = true;
      hotKeyComboBox.Location = new Point(110, 22);
      hotKeyComboBox.Margin = new Padding(2);
      hotKeyComboBox.Name = "hotKeyComboBox";
      hotKeyComboBox.Size = new Size(370, 23);
      hotKeyComboBox.TabIndex = 1;
      hotKeyComboBox.SelectedIndexChanged += UpdateHotKey;
      // 
      // controlCheckBox
      // 
      controlCheckBox.AutoSize = true;
      controlCheckBox.Location = new Point(166, 49);
      controlCheckBox.Margin = new Padding(2);
      controlCheckBox.Name = "controlCheckBox";
      controlCheckBox.Size = new Size(66, 19);
      controlCheckBox.TabIndex = 3;
      controlCheckBox.Text = "&Control";
      controlCheckBox.UseVisualStyleBackColor = true;
      controlCheckBox.CheckedChanged += UpdateHotKey;
      // 
      // altCheckBox
      // 
      altCheckBox.AutoSize = true;
      altCheckBox.Location = new Point(239, 49);
      altCheckBox.Margin = new Padding(2);
      altCheckBox.Name = "altCheckBox";
      altCheckBox.Size = new Size(41, 19);
      altCheckBox.TabIndex = 4;
      altCheckBox.Text = "&Alt";
      altCheckBox.UseVisualStyleBackColor = true;
      altCheckBox.CheckedChanged += UpdateHotKey;
      // 
      // windowsCheckBox
      // 
      windowsCheckBox.AutoSize = true;
      windowsCheckBox.Location = new Point(285, 49);
      windowsCheckBox.Margin = new Padding(2);
      windowsCheckBox.Name = "windowsCheckBox";
      windowsCheckBox.Size = new Size(75, 19);
      windowsCheckBox.TabIndex = 5;
      windowsCheckBox.Text = "&Windows";
      windowsCheckBox.UseVisualStyleBackColor = true;
      windowsCheckBox.CheckedChanged += UpdateHotKey;
      // 
      // transparencyTrackBar
      // 
      transparencyTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      transparencyTrackBar.LargeChange = 32;
      transparencyTrackBar.Location = new Point(110, 78);
      transparencyTrackBar.Margin = new Padding(2);
      transparencyTrackBar.Maximum = 250;
      transparencyTrackBar.Minimum = 10;
      transparencyTrackBar.Name = "transparencyTrackBar";
      transparencyTrackBar.Size = new Size(369, 45);
      transparencyTrackBar.SmallChange = 8;
      transparencyTrackBar.TabIndex = 7;
      transparencyTrackBar.TickFrequency = 16;
      transparencyTrackBar.Value = 10;
      transparencyTrackBar.ValueChanged += transparencyTrackBar_ValueChanged;
      // 
      // previewCheckBox
      // 
      previewCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      previewCheckBox.AutoSize = true;
      previewCheckBox.Location = new Point(4, 210);
      previewCheckBox.Margin = new Padding(2);
      previewCheckBox.Name = "previewCheckBox";
      previewCheckBox.Size = new Size(138, 19);
      previewCheckBox.TabIndex = 13;
      previewCheckBox.Text = "&Preview transparency";
      previewCheckBox.UseVisualStyleBackColor = true;
      previewCheckBox.CheckedChanged += previewCheckBox_CheckedChanged;
      // 
      // helpLink
      // 
      helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      helpLink.Location = new Point(207, 348);
      helpLink.Margin = new Padding(2, 0, 2, 0);
      helpLink.Name = "helpLink";
      helpLink.Size = new Size(354, 15);
      helpLink.TabIndex = 2;
      helpLink.TabStop = true;
      helpLink.Text = "See Through Windows (#3264#-bit) v#V# by MOBZystems";
      helpLink.TextAlign = ContentAlignment.MiddleRight;
      helpLink.VisitedLinkColor = Color.Blue;
      helpLink.LinkClicked += helpLink_LinkClicked;
      // 
      // groupBox1
      // 
      groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      groupBox1.Controls.Add(restoreAllButton);
      groupBox1.Controls.Add(enableChangeTransparencyCheckbox);
      groupBox1.Controls.Add(topMostCheckBox);
      groupBox1.Controls.Add(clickThroughCheckBox);
      groupBox1.Controls.Add(hotKeyComboBox);
      groupBox1.Controls.Add(shiftCheckBox);
      groupBox1.Controls.Add(previewCheckBox);
      groupBox1.Controls.Add(label4);
      groupBox1.Controls.Add(label1);
      groupBox1.Controls.Add(controlCheckBox);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(altCheckBox);
      groupBox1.Controls.Add(windowsCheckBox);
      groupBox1.Controls.Add(label2);
      groupBox1.Controls.Add(transparencyTrackBar);
      groupBox1.Location = new Point(78, 11);
      groupBox1.Margin = new Padding(2);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new Padding(2);
      groupBox1.Size = new Size(483, 256);
      groupBox1.TabIndex = 0;
      groupBox1.TabStop = false;
      groupBox1.Text = "Make window transparent";
      // 
      // restoreAllButton
      // 
      restoreAllButton.Location = new Point(110, 182);
      restoreAllButton.Name = "restoreAllButton";
      restoreAllButton.Size = new Size(158, 23);
      restoreAllButton.TabIndex = 12;
      restoreAllButton.Text = "Restore all windows";
      restoreAllButton.UseVisualStyleBackColor = true;
      restoreAllButton.Click += restoreAllButton_Click;
      // 
      // enableChangeTransparencyCheckbox
      // 
      enableChangeTransparencyCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      enableChangeTransparencyCheckbox.AutoSize = true;
      enableChangeTransparencyCheckbox.Location = new Point(4, 233);
      enableChangeTransparencyCheckbox.Margin = new Padding(2);
      enableChangeTransparencyCheckbox.Name = "enableChangeTransparencyCheckbox";
      enableChangeTransparencyCheckbox.Size = new Size(399, 19);
      enableChangeTransparencyCheckbox.TabIndex = 14;
      enableChangeTransparencyCheckbox.Text = "Enable Control+Windows+PageUp/PageDown to change &transparency";
      enableChangeTransparencyCheckbox.UseVisualStyleBackColor = true;
      enableChangeTransparencyCheckbox.CheckedChanged += enableChangeTransparencyCheckbox_CheckedChanged;
      // 
      // topMostCheckBox
      // 
      topMostCheckBox.AutoSize = true;
      topMostCheckBox.Enabled = false;
      topMostCheckBox.Location = new Point(129, 158);
      topMostCheckBox.Margin = new Padding(2);
      topMostCheckBox.Name = "topMostCheckBox";
      topMostCheckBox.Size = new Size(219, 19);
      topMostCheckBox.TabIndex = 11;
      topMostCheckBox.Text = "... and keep &on top of other windows";
      topMostCheckBox.UseVisualStyleBackColor = true;
      // 
      // clickThroughCheckBox
      // 
      clickThroughCheckBox.AutoSize = true;
      clickThroughCheckBox.Location = new Point(110, 135);
      clickThroughCheckBox.Margin = new Padding(2);
      clickThroughCheckBox.Name = "clickThroughCheckBox";
      clickThroughCheckBox.Size = new Size(183, 19);
      clickThroughCheckBox.TabIndex = 10;
      clickThroughCheckBox.Text = "Make window 'C&lick-through'";
      clickThroughCheckBox.UseVisualStyleBackColor = true;
      clickThroughCheckBox.CheckedChanged += clickThroughCheckBox_CheckedChanged;
      // 
      // groupBox2
      // 
      groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      groupBox2.Controls.Add(sendToMonitorEnabledCheckBox);
      groupBox2.Controls.Add(minMaxEnabledCheckBox);
      groupBox2.Location = new Point(78, 271);
      groupBox2.Margin = new Padding(2);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new Padding(2);
      groupBox2.Size = new Size(483, 67);
      groupBox2.TabIndex = 1;
      groupBox2.TabStop = false;
      groupBox2.Text = "Move window";
      // 
      // sendToMonitorEnabledCheckBox
      // 
      sendToMonitorEnabledCheckBox.AutoSize = true;
      sendToMonitorEnabledCheckBox.Location = new Point(4, 43);
      sendToMonitorEnabledCheckBox.Margin = new Padding(2);
      sendToMonitorEnabledCheckBox.Name = "sendToMonitorEnabledCheckBox";
      sendToMonitorEnabledCheckBox.Size = new Size(410, 19);
      sendToMonitorEnabledCheckBox.TabIndex = 1;
      sendToMonitorEnabledCheckBox.Text = "Enable Control+Windows+Left/Right to &send windows to other monitors";
      sendToMonitorEnabledCheckBox.UseVisualStyleBackColor = true;
      sendToMonitorEnabledCheckBox.CheckedChanged += sendToMonitorEnabledCheckBox_CheckedChanged;
      // 
      // minMaxEnabledCheckBox
      // 
      minMaxEnabledCheckBox.AutoSize = true;
      minMaxEnabledCheckBox.Location = new Point(4, 20);
      minMaxEnabledCheckBox.Margin = new Padding(2);
      minMaxEnabledCheckBox.Name = "minMaxEnabledCheckBox";
      minMaxEnabledCheckBox.Size = new Size(396, 19);
      minMaxEnabledCheckBox.TabIndex = 0;
      minMaxEnabledCheckBox.Text = "Enable Control+Windows+Up/Down to &maximize/mimimize windows";
      minMaxEnabledCheckBox.UseVisualStyleBackColor = true;
      minMaxEnabledCheckBox.CheckedChanged += minMaxEnabledCheckBox_CheckedChanged;
      // 
      // SeeThrougWindowsForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(572, 372);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(helpLink);
      Controls.Add(pictureBox1);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      Icon = (Icon)resources.GetObject("$this.Icon");
      Margin = new Padding(2);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "SeeThrougWindowsForm";
      Text = "See Through Windows Options";
      Shown += SeeThrougWindowsForm_Shown;
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      contextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)transparencyTrackBar).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ResumeLayout(false);
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
    private Button restoreAllButton;
  }
}

