using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Clock
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, Flags = System.Security.Permissions.SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            // Make single-instance by checking for existing running version.
            if (PriorProcess() != null)
            {
                // Tell the other me to show
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Clock 2", true);
                if (regKey == null)
                {
                    Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clock 2");
                    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Clock 2", true);
                }
                regKey.SetValue("Show", "true");
                regKey.Close();

                // And exit.
                return;
            }

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(UIThreadException);
            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Upgrade settings from previous installed versions.
            UpgradeSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        /// <summary>
        /// From http://www.ai.uga.edu/mc/SingleInstance.html, 22 June 2013.
        /// </summary>
        /// <returns></returns>
        public static System.Diagnostics.Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            System.Diagnostics.Process curr = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(curr.ProcessName);
            foreach (System.Diagnostics.Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }

        private static void UIThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(Application.ProductName))
                {
                    System.Diagnostics.EventLog.CreateEventSource(Application.ProductName, Application.ProductName);
                }

                System.Diagnostics.EventLog.WriteEntry(Application.ProductName, e.Exception.Message + "\r\n" + e.Exception.Source, System.Diagnostics.EventLogEntryType.Error, 1000);
            }
            catch
            {
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            // Doesn't matter what you do, the application will terminate.
        }

        /// <summary>
        /// UpgradeSettings. This function migrates your application's settings from the previous
        /// version, if any, to this one. This is because Properties.Settings are saved to a 
        /// different user folder with every version, so unless you explicitly call this function
        /// then user settings will be lost with every upgrade.
        /// You must create a String setting called "LastVersionRun"
        /// Alasdair 11 June 2013
        /// </summary>
        public static void UpgradeSettings()
        {
            try
            {
                if (Properties.Settings.Default.LastVersionRun != Application.ProductVersion)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.Reload();
                    Properties.Settings.Default.LastVersionRun = Application.ProductVersion;
                    Properties.Settings.Default.Save();
                }
            }
            catch
            {
                //MessageBox.Show("Error in UpgradeSettings. Have you created a property called \"LastVersionRun\"?");
            }
        }
        // End of UpgradeSettings()

    }
}
