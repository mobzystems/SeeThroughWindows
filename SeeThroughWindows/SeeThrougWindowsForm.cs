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

    #region Interop Stuff
    private const int GWL_STYLE = -16;
    private const int GWL_EX_STYLE = -20;

    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const int WS_EX_TOPMOST = 0x8; // Set using SetWindowPos!

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
         IntPtr hWnd,            // window handle
         IntPtr hWndInsertAfter, // placement-order handle
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

    const int HWND_BOTTOM = 1; // Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
    const int HWND_NOTOPMOST = -2; // Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
    const int HWND_TOP = 0; // Places the window at the top of the Z order.
    const int HWND_TOPMOST = -1; // Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.

    const int SW_HIDE = 0;
    const int SW_SHOWNORMAL = 1;
    const int SW_SHOWMINIMIZED = 2;
    const int SW_SHOWMAXIMIZED = 3;
    const int SW_RESTORE = 9;

    const uint SWP_NOMOVE = 0x0002; // Retains the current position (ignores X and Y parameters).
    const uint SWP_NOSIZE = 0x0001; // Retains the current size (ignores the cx and cy parameters).
    const uint SWP_NOZORDER = 0x0004; // Retains the current Z order (ignores the hWndInsertAfter parameter).

    #endregion

    #region Constants
    // Constant for opaque transparency
    private const short OPAQUE = 255;
    // Default value for transparency
    private const short DEFAULT_SEMITRANSPARENT = 64;
    // The registry root we're using to save and load settings.
    // This should be See Thorugh Windows, but MOBZXRay was the previous name of STW
    private const string REGROOT = "Software\\MOBZystems\\MOBZXRay\\";

    // The style to apply to transparenticized windows
    private const uint NEW_STYLE_TRANSPARENT = (WS_EX_LAYERED);
    private const uint NEW_STYLE_CLICKTHROUGH = (WS_EX_LAYERED | WS_EX_TRANSPARENT);
    private const uint NEW_STYLE_CLICKTHROUGH_TOPMOST = (WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOPMOST);
    private const uint NEW_STYLE_ALL = (WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOPMOST);
    #endregion

    #region Data members
    // The list of windows we have hijacked (e.g. 'transparenticized')
    private Dictionary<IntPtr, WindowInfo> hijackedWindows = new Dictionary<IntPtr, WindowInfo>();

    // Current hotkey for transparency
    protected MOBZystems.Hotkey userHotkey = null;
    // Current transparency value
    private short semiTransparentValue = DEFAULT_SEMITRANSPARENT;

    // Maximize hotkey: Control+Windows+Up
    protected MOBZystems.Hotkey maximizeHotkey = new MOBZystems.Hotkey(Keys.Up, false, true, false, true);
    // Minimize hotkey: Control+Windows+Down
    protected MOBZystems.Hotkey minimizeHotkey = new MOBZystems.Hotkey(Keys.Down, false, true, false, true);
    // Previous monitor hotkey: Control+Windows+Left
    protected MOBZystems.Hotkey previousScreenHotkey = new MOBZystems.Hotkey(Keys.Left, false, true, false, true);
    // Next monitor hotkey: Control+Windows+Right
    protected MOBZystems.Hotkey nextScreenHotkey = new MOBZystems.Hotkey(Keys.Right, false, true, false, true);

    // More transparent hotkey: Control+Windows+PageDn
    protected MOBZystems.Hotkey moreTransparentHotkey = new MOBZystems.Hotkey(Keys.PageDown, false, true, false, true);
    // Less transparent hotkey: Control+Windows+PageUp
    protected MOBZystems.Hotkey lessTransparentHotkey = new MOBZystems.Hotkey(Keys.PageUp, false, true, false, true);

    // Flag indicating we're really closing the application
    private bool exitingApplication = false;

    // Flag indicating we're still loading
    private bool loading = false;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    public SeeThrougWindowsForm()
    {
      this.loading = true;

      InitializeComponent();

      Version version = new Version(Application.ProductVersion);
      this.helpLink.Text = this.helpLink.Text
        .Replace("#V#", string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build))
        .Replace("#3264#", (IntPtr.Size * 8).ToString());

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

      // Click-through-ness:
      this.clickThroughCheckBox.Checked = BoolFromString((string)root.GetValue("ClickThrough", "0"));
      this.topMostCheckBox.Checked = BoolFromString((string)root.GetValue("TopMost", "1"));

      // Then the hot key:
      string hotkeyString = (string)root.GetValue("Hotkey", "Z");
      bool shiftKey = BoolFromString((string)root.GetValue("Shift", "1"));
      bool controlKey = BoolFromString((string)root.GetValue("Control", "1"));
      bool altKey = BoolFromString((string)root.GetValue("Alt", "0"));
      bool windowsKey = BoolFromString((string)root.GetValue("Windows", "0"));

      this.sendToMonitorEnabledCheckBox.Checked = BoolFromString((string)root.GetValue("EnableLeftRight", "0"));
      this.minMaxEnabledCheckBox.Checked = BoolFromString((string)root.GetValue("EnableUpDown", "0"));
      this.enableChangeTransparencyCheckbox.Checked = BoolFromString((string)root.GetValue("EnablePageUpDown", "1"));

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

      this.moreTransparentHotkey.Pressed += new HandledEventHandler(MoreTransparentHotkey_Pressed);
      if (this.enableChangeTransparencyCheckbox.Checked)
        this.moreTransparentHotkey.Register(this);

      this.lessTransparentHotkey.Pressed += new HandledEventHandler(LessTransparentHotkey_Pressed);
      if (this.enableChangeTransparencyCheckbox.Checked)
        this.lessTransparentHotkey.Register(this);

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

      this.UpdateUI();

      // Done loading
      this.loading = false;
    }

    protected void AddKeyToComboBox(Keys k)
    {
      int index = this.hotKeyComboBox.Items.Add(k);
      if (k == this.userHotkey.KeyCode)
        this.hotKeyComboBox.SelectedItem = k;
    }
    #endregion

    #region Overridden methods

    private void SeeThrougWindowsForm_Shown(object sender, EventArgs e)
    {
      // Make sure we hide ourselves when started minimized
      if (this.WindowState == FormWindowState.Minimized)
      {
        // This causes a breif flash but swapping WindowState and Hide causes the form
        // to stay minimized and to never become Normal
        this.WindowState = FormWindowState.Normal;
        this.Hide();
      }
    }

    /// <summary>
    /// Make sure we hide when the user wants to close,
    /// except when this.exitingApplication
    /// </summary>
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

      RestoreAllWindows();

      // Unregister the hotkey:
      if (this.userHotkey.IsRegistered)
        this.userHotkey.Unregister();
    }

    private void RestoreAllWindows()
    {
      // Restore all windows, ignoring exceptions. If there's an error,
      // we can't really do anything about it now
      foreach (IntPtr handle in this.hijackedWindows.Keys)
      {
        WindowInfo w = this.hijackedWindows[handle];
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

      // We have no more hijacked windows now
      this.hijackedWindows.Clear();
    }
    #endregion

    #region Hotkey Event handlers

    /// <summary>
    /// The hotkey was pressed! Handle it
    /// </summary>
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
        if (this.hijackedWindows.ContainsKey(activeWindowHandle))
          window = this.hijackedWindows[activeWindowHandle];
        else
          window = HijackWindow(activeWindowHandle);

        short newAlpha = window.CurrentAlpha;

        // Toggle the alpha value betwee OPAQUE and semitransparent
        if (newAlpha == OPAQUE)
          newAlpha = this.semiTransparentValue;
        else
          newAlpha = OPAQUE;

        if (!SetLayeredWindowAttributes(activeWindowHandle, 0, newAlpha, LWA_ALPHA))
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Could not set transparency on this window", ToolTipIcon.Error);
        }
        else
        {
          window.CurrentAlpha = newAlpha;

          // Also make window topmost if specified
          if (this.clickThroughCheckBox.Checked && this.topMostCheckBox.Checked)
            SetWindowPos(activeWindowHandle, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        // If the window is not transparent anymore, we're not interested anymore
        if (newAlpha == OPAQUE)
        {
          // Restore the old window style
          RestoreStyle(activeWindowHandle, window.Style);
          // Let go of the window info
          this.hijackedWindows.Remove(activeWindowHandle);
        }

        e.Handled = true;

        this.UpdateUI();
      }
    }

    /// <summary>
    /// Minimize the current window
    /// </summary>
    private void MinimizeHotkey_Pressed(object sender, HandledEventArgs e)
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

    /// <summary>
    /// Maximize the current window
    /// </summary>
    private void MaximizeHotkey_Pressed(object sender, HandledEventArgs e)
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

    /// <summary>
    /// Move the active window to the previous screen
    /// </summary>
    private void PreviousScreenHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = MoveActiveWindowToScreen(-1);
    }

    /// <summary>
    /// Move the active window to the next screen
    /// </summary>
    private void NextScreenHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = MoveActiveWindowToScreen(+1);
    }

    /// <summary>
    /// Make the active window more transparent
    /// </summary>
    private void MoreTransparentHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = ChangeAlpha(GetForegroundWindow(), -16);
    }

    /// <summary>
    /// Make the active window less transparent
    /// </summary>
    private void LessTransparentHotkey_Pressed(object sender, HandledEventArgs e)
    {
      e.Handled = ChangeAlpha(GetForegroundWindow(), +16);
    }
    #endregion

    #region Implementation
    private void UpdateUI()
    {
      this.restoreAllButton.Enabled = this.hijackedWindows.Any();
    }

    /// <summary>
    /// Change the alpha value of a window handle.
    /// 
    /// If the window has not been 'transparenticized', it will be
    /// </summary>
    /// <param name="windowHandle">The window handle</param>
    /// <param name="change">The change in the alpha value</param>
    /// <returns>true if successful</returns>
    protected bool ChangeAlpha(IntPtr windowHandle, short change)
    {
      if (windowHandle == IntPtr.Zero)
        return false;

      // Do we know this window?
      WindowInfo window = null;
      if (!this.hijackedWindows.ContainsKey(windowHandle))
      {
        // No - hijack it
        window = HijackWindow(windowHandle);
        // Start with the default transparency value
        if (!SetLayeredWindowAttributes(windowHandle, 0, this.semiTransparentValue, LWA_ALPHA))
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Could not set transparency on this window", ToolTipIcon.Error);
        }
      }
      else
        // Yes, get the window information
        window = this.hijackedWindows[windowHandle];

      // Get the alpha value of the window
      int transparentColor;
      int action;
      short originalAlpha;

      if (!GetLayeredWindowAttributes(windowHandle, out transparentColor, out originalAlpha, out action))
      {
        this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Could not get transparency of this window", ToolTipIcon.Error);
        return false;
      }
      else
      {
        // Treat 255 as 256 for a nice decrease by step 16
        if (originalAlpha == 255) originalAlpha = 256;
        // Update the alpha value
        short newAlpha = (short)(originalAlpha + change);
        // Maximinze/minimize to 0-255
        if (newAlpha > 255) newAlpha = 255; else if (newAlpha < 0) newAlpha = 0;

        // Try to apply new alpha
        if (!SetLayeredWindowAttributes(windowHandle, 0, newAlpha, LWA_ALPHA))
        {
          this.notifyIcon.ShowBalloonTip(3000, "See Through Windows", "Could not set transparency on this window", ToolTipIcon.Error);
          return false;
        }
        else
        {
          // Update the aplha value. Unused so far but hey
          window.CurrentAlpha = newAlpha;
          return true;
        }
      }
    }

    /// <summary>
    /// Move the active window to the next or previous screen
    /// </summary>
    /// <param name="offset">+1: next screen, -1: previous screen</param>
    /// <returns>true if successful</returns>
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
    /// Hijack (e.g. transparenticize) a window. Create a WindowInfo for it,
    /// set window style etc. based on current settings
    /// </summary>
    /// <param name="windowHandle">The window handle to hijack</param>
    /// <returns>The WindowInfo object for the window handle</returns>
    private WindowInfo HijackWindow(IntPtr windowHandle)
    {
      // If the window is already hijacked, simply return the existing WindowInfo object
      if (this.hijackedWindows.ContainsKey(windowHandle))
        return this.hijackedWindows[windowHandle];

      // See if we need to make the window transparent
      uint originalStyle = GetWindowLong(windowHandle, GWL_EX_STYLE);

      // This is the style we need
      uint style = this.clickThroughCheckBox.Checked ? NEW_STYLE_CLICKTHROUGH : NEW_STYLE_TRANSPARENT;

      // Obtain the original alpha value of the window
      short originalAlpha;

      // Does it have all the necessary styles?
      if ((originalStyle & style) != style)
      {
        // No: set them and assume the window is opaque
        SetWindowLong(windowHandle, GWL_EX_STYLE, originalStyle | style);
        originalAlpha = OPAQUE;
      }
      else
      {
        int transparentColor;
        int action;

        // Get the alpha value. In case of failure, assume OPAQUE
        if (!GetLayeredWindowAttributes(windowHandle, out transparentColor, out originalAlpha, out action))
          originalAlpha = OPAQUE;
      }

      // Create a new WindowInfo
      WindowInfo window = new WindowInfo(originalStyle, originalAlpha);
      // Store it in our window list
      this.hijackedWindows[windowHandle] = window;

      return window;
    }

    /// <summary>
    /// Update the hotkey settings
    /// This is the event handler for the key code and check boxes!
    /// </summary>
    private void UpdateHotKey(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      this.userHotkey.Reregister((Keys)hotKeyComboBox.SelectedItem, shiftCheckBox.Checked, controlCheckBox.Checked, altCheckBox.Checked, windowsCheckBox.Checked);
    }
    /// <summary>
    /// Restore the window style based on the original style
    /// </summary>
    /// <param name="handle">Window handle</param>
    /// <param name="originalStyle">Original style</param>
    private void RestoreStyle(IntPtr handle, uint originalStyle)
    {
      uint style = GetWindowLong(handle, GWL_EX_STYLE);

      SetWindowLong(handle, GWL_EX_STYLE, (style & ~NEW_STYLE_ALL) | (originalStyle & NEW_STYLE_ALL));
      // Remove topmost style if not set in original style
      if ((originalStyle & WS_EX_TOPMOST) == 0)
        SetWindowPos(handle, new IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
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
    /// Register or unregister a hotkey
    /// </summary>
    private void RegisterHotkey(MOBZystems.Hotkey hotkey, bool register)
    {
      if (register)
      {
        if (!hotkey.IsRegistered)
          hotkey.Register(this);
      }
      else
      {
        if (hotkey.IsRegistered)
          hotkey.Unregister();
      }
    }

    /// <summary>
    /// Save the settings to the registry
    /// </summary>
    private void SaveSettings()
    {
      RegistryKey root = Registry.CurrentUser.CreateSubKey(REGROOT);

      root.SetValue("Transparency", this.semiTransparentValue.ToString());
      root.SetValue("ClickThrough", BoolToString(this.clickThroughCheckBox.Checked));
      root.SetValue("TopMost", BoolToString(this.topMostCheckBox.Checked));
      root.SetValue("Hotkey", MOBZystems.Hotkey.KeyCodeToString(this.userHotkey.KeyCode));
      root.SetValue("Shift", BoolToString(this.userHotkey.Shift));
      root.SetValue("Control", BoolToString(this.userHotkey.Control));
      root.SetValue("Alt", BoolToString(this.userHotkey.Alt));
      root.SetValue("Windows", BoolToString(this.userHotkey.Windows));

      root.SetValue("EnableLeftRight", BoolToString(this.sendToMonitorEnabledCheckBox.Checked));
      root.SetValue("EnableUpDown", BoolToString(this.minMaxEnabledCheckBox.Checked));
      root.SetValue("EnablePageUpDown", BoolToString(this.enableChangeTransparencyCheckbox.Checked));
    }
    #endregion

    #region UI Event Handlers
    /// <summary>
    /// Update the transparency when the track bar changes
    /// </summary>
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
    private void helpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        var psi = new ProcessStartInfo("http://www.mobzystems.com/Tools/SeeThroughWindows");
        psi.UseShellExecute = true;
        Process.Start(psi);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Show ourselves when the notify icon was clicked
    /// </summary>
    private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        if (!this.Visible)
          Show();

        Activate();
      }
    }

    /// <summary>
    /// Really close the form from the exit menu
    /// </summary>
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.exitingApplication = true;
      Close();

      Application.Exit();
    }

    /// <summary>
    /// (Un)register hotkeys for window minimize/maximize
    /// </summary>
    private void minMaxEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      RegisterHotkey(this.minimizeHotkey, minMaxEnabledCheckBox.Checked);
      RegisterHotkey(this.maximizeHotkey, minMaxEnabledCheckBox.Checked);
    }

    /// <summary>
    /// (Un)register hotkeys for move window
    /// </summary>
    private void sendToMonitorEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      RegisterHotkey(this.previousScreenHotkey, this.sendToMonitorEnabledCheckBox.Checked);
      RegisterHotkey(this.nextScreenHotkey, this.sendToMonitorEnabledCheckBox.Checked);
    }

    /// <summary>
    /// (Un)register hotkeys for window transparency change
    /// </summary>
    private void enableChangeTransparencyCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loading)
        return;

      RegisterHotkey(this.moreTransparentHotkey, enableChangeTransparencyCheckbox.Checked);
      RegisterHotkey(this.lessTransparentHotkey, enableChangeTransparencyCheckbox.Checked);
    }

    /// <summary>
    /// Enable topmost if clickthrough
    /// </summary>
    private void clickThroughCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      topMostCheckBox.Enabled = clickThroughCheckBox.Checked;
    }

    private void restoreAllButton_Click(object sender, EventArgs e)
    {
      RestoreAllWindows();
      UpdateUI();
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
  }
}