using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace SeeThroughWindows
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // Make sure we have a single instance of this application running:
      bool ok;
      Mutex m = new Mutex(true, "MOBZystems.SeeThroughWindows", out ok);

      if (!ok)
      {
        MessageBox.Show(null, "See Through Windows is already active in the system tray!", "See Through Windows", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Exit immediately
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Form f = new SeeThrougWindowsForm();
      Application.Run();

      GC.KeepAlive(m);                // important!
    }
  }
}