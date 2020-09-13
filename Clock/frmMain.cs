using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

///
/// Clock
/// 
/// Alasdair King, April 2013
/// A simple big clock with speech and reminders.
/// Previous versions were VB6: this is the .Net version.
/// Notes:
/// Uses SAPI5 through COM, since System.Speech only came along
/// in .Net 3, so my USB stick .Net 2 version would need 
/// different code. 

// TODO
//      I18N name of program when installed.
//      I18N time format.


namespace Clock
{
    /// <summary>
    /// When the clock chimes.
    /// </summary>
    public enum Chimes
    {
        /// <summary>
        /// Not at all.
        /// </summary>
        None,
        /// <summary>
        /// On the hour.
        /// </summary>
        Hour,
        /// <summary>
        /// On the quarter hour.
        /// </summary>
        QuarterHour,
        /// <summary>
        /// On the half hour.
        /// </summary>
        HalfHour
    }

    public partial class frmMain : Form
    {

        /// <summary>
        /// For internationalisation.
        /// </summary>
        public I18N _I18N;
        
        public frmMain()
        {
            InitializeComponent();
        }

        private clsReminders mReminders;

        /// <summary>
        /// Notes the last time announcement we did, so we don't do it again.
        /// </summary>
        private string mAnnounced;
        
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ChangeChimes(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem tsmi = (System.Windows.Forms.ToolStripMenuItem)sender;
            this.mnuChimesHalf.Checked = false;
            this.mnuChimesHour.Checked = false;
            this.mnuChimesNone.Checked = false;
            this.mnuChimesQuarter.Checked = false;
            tsmi.Checked = true;
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            string displayFormat;
            if (this.mnuOptionsShowseconds.Checked)
            {
                displayFormat = "mm:ss";
            }
            else
            {
                displayFormat = "mm";
            }
            if (this.mnuOptions24hour.Checked)
            {
                displayFormat = "H:" + displayFormat;
            }
            else
            {
                displayFormat = "h:" + displayFormat + " tt";
            }
            try
            {
                this.textTime.Text = System.DateTime.Now.ToString(displayFormat + ", d MMM yyyy");
            }
            catch
            {
                // Formatting and I18N can go wrong. If the above doesn't work...
                this.textTime.Text = System.DateTime.Now.ToLongTimeString() + "  " + System.DateTime.Now.ToLongDateString();
            }
            this.nfyMain.Text = this.textTime.Text;

            // Check for quarter, half hour etc. that we will announce.
            if (Properties.Settings.Default.Chimes != Chimes.None)
            {
                // Not already announced this.
                string mins = System.DateTime.Now.ToShortTimeString();
                mins = mins.Substring(mins.Length - 2, 2);
                if (mins != this.mAnnounced)
                {
                    bool doChimes = false;
                    switch (Properties.Settings.Default.Chimes)
                    {
                        case Chimes.Hour:
                            if (mins == "00")
                                doChimes = true;
                            break;
                        case Chimes.HalfHour:
                            if (mins == "00" || mins == "30")
                                doChimes = true;
                            break;
                        case Chimes.QuarterHour:
                            if (mins == "00" || mins == "30" || mins == "15" || mins == "45")
                                doChimes = true;
                            break;
                    }
                    if (doChimes)
                    {
                        this.mAnnounced = mins;
                        Speak(System.DateTime.Now.ToShortTimeString());
                    }

                }
            }
        }

        private void mnuOptions24hour_Click(object sender, EventArgs e)
        {
            mnuOptions24hour.Checked = !mnuOptions24hour.Checked;
            Properties.Settings.Default.TwentyFourHour = mnuOptions24hour.Checked;
            DisplayReminders();
        }

        private void mnuOptionsShowseconds_Click(object sender, EventArgs e)
        {
            mnuOptionsShowseconds.Checked = !mnuOptionsShowseconds.Checked;
            Properties.Settings.Default.ShowSeconds = mnuOptionsShowseconds.Checked;
        }

        private void addReminderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSchedule fs = new frmSchedule();
            this._I18N.DoForm(fs);
            fs.Is24HourClock = mnuOptions24hour.Checked;
            fs.ShowDialog(this);
            if (fs.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                mReminders.AddReminder(fs.NewReminder);
                DisplayReminders();
            }
            fs.Close();
        }

        /// <summary>
        /// Checks for udpates if we haven't already checked today Prevents repeated cycles of update attempts 
        /// if there is a problem with updating. Requires string value "UpdateCheck" created as a property.
        /// Add "CheckForUpdates();" to your Form_Load event.
        /// 18 June 2013
        /// </summary>
        private void CheckForUpdates(string url)
        {
            // Have we checked already today?
            if (Properties.Settings.Default.UpdateCheck != System.DateTime.Now.ToShortDateString())
            {
                // No! Let's do so.
                // First note that we have now checked today.
                Properties.Settings.Default.UpdateCheck = System.DateTime.Now.ToShortDateString();
                Properties.Settings.Default.Save();
                UpdaterCSharp.Updater.CheckForUpdates(url);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this._I18N = new I18N();
            this._I18N.DoForm(this);

            // Check for updates if a file called WebbIEUpdater.dll is present.
            if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.
                GetExecutingAssembly().Location) + "\\WebbIEUpdater.dll"))
            {
                CheckForUpdates("http://www.webbie.org.uk/clock/updates.xml");
            }

            mnuOptions24hour.Checked = Properties.Settings.Default.TwentyFourHour;
            mnuOptionsShowseconds.Checked = Properties.Settings.Default.ShowSeconds;
            hideToSystemTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinimizeToSystemTray;
            this.mnuOptionsStartup.Checked = Properties.Settings.Default.StartUpWithWindows;

            mReminders = new clsReminders();
            mReminders.LoadReminders();
            DisplayReminders();

            switch (Properties.Settings.Default.Chimes)
            {
                case Chimes.HalfHour:
                    mnuChimesHalf.Checked = true;
                    break;
                case Chimes.Hour:
                    mnuChimesHour.Checked = true;
                    break;
                case Chimes.None:
                    mnuChimesNone.Checked = true;
                    break;
                case Chimes.QuarterHour:
                    mnuChimesQuarter.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Loads the current set of reminders into the Reminders menu in the form.
        /// </summary>
        private void DisplayReminders()
        {
            // First clear all reminders
            for (int i = mnuReminders.DropDownItems.Count -1; i > 1; i--) 
            {
                mnuReminders.DropDownItems.Remove(mnuReminders.DropDownItems[i]);
            }

            // Now add current ones
            foreach (clsReminderEntry re in mReminders.Reminders)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(re.whenString + " - " + re.reminder);
                tsmi.Tag = re;
                tsmi.Click += new EventHandler(tsmi_Click);
                mnuReminders.DropDownItems.Add(tsmi);
            }
        }

        /// <summary>
        /// Click on a Reminder, go open it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tsmi_Click(object sender, EventArgs e)
        {
            clsReminderEntry re = (clsReminderEntry)(((ToolStripMenuItem)sender).Tag);
            frmSchedule fs = new frmSchedule();
            this._I18N.DoForm(fs);
            fs.NewReminder = re;
            fs.ShowDialog(this);
            this.Focus();
            if (fs.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                mReminders.DeleteReminder(re);
                // Delete or edit?
                if (fs.DeleteReminder)
                {
                    // Don't add in again.
                }
                else
                {
                    // Add amended version in again.
                    mReminders.AddReminder(fs.NewReminder);
                }
                DisplayReminders();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mReminders.SaveReminders();
            Properties.Settings.Default.ShowSeconds = mnuOptionsShowseconds.Checked;
            Properties.Settings.Default.TwentyFourHour = mnuOptions24hour.Checked;
            if (mnuChimesHalf.Checked)
                Properties.Settings.Default.Chimes = Chimes.HalfHour;
            else if (mnuChimesHour.Checked)
                Properties.Settings.Default.Chimes = Chimes.Hour;
            else if (mnuChimesNone.Checked)
                Properties.Settings.Default.Chimes = Chimes.None;
            else if (mnuChimesQuarter.Checked)
                Properties.Settings.Default.Chimes = Chimes.QuarterHour;
            Properties.Settings.Default.MinimizeToSystemTray = hideToSystemTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void nfyMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                while (!this.Visible)
                {
                    this.Visible = true;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                this.WindowState = FormWindowState.Minimized;
        }

        private void hideToSystemTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToSystemTrayToolStripMenuItem.Checked = !hideToSystemTrayToolStripMenuItem.Checked;
        }

        private void mnuHelpManual_Click(object sender, EventArgs e)
        {
            _I18N.ShowHelp();
        }

        private void frmMain_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, Application.ProductName + "\t" + Application.ProductVersion + "\n\r" + Application.CompanyName + "\n\rhttp://www.webbie.org.uk", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tmrReminders_Tick(object sender, EventArgs e)
        {
            mReminders.Check(this._I18N);
        }

        private void mnuFileHide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void textTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Speak(textTime.Text);
            }
        }

        private void Speak(string s)
        {
#if USE_SYSTEM_SPEECH
                System.Speech.Synthesis.SpeechSynthesizer tts = new System.Speech.Synthesis.SpeechSynthesizer();
                tts.SpeakAsync(textTime.Text);
#else
                try
                {
                    SpeechLib.SpVoice voice = new SpeechLib.SpVoice();
                    voice.Speak(s, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                }
                catch (Exception err)
                {
                    System.Diagnostics.EventLog.WriteEntry("Clock", "Failed to use SAPI5 through COM: " + err.Message);
                }
#endif
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && this.hideToSystemTrayToolStripMenuItem.Checked)
            {
                // Minimimized, hide (use system tray to get back)
                if (this.Visible)
                {
                    this.Visible = false;
                }
            }
        }

        private void mnuOptionsStartup_Click(object sender, EventArgs e)
        {
            mnuOptionsStartup.Checked = !mnuOptionsStartup.Checked;
            Properties.Settings.Default.StartUpWithWindows = mnuOptionsStartup.Checked;
            StartUpWithWindows(Properties.Settings.Default.StartUpWithWindows);
        }

        private void StartUpWithWindows(bool startup)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (startup)
            {
                regKey.SetValue("WebbIEClock", "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                regKey.DeleteValue("WebbIEClock");
            }
            regKey.Close();
        }

        /// <summary>
        /// Checks to see if another instance of Clock has been instantiated, has found this instance, and 
        /// has therefore closed, leaving only the instruction to show the running instance (me)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrShowBecauseInstance_Tick(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Clock 2", true);
            if (regKey == null)
            {
                // Certainly hasn't written it!
            }
            else
            {
                if ((string)regKey.GetValue("Show") == "true")
                {
                    // Yes! Show me.
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                    // And delete key.
                    regKey.DeleteValue("Show");
                }
            }
            
        }
    }
}
