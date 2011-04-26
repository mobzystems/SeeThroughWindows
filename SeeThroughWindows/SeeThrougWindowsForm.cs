using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;

namespace SeeThroughWindows
{
  public partial class SeeThrougWindowsForm : Form
  {
    #region WindowInfo class
    // Class to store information about windows we changed
    private class WindowInfo
    {
      public uint Style;
      public short OriginalAlpha;
      public short CurrentAlpha;

      public WindowInfo(uint style, short alpha)
      {
        this.Style = style;
        this.CurrentAlpha = alpha;
        this.OriginalAlpha = alpha;
      }
    }
    #endregion

    // The list of windows we have 'transparenticized'
    private Dictionary<IntPtr, WindowInfo> windows = new Dictionary<IntPtr, WindowInfo>();

    #region Interop Stuff
    private const int GWL_STYLE = -16;
    private const int GWL_EX_STYLE = -20;
    private const int WS_EX_LAYERED = 1;
    private const int WS_EX_TRANSPARENT = 0x80000;

    [DllImport("user32.dll")]
    private static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int transparentColor, short alpha, int action); // true is OK, 0=transparent, 255=opaque

    [DllImport("user32.dll")]
    private static extern bool GetLayeredWindowAttributes(IntPtr hWnd, out int transparentColor, out short alpha, out int action); // true is OK, 0=transparent, 255=opaque

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr BeginDeferWindowPos(int numWindows);

    [DllImport("user32.dll")]
    private static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

    [DllImport("user32.dll")]
    private static extern IntPtr DeferWindowPos(
      IntPtr hWinPosInfo,
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      uint uFlags
    );
    private const int LWA_COLORKEY = 1;
    private const int LWA_ALPHA = 2;

    private const uint WS_MAXIMIZE = 0x01000000;
    private const uint WS_MINIMIZE = 0x20000000;
    private const uint WS_MAXIMIZEBOX = 0x00010000;
    private const uint WS_MINIMIZEBOX = 0x00020000;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
      public uint length;
      public uint flags;
      public uint showCmd;
      public POINT ptMinPosition;
      public POINT ptMaxPosition;
      public RECT rcNormalPosition;
    }

    [DllImport("user32.dll")]
    private static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll")]
    private static extern bool IsZoomed(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nShowCmd);
    [DllImport("user32.dll")]
    private static extern bool SendMessage(IntPtr handle, uint msg, uint wParam, uint lParam);
    [DllImport("user32.dll")]
    private static extern bool InvalidateRect(IntPtr handle, IntPtr lpRect, bool bErase);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(
         int hWnd,            // window handle
         int hWndInsertAfter, // placement-order handle
         int X,               // horizontal position
         int Y,               // vertical position
         int cx,              // width
         int cy,              // height
         uint uFlags);        // window positioning flags    
    [DllImport("user32.dll")]
    static extern bool SetWindowPlacement(
      IntPtr hWnd,
      ref WINDOWPLACEMENT lpwndpl
    );
    [DllImport("user32.dll")]
    static extern bool GetWindowPlacement(
      IntPtr hWnd,
      ref WINDOWPLACEMENT lpwndpl
    );

    const int SW_HIDE = 0;
    const int SW_SHOWNORMAL = 1;
    const int SW_SHOWMINIMIZED = 2;
    const int SW_SHOWMAXIMIZED = 3;
    const int SW_RESTORE = 9;

    #endregion

    // Constant for opaque transparency
    private const short OPAQUE = 255;
    // Default value for transparency
    private const short DEFAULT_SEMITRANSPARENT = 64;
    // The registry root we're using to save and load settings
    private const string REGROOT = "Software\\MOBZystems\\MOBZXRay\\";

    // The style to apply to transparenticized windows
    private const uint NEW_STYLE = (WS_EX_LAYERED | WS_EX_TRANSPARENT);

    // Current hotkey for transparency
    protected MOBZystems.Hotkey userHotkey = null;
    // Current transparency value
    private short semiTransparentValue = DEFAULT_SEMITRANSPARENT;

    // Maximize hotkey: Windows+Up
    protected MOBZystems.Hotkey maximizeHotkey = new MOBZystems.Hotkey(Keys.Up, false, false, false, true);
    // Minimize hotkey: Windows+Down
    protected MOBZystems.Hotkey minimizeHotkey = new MOBZystems.Hotkey(Keys.Down, false, false, false, true);
    // Previous monitor hotkey: Shift+Windows+Left
    protected MOBZystems.Hotkey previousScreenHotkey = new MOBZystems.Hotkey(Keys.Left, false, false, false, true);
    // Next monitor hotkey: Shift+Windows+Right
    protected MOBZystems.Hotkey nextScreenHotkey = new MOBZystems.Hotkey(Keys.Right, false, false, false, true);

    // Flag indicating we're really closing the application
    private bool exitingApplication = false;

    // Flag indicating we're still loading
    private bool loading = false;

    /// <summary>
    /// Constructor
    /// </summary>
    public SeeThrougWindowsForm()
    {
      this.loading = true;

      InitializeComponent();

      Version version = new Version(Application.ProductVersion);
      this.helpLink.Text = this.helpLink.Text.Replace("#", string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build));

      // Use same icon for notify icon as for this form
      this.notifyIcon.Icon = this.Icon;

      // Read our settings from the registry:
      RegistryKey root = Registry.CurrentUser.CreateSubKey(REGROOT);

      // First transparency:
      this.semiTransparentValue = short.Parse((string)root.GetValue("Transparency", DEFAULT_SEMITRANSPARENT.ToString()));
      if (this.semiTransparentValue < this.transparencyTrackBar.Minimum)
        this.semiTransparentValue = (short)this.transparencyTrackBar.Minimum;
      if (this.semiTransparentValue > this.transparencyTrackBar.Maximum)
        this.semiTransparentValue = (short)this.transparencyTrackBar.Maximum;

      // Then the hot key:
      string hotkeyString = (string)root.GetValue("Hotkey", "Z");
      bool shiftKey = BoolFromString((string)root.GetValue("Shift", "1"));
      bool controlKey = BoolFromString((string)root.GetValue("Control", "1"));
      bool altKey = BoolFromString((string)root.GetValue("Alt", "0"));
      bool windowsKey = BoolFromString((string)root.GetValue("Windows", "0"));

      this.sendToMonitorEnabledCheckBox.Checked = BoolFromString((string)root.GetValue("EnableLeftRight", "1"));
      this.minMaxEnabledCheckBox.Checked = BoolFromString((string)root.GetValue("EnableUpDown", "1"));

      // Create a hot key with the settings:
      this.userHotkey = new MOBZystems.Hotkey(MOBZystems.Hotkey.KeyCodeFromString(hotkeyString), shiftKey, controlKey, altKey, windowsKey);
      // Catch the 'Pressed' event
      this.userHotkey.Pressed += new HandledEventHandler(UserHotkey_Pressed);
      this.userHotkey.Register(this);

      this.minimizeHotkey.Pressed += new HandledEventHandler(MinimizeHotkey_Pressed);
      if (this.minMaxEnabledCheckBox.Checked)
        this.minimizeHotkey.Register(this);

      this.maximizeHotkey.Pressed += new HandledEventHandler(MaximizeHotkey_Pressed);
      if (this.minMaxEnabledCheckBox.Checked)
        this.maximizeHotkey.Register(this);

      this.previousScreenHotkey.Pressed += new HandledEventHandler(PreviousScreenHotkey_Pressed);
      if (this.sendToMonitorEnabledCheckBox.Checked)
        this.previousScreenHotkey.Register(this);

      this.nextScreenHotkey.Pressed += new HandledEventHandler(NextScreenHotkey_Pressed);
      if (this.sendToMonitorEnabledCheckBox.Checked)
        this.nextScreenHotkey.Register(this);

      // Set up our form:
      for (Keys k = Keys.A; k <= Keys.Z; k++)
      {
        AddKeyToComboBox(k);
      }
      for (Keys k = Keys.F1; k <= Keys.F24; k++)
      {
        AddKeyToComboBox(k);
      }

      //AddKeyToComboBox(Keys.Up);
      //AddKeyToComboBox(Keys.Down);
      //AddKeyToComboBox(Keys.Left);
      //AddKeyToComboBox(Keys.Right);
      //AddKeyToComboBox(Keys.Home);
      //AddKeyToComboBox(Keys.End);
      //AddKeyToComboBox(Keys.PageDown);
      //AddKeyToComboBox(Keys.PageUp);

      // Update the form now controls
      this.shiftCheckBox.Checked = this.userHotkey.Shift;
      this.controlCheckBox.Checked = this.userHotkey.Control;
      this.altCheckBox.Checked = this.userHotkey.Alt;
      this.windowsCheckBox.Checked = this.userHotkey.Windows;

      this.transparencyTrackBar.Value = this.semiTransparentValue;

      this.loading = false;
    }

    protected void AddKeyToComboBox(Keys k)
    {
      int index = this.hotKeyComboBox.Items.Add(k);
      if (k == this.userHotkey.KeyCode)
        this.hotKeyComboBox.SelectedItem = k;
    }

    #region Overridden methods

    /// <summary>
    /// Make sure we hide when the user wants to close,
    /// except when this.closing = true
    /// </summary>
    /// <param name="e"></param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      base.OnFormClosing(e);

      if (e.CloseReason == CloseReason.UserClosing && !this.exitingApplication)
      {
        // No - save settings and hide ourselves
        SaveSettings();

        e.Cancel = true;
        Hide();
      }
    }

    /// <summary>
    /// Clean up after ourselves when we really do close
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);

      // Restore all windows
      foreach (IntPtr handle in this.windows.Keys)
      {
        WindowInfo w = this.windows[handle];
        try
        {
          SetLayeredWindowAttributes(handle, 0, w.OriginalAlpha, LWA_ALPHA);
        }
        catch { }
        try
        {
          RestoreStyle(handle, w.Style);
        }
        catch { }
      }

      // Unregister the hotkey:
      if (this.userHotkey.IsRegistered)
        this.userHotkey.Unregister();
    }
    #endregion

    #region Event handlers
    /// <summary>
    /// The hotkey was pressed! Handle it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UserHotkey_Pressed(object sender, HandledEventArgs e)
    {
      IntPtr activeWindowHandle = GetForegroundWindow();

      if (activeWindowHandle != IntPtr.Zero)
      {
        if (activeWindowHandle == GetShellWindow())
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Cannot make desktop transparent", ToolTipIcon.Warning);
          return;
        }

        // Do we know this window?
        WindowInfo window = null;
        if (windows.ContainsKey(activeWindowHandle))
          window = windows[activeWindowHandle];

        if (window == null)
        {
          // See if we need to make the window transparent
          uint originalStyle = GetWindowLong(activeWindowHandle, GWL_EX_STYLE);
          short originalAlpha = OPAQUE;

          if ((originalStyle & NEW_STYLE) != NEW_STYLE)
          {
            SetWindowLong(activeWindowHandle, GWL_EX_STYLE, originalStyle | NEW_STYLE);
          }
          else
          {
            int transparentColor;
            int action;

            if (!GetLayeredWindowAttributes(activeWindowHandle, out transparentColor, out originalAlpha, out action))
              originalAlpha = OPAQUE;
          }
          window = new WindowInfo(originalStyle, originalAlpha);
          windows[activeWindowHandle] = window;
        }

        short newAlpha = window.CurrentAlpha;

        if (newAlpha == OPAQUE) // Opaque
          newAlpha = this.semiTransparentValue;
        else
          newAlpha = OPAQUE;

        if (!SetLayeredWindowAttributes(activeWindowHandle, 0, newAlpha, LWA_ALPHA))
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Could not set transparency on this window", ToolTipIcon.Error);
          // MessageBox.Show(this, "Could not set transparency on window " + activeWindowHandle.ToString(), "SeeThroughWindows", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          window.CurrentAlpha = newAlpha;
        }

        // If the window is not transparent anymore, we're not interested anymore
        if (newAlpha == OPAQUE)
        {
          RestoreStyle(activeWindowHandle, window.Style);
          windows.Remove(activeWindowHandle);
        }

        e.Handled = true;
      }
    }

    void MinimizeHotkey_Pressed(object sender, HandledEventArgs e)
    {
      IntPtr activeWindowHandle = GetForegroundWindow();

      if (activeWindowHandle != IntPtr.Zero)
      {
        if (activeWindowHandle == GetShellWindow())
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Cannot minimize desktop", ToolTipIcon.Warning);
          return;
        }

        uint style = GetWindowLong(activeWindowHandle, GWL_STYLE);

        IntPtr deferInfo = BeginDeferWindowPos(1);

        if (IsZoomed(activeWindowHandle))
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
        }
        else if (IsIconic(activeWindowHandle))
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
          if ((style & WS_MAXIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMAXIMIZED);
          }
        }
        else
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
          if ((style & WS_MINIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMINIMIZED);
          }
          else if ((style & WS_MAXIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMAXIMIZED);
          }
        }
        SetForegroundWindow(activeWindowHandle);

        EndDeferWindowPos(deferInfo);

        e.Handled = true;
      }
    }

    void MaximizeHotkey_Pressed(object sender, HandledEventArgs e)
    {
      IntPtr activeWindowHandle = GetForegroundWindow();

      if (activeWindowHandle != IntPtr.Zero)
      {
        if (activeWindowHandle == GetShellWindow())
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Cannot maximize desktop", ToolTipIcon.Warning);
          return;
        }

        uint style = GetWindowLong(activeWindowHandle, GWL_STYLE);

        IntPtr deferInfo = BeginDeferWindowPos(1);

        if (IsIconic(activeWindowHandle))
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
        }
        else if (IsZoomed(activeWindowHandle))
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
          if ((style & WS_MINIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMINIMIZED);
          }
        }
        else
        {
          ShowWindow(activeWindowHandle, SW_RESTORE);
          if ((style & WS_MAXIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMAXIMIZED);
          }
          else if ((style & WS_MINIMIZEBOX) != 0)
          {
            ShowWindow(activeWindowHandle, SW_SHOWMINIMIZED);
          }
        }
        SetForegroundWindow(activeWindowHandle);

        EndDeferWindowPos(deferInfo);

        e.Handled = true;
      }
    }

    void PreviousScreenHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = MoveActiveWindowToScreen(-1);
    }

    void NextScreenHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = MoveActiveWindowToScreen(+1);
    }

    protected bool MoveActiveWindowToScreen(int offset)
    {
      IntPtr activeWindowHandle = GetForegroundWindow();

      if (activeWindowHandle != IntPtr.Zero)
      {
        if (activeWindowHandle == GetShellWindow())
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Cannot send desktop to other monitor", ToolTipIcon.Warning);
          return false;
        }

        RECT r = new RECT();
        GetWindowRect(activeWindowHandle, out r);

        Screen s = Screen.FromRectangle(new Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top));
        int screens = Screen.AllScreens.Length;
        Screen s2 = Screen.AllScreens[(Array.IndexOf(Screen.AllScreens, s) + screens + offset) % screens];

        if (s != s2)
        {
          WINDOWPLACEMENT wpl = new WINDOWPLACEMENT();
          wpl.length = (uint)Marshal.SizeOf(wpl);
          GetWindowPlacement(activeWindowHandle, ref wpl);

          if (wpl.showCmd != SW_SHOWMINIMIZED)
          {
            // IntPtr deferInfo = BeginDeferWindowPos(1);

            bool maximized = wpl.showCmd == SW_SHOWMAXIMIZED; // IsZoomed(activeWindowHandle);

            if (maximized)
            {
              //// ShowWindow(activeWindowHandle, SW_HIDE);
              //uint style = GetWindowLong(activeWindowHandle, GWL_STYLE);
              //style &= ~WS_MAXIMIZE;
              //SetWindowLong(activeWindowHandle, GWL_STYLE, style);

              //// wpl.showCmd = SW_RESTORE;
              //// SendMessage(activeWindowHandle, 0xB, 0, 0);
              //// wpl.showCmd = SW_RESTORE;
              //// SetWindowPlacement(activeWindowHandle, ref wpl);

              ShowWindow(activeWindowHandle, SW_RESTORE);
              // GetWindowRect(activeWindowHandle, out r);
            }
            //wpl.ptMinPosition.X += s2.WorkingArea.Left - s.WorkingArea.Left;
            //wpl.ptMinPosition.Y += s2.WorkingArea.Top - s.WorkingArea.Top;

            // wpl.ptMaxPosition.X += s2.WorkingArea.Left - s.WorkingArea.Left;
            // wpl.ptMaxPosition.Y += s2.WorkingArea.Top - s.WorkingArea.Top;

            wpl.rcNormalPosition.Left += s2.WorkingArea.Left - s.WorkingArea.Left;
            wpl.rcNormalPosition.Top += s2.WorkingArea.Top - s.WorkingArea.Top;
            wpl.rcNormalPosition.Right += s2.WorkingArea.Left - s.WorkingArea.Left;
            wpl.rcNormalPosition.Bottom += s2.WorkingArea.Top - s.WorkingArea.Top;

            if (maximized)
            //{
            //  // ShowWindow(activeWindowHandle, SW_RESTORE);
            //  // ShowWindow(activeWindowHandle, SW_HIDE);

            //  deferInfo = DeferWindowPos(deferInfo, activeWindowHandle, IntPtr.Zero, wpl.rcNormalPosition.Left, wpl.rcNormalPosition.Top, wpl.rcNormalPosition.Right - wpl.rcNormalPosition.Left, wpl.rcNormalPosition.Bottom - wpl.rcNormalPosition.Top, 0x4);
            //  if (deferInfo == IntPtr.Zero)
            //  {
            //    this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Failed to move window", ToolTipIcon.Warning);
            //  }
            //  else
            //  {
            //    deferInfo = DeferWindowPos(deferInfo, activeWindowHandle, IntPtr.Zero, s2.WorkingArea.Left, s2.WorkingArea.Top, s2.WorkingArea.Width, s2.WorkingArea.Height, 0x4);

            //    uint style = GetWindowLong(activeWindowHandle, GWL_STYLE);
            //    style |= WS_MAXIMIZE;
            //    SetWindowLong(activeWindowHandle, GWL_STYLE, style);
            //  }
            //}
            //else
            {
              if (SetWindowPlacement(activeWindowHandle, ref wpl) == false)
              //if (MoveWindow(activeWindowHandle,
              //  wpl.rcNormalPosition.Left,
              //  wpl.rcNormalPosition.Top,
              //  wpl.rcNormalPosition.Right - wpl.rcNormalPosition.Left,
              //  wpl.rcNormalPosition.Bottom - wpl.rcNormalPosition.Top,                
              //  true) == false)
              {
                this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Failed to place window", ToolTipIcon.Warning);
              }
            }
            else
            {
              if (MoveWindow(activeWindowHandle,
                r.Left - s.WorkingArea.Left + s2.WorkingArea.Left,
                r.Top - s.WorkingArea.Top + s2.WorkingArea.Top,
                (int)((r.Right - r.Left) * ((double)s2.WorkingArea.Width / s.WorkingArea.Width)),
                (int)((r.Bottom - r.Top) * ((double)s2.WorkingArea.Height / s.WorkingArea.Height)),
                true
              ) == false)
              {
                this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Failed to move window", ToolTipIcon.Warning);
              }

            }

            //if (deferInfo != IntPtr.Zero)
            //  EndDeferWindowPos(deferInfo);

            if (maximized)
            {
              // wpl.showCmd = SW_SHOWMAXIMIZED;
              // SetWindowPlacement(activeWindowHandle, ref wpl);
              ShowWindow(activeWindowHandle, SW_SHOWMAXIMIZED);
              //  //SendMessage(activeWindowHandle, 0xB, 1, 0);
              //  //InvalidateRect(activeWindowHandle, IntPtr.Zero, true);
            }
          }
        }

        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Update the hotkey settings. Event handler for the key code and check boxes!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateHotKey(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      this.userHotkey.Reregister((Keys)hotKeyComboBox.SelectedItem, shiftCheckBox.Checked, controlCheckBox.Checked, altCheckBox.Checked, windowsCheckBox.Checked);
    }

    /// <summary>
    /// Update the transparency when the track bar changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void transparencyTrackBar_ValueChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      UpdateTransparency();

      if (previewCheckBox.Checked)
        ApplyTransparency();
    }

    /// <summary>
    /// Preview the current transparency, or reset it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void previewCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      if (previewCheckBox.Checked)
        ApplyTransparency();
      else
        this.Opacity = 1.0;
    }

    /// <summary>
    /// Surf to the web site
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void helpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        Process.Start("http://www.mobzystems.com/Tools/SeeThroughWindows.aspx");
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Show ourselves when the notify icon was clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void notifyIcon_DoubleClick(object sender, EventArgs e)
    {
      if (!this.Visible)
        Show();

      Activate();
    }

    /// <summary>
    /// Really close the form from the exit menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.exitingApplication = true;
      Close();

      Application.Exit();
    }
    #endregion

    #region Conversion helpers
    private string BoolToString(bool b)
    {
      if (b)
        return "1";
      else
        return "0";
    }

    private bool BoolFromString(string s)
    {
      if (s == null || s != "1")
        return false;
      else
        return true;
    }
    #endregion

    /// <summary>
    /// Restore the window style based on the original style
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="originalStyle"></param>
    private void RestoreStyle(IntPtr handle, uint originalStyle)
    {
      uint style = GetWindowLong(handle, GWL_EX_STYLE);

      SetWindowLong(handle, GWL_EX_STYLE, (style & ~NEW_STYLE) | (originalStyle & NEW_STYLE));
    }

    /// <summary>
    /// Update the value of the transparency 
    /// </summary>
    private void UpdateTransparency()
    {
      this.semiTransparentValue = (short)transparencyTrackBar.Value;
    }

    /// <summary>
    /// Apply the transparency to ourselves
    /// </summary>
    private void ApplyTransparency()
    {
      this.Opacity = ((double)this.semiTransparentValue) / 255;
    }

    /// <summary>
    /// Save the settings to the registry
    /// </summary>
    private void SaveSettings()
    {
      RegistryKey root = Registry.CurrentUser.CreateSubKey(REGROOT);

      root.SetValue("Transparency", this.semiTransparentValue.ToString());
      root.SetValue("Hotkey", MOBZystems.Hotkey.KeyCodeToString(this.userHotkey.KeyCode));
      root.SetValue("Shift", BoolToString(this.userHotkey.Shift));
      root.SetValue("Control", BoolToString(this.userHotkey.Control));
      root.SetValue("Alt", BoolToString(this.userHotkey.Alt));
      root.SetValue("Windows", BoolToString(this.userHotkey.Windows));

      root.SetValue("EnableUpDown", BoolToString(this.minMaxEnabledCheckBox.Checked));
      root.SetValue("EnableLeftRight", BoolToString(this.sendToMonitorEnabledCheckBox.Checked));
    }

    private void minMaxEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      if (minMaxEnabledCheckBox.Checked)
      {
        if (!this.minimizeHotkey.IsRegistered)
          this.minimizeHotkey.Register(this);
        if (!this.maximizeHotkey.IsRegistered)
          this.maximizeHotkey.Register(this);
      }
      else
      {
        if (this.minimizeHotkey.IsRegistered)
          this.minimizeHotkey.Unregister();
        if (this.maximizeHotkey.IsRegistered)
          this.maximizeHotkey.Unregister();
      }
    }

    private void sendToMonitorEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      if (sendToMonitorEnabledCheckBox.Checked)
      {
        if (!this.previousScreenHotkey.IsRegistered)
          this.previousScreenHotkey.Register(this);
        if (!this.nextScreenHotkey.IsRegistered)
          this.nextScreenHotkey.Register(this);
      }
      else
      {
        if (this.previousScreenHotkey.IsRegistered)
          this.previousScreenHotkey.Unregister();
        if (this.nextScreenHotkey.IsRegistered)
          this.nextScreenHotkey.Unregister();
      }
    }
  }
}