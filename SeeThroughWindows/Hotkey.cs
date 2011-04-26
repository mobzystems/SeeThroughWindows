using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MOBZystems // was: MovablePython, but I changed a lot in the mean time
{
  public class Hotkey : IMessageFilter
  {
    #region Interop

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int UnregisterHotKey(IntPtr hWnd, int id);

    private const uint WM_HOTKEY = 0x312;

    private const uint MOD_ALT = 0x1;
    private const uint MOD_CONTROL = 0x2;
    private const uint MOD_SHIFT = 0x4;
    private const uint MOD_WIN = 0x8;

    private const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409;

    #endregion

    private static int currentID;
    private const int maximumID = 0xBFFF;

    private Keys keyCode;
    private bool shift;
    private bool control;
    private bool alt;
    private bool windows;

    private int id;
    private bool isRegistered;
    private Control windowControl;

    public event HandledEventHandler Pressed;

    /// <summary>
    /// Private constructor. Registers this hotkey as an application message filter
    /// </summary>
    private Hotkey()
    {
      // Register us as a message filter
      Application.AddMessageFilter(this);
    }

    /// <summary>
    /// Constructor with keycode and modifiers
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="shift"></param>
    /// <param name="control"></param>
    /// <param name="alt"></param>
    /// <param name="windows"></param>
    public Hotkey(Keys keyCode, bool shift, bool control, bool alt, bool windows)
      : this()
    {
      // Assign properties
      this.KeyCode = keyCode;
      this.Shift = shift;
      this.Control = control;
      this.Alt = alt;
      this.Windows = windows;
    }

    /// <summary>
    /// Constructor with hotkey name (e.g. "Control+F1") 
    /// </summary>
    /// <param name="name"></param>
    public Hotkey(string name)
      : this()
    {
      this.shift = false;
      this.control = false;
      this.alt = false;
      this.windows = false;

      if (name.StartsWith("Shift+"))
      {
        this.shift = true;
        name = name.Substring(6);
      }
      if (name.StartsWith("Control+"))
      {
        this.control = true;
        name = name.Substring(8);
      }
      if (name.StartsWith("Alt+"))
      {
        this.alt = true;
        name = name.Substring(4);
      }
      if (name.StartsWith("Windows+"))
      {
        this.windows = true;
        name = name.Substring(8);
      }

      this.keyCode = KeyCodeFromString(name);
    }

    /// <summary>
    /// Helper function to convert a key name ('F1') to a key code (Keys.F1)
    /// </summary>
    /// <param name="keyname"></param>
    /// <returns></returns>
    public static Keys KeyCodeFromString(string keyname)
    {
      if (keyname.Length == 1 && Char.IsDigit(keyname[0]))
        keyname = "D" + keyname;

      return (Keys)Enum.Parse(typeof(Keys), keyname);
    }

    /// <summary>
    /// Helper function to convert a key code (Keys.F1) to a string ('F1')
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string KeyCodeToString(Keys key)
    {
      string keyName = Enum.GetName(typeof(Keys), key);
      switch (key)
      {
        case Keys.D0:
        case Keys.D1:
        case Keys.D2:
        case Keys.D3:
        case Keys.D4:
        case Keys.D5:
        case Keys.D6:
        case Keys.D7:
        case Keys.D8:
        case Keys.D9:
          // Strip the first character
          keyName = keyName.Substring(1);
          break;
        default:
          // Leave everything alone
          break;
      }

      return keyName;
    }

    /// <summary>
    /// Destructor: unregister the hotkey
    /// </summary>
    ~Hotkey()
    {
      // Unregister the hotkey if necessary
      if (this.IsRegistered)
      {
        this.Unregister();
      }
    }

    /// <summary>
    /// Clone method. Creates a new hotkey with the same properties
    /// </summary>
    /// <returns></returns>
    public Hotkey Clone()
    {
      // Clone the whole object
      return new Hotkey(this.keyCode, this.shift, this.control, this.alt, this.windows);
    }

    /// <summary>
    /// This method is 'required by the underlying API'?
    /// </summary>
    /// <param name="windowControl"></param>
    /// <returns></returns>
    public bool GetCanRegister(Control windowControl)
    {
      // Handle any exceptions: they mean "no, you can't register" :)
      try
      {
        // Attempt to register
        if (!this.Register(windowControl))
        {
          return false;
        }

        // Unregister and say we managed it
        this.Unregister();
        return true;
      }
      catch (Win32Exception)
      {
        return false;
      }
      catch (NotSupportedException)
      {
        return false;
      }
    }

    /// <summary>
    /// Register the hotkey with a Windows control
    /// </summary>
    /// <param name="windowControl"></param>
    /// <returns></returns>
    public bool Register(Control windowControl)
    {
      // Check that we have not registered
      if (this.isRegistered)
      {
        throw new NotSupportedException("You cannot register a hotkey that is already registered");
      }

      // We can't register an empty hotkey
      if (this.IsEmpty)
      {
        throw new NotSupportedException("You cannot register an empty hotkey");
      }

      // Get an ID for the hotkey and increase current ID
      this.id = Hotkey.currentID;
      Hotkey.currentID = (Hotkey.currentID + 1) % Hotkey.maximumID;

      // Translate modifier keys into unmanaged version
      uint modifiers = (this.Alt ? Hotkey.MOD_ALT : 0)
        | (this.Control ? Hotkey.MOD_CONTROL : 0)
        | (this.Shift ? Hotkey.MOD_SHIFT : 0)
        | (this.Windows ? Hotkey.MOD_WIN : 0);

      // Register the hotkey
      if (Hotkey.RegisterHotKey(windowControl.Handle, this.id, modifiers, keyCode) == 0)
      {
        // Is the error that the hotkey is registered?
        if (Marshal.GetLastWin32Error() == ERROR_HOTKEY_ALREADY_REGISTERED)
        {
          return false;
        }
        else
        {
          throw new Win32Exception();
        }
      }

      // Save the control reference and register state
      this.isRegistered = true;
      this.windowControl = windowControl;

      // We successfully registered
      return true;
    }

    /// <summary>
    /// Unregister the hotkey
    /// </summary>
    public void Unregister()
    {
      // Check that we have registered
      if (!this.isRegistered)
      {
        throw new NotSupportedException("You cannot unregister a hotkey that is not registered");
      }

      // It's possible that the control itself has died: in that case, no need to unregister!
      if (!this.windowControl.IsDisposed)
      {
        // Clean up after ourselves
        if (Hotkey.UnregisterHotKey(this.windowControl.Handle, this.id) == 0)
        {
          throw new Win32Exception();
        }
      }

      // Clear the control reference and register state
      this.isRegistered = false;
      this.windowControl = null;
    }

    /// <summary>
    /// Re-register the hotkey using the current properties
    /// </summary>
    private void Reregister()
    {
      // Only do something if the key is already registered
      if (!this.isRegistered)
      {
        return;
      }

      // Save control reference
      Control windowControl = this.windowControl;

      // Unregister and then reregister again
      this.Unregister();
      this.Register(windowControl);
    }

    /// <summary>
    /// RE-register the hotkey with new properties
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="shift"></param>
    /// <param name="control"></param>
    /// <param name="alt"></param>
    /// <param name="windows"></param>
    public void Reregister(Keys keyCode, bool shift, bool control, bool alt, bool windows)
    {
      // Assign properties
      this.KeyCode = keyCode;
      this.Shift = shift;
      this.Control = control;
      this.Alt = alt;
      this.Windows = windows;

      Reregister();
    }

    /// <summary>
    /// IMessageFilter implementation.
    /// </summary>
    /// <param name="message"></param>
    /// <returns>true if the message is to be removed from the application message queue</returns>
    public bool PreFilterMessage(ref Message message)
    {
      // Only process WM_HOTKEY messages
      if (message.Msg != Hotkey.WM_HOTKEY)
      {
        return false;
      }

      // Check that the ID is our key and we are registerd
      if (this.isRegistered && (message.WParam.ToInt32() == this.id))
      {
        // Fire the event and pass on the event if our handlers didn't handle it
        return this.OnPressed();
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Fire the Pressed event
    /// </summary>
    /// <returns></returns>
    private bool OnPressed()
    {
      // Fire the event if we can
      HandledEventArgs handledEventArgs = new HandledEventArgs(false);
      if (this.Pressed != null)
      {
        this.Pressed(this, handledEventArgs);
      }

      // Return whether we handled the event or not
      return handledEventArgs.Handled;
    }

    /// <summary>
    /// String representation of the hotkey, e.g. "Control+F1"
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      // We can be empty
      if (this.IsEmpty)
      {
        return "(none)";
      }

      // Build key name
      string keyName = KeyCodeToString(this.keyCode);

      // Build modifiers
      string modifiers = "";
      if (this.shift)
      {
        modifiers += "Shift+";
      }
      if (this.control)
      {
        modifiers += "Control+";
      }
      if (this.alt)
      {
        modifiers += "Alt+";
      }
      if (this.windows)
      {
        modifiers += "Windows+";
      }

      // Return result
      return modifiers + keyName;
    }

    /// <summary>
    /// If the hotkey empty?
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return this.keyCode == Keys.None; 
      }
    }

    /// <summary>
    /// Is the hotkey registered?
    /// </summary>
    public bool IsRegistered
    {
      get
      {
        return this.isRegistered;
      }
    }

    /// <summary>
    /// The key code of the hot key (excluding modifiers)
    /// </summary>
    public Keys KeyCode
    {
      get
      {
        return this.keyCode;
      }
      set
      {
        // Save and reregister
        this.keyCode = value;
        this.Reregister();
      }
    }

    /// <summary>
    /// The Shift modifier
    /// </summary>
    public bool Shift
    {
      get
      {
        return this.shift;
      }
      set
      {
        // Save and reregister
        this.shift = value;
        this.Reregister();
      }
    }

    /// <summary>
    /// The Control modifier
    /// </summary>
    public bool Control
    {
      get
      { 
        return this.control;
      }
      set
      {
        // Save and reregister
        this.control = value;
        this.Reregister();
      }
    }

    /// <summary>
    /// The Alt modifier
    /// </summary>
    public bool Alt
    {
      get
      {
        return this.alt;
      }
      set
      {
        // Save and reregister
        this.alt = value;
        this.Reregister();
      }
    }

    /// <summary>
    /// The Windows modifier
    /// </summary>
    public bool Windows
    {
      get
      {
        return this.windows;
      }
      set
      {
        // Save and reregister
        this.windows = value;
        this.Reregister();
      }
    }
  }
}
