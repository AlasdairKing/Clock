namespace Clock
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileHide = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReminders = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemindersAddreminder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBar = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions24hour = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptionsShowseconds = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuChimesNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChimesHour = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChimesHalf = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChimesQuarter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.hideToSystemTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptionsStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpManual = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.textTime = new System.Windows.Forms.TextBox();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.nfyMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrReminders = new System.Windows.Forms.Timer(this.components);
            this.tmrShowBecauseInstance = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuReminders,
            this.mnuOptions,
            this.mnuHelp});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileHide,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            resources.ApplyResources(this.mnuFile, "mnuFile");
            // 
            // mnuFileHide
            // 
            this.mnuFileHide.Name = "mnuFileHide";
            resources.ApplyResources(this.mnuFileHide, "mnuFileHide");
            this.mnuFileHide.Click += new System.EventHandler(this.mnuFileHide_Click);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            resources.ApplyResources(this.mnuFileExit, "mnuFileExit");
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuReminders
            // 
            this.mnuReminders.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemindersAddreminder,
            this.tsmiBar});
            this.mnuReminders.Name = "mnuReminders";
            resources.ApplyResources(this.mnuReminders, "mnuReminders");
            // 
            // mnuRemindersAddreminder
            // 
            this.mnuRemindersAddreminder.Name = "mnuRemindersAddreminder";
            resources.ApplyResources(this.mnuRemindersAddreminder, "mnuRemindersAddreminder");
            this.mnuRemindersAddreminder.Click += new System.EventHandler(this.addReminderToolStripMenuItem_Click);
            // 
            // tsmiBar
            // 
            this.tsmiBar.Name = "tsmiBar";
            resources.ApplyResources(this.tsmiBar, "tsmiBar");
            // 
            // mnuOptions
            // 
            this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOptions24hour,
            this.mnuOptionsShowseconds,
            this.toolStripMenuItem1,
            this.mnuChimesNone,
            this.mnuChimesHour,
            this.mnuChimesHalf,
            this.mnuChimesQuarter,
            this.toolStripMenuItem2,
            this.hideToSystemTrayToolStripMenuItem,
            this.mnuOptionsStartup});
            this.mnuOptions.Name = "mnuOptions";
            resources.ApplyResources(this.mnuOptions, "mnuOptions");
            // 
            // mnuOptions24hour
            // 
            this.mnuOptions24hour.Checked = true;
            this.mnuOptions24hour.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuOptions24hour.Name = "mnuOptions24hour";
            resources.ApplyResources(this.mnuOptions24hour, "mnuOptions24hour");
            this.mnuOptions24hour.Click += new System.EventHandler(this.mnuOptions24hour_Click);
            // 
            // mnuOptionsShowseconds
            // 
            this.mnuOptionsShowseconds.Name = "mnuOptionsShowseconds";
            resources.ApplyResources(this.mnuOptionsShowseconds, "mnuOptionsShowseconds");
            this.mnuOptionsShowseconds.Click += new System.EventHandler(this.mnuOptionsShowseconds_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // mnuChimesNone
            // 
            this.mnuChimesNone.Name = "mnuChimesNone";
            resources.ApplyResources(this.mnuChimesNone, "mnuChimesNone");
            this.mnuChimesNone.Click += new System.EventHandler(this.ChangeChimes);
            // 
            // mnuChimesHour
            // 
            this.mnuChimesHour.Name = "mnuChimesHour";
            resources.ApplyResources(this.mnuChimesHour, "mnuChimesHour");
            this.mnuChimesHour.Click += new System.EventHandler(this.ChangeChimes);
            // 
            // mnuChimesHalf
            // 
            this.mnuChimesHalf.Name = "mnuChimesHalf";
            resources.ApplyResources(this.mnuChimesHalf, "mnuChimesHalf");
            this.mnuChimesHalf.Click += new System.EventHandler(this.ChangeChimes);
            // 
            // mnuChimesQuarter
            // 
            this.mnuChimesQuarter.Name = "mnuChimesQuarter";
            resources.ApplyResources(this.mnuChimesQuarter, "mnuChimesQuarter");
            this.mnuChimesQuarter.Click += new System.EventHandler(this.ChangeChimes);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // hideToSystemTrayToolStripMenuItem
            // 
            this.hideToSystemTrayToolStripMenuItem.Name = "hideToSystemTrayToolStripMenuItem";
            resources.ApplyResources(this.hideToSystemTrayToolStripMenuItem, "hideToSystemTrayToolStripMenuItem");
            this.hideToSystemTrayToolStripMenuItem.Click += new System.EventHandler(this.hideToSystemTrayToolStripMenuItem_Click);
            // 
            // mnuOptionsStartup
            // 
            this.mnuOptionsStartup.Name = "mnuOptionsStartup";
            resources.ApplyResources(this.mnuOptionsStartup, "mnuOptionsStartup");
            this.mnuOptionsStartup.Click += new System.EventHandler(this.mnuOptionsStartup_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpManual,
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            resources.ApplyResources(this.mnuHelp, "mnuHelp");
            // 
            // mnuHelpManual
            // 
            this.mnuHelpManual.Name = "mnuHelpManual";
            resources.ApplyResources(this.mnuHelpManual, "mnuHelpManual");
            this.mnuHelpManual.Click += new System.EventHandler(this.mnuHelpManual_Click);
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            resources.ApplyResources(this.mnuHelpAbout, "mnuHelpAbout");
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // textTime
            // 
            this.textTime.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.textTime, "textTime");
            this.textTime.Name = "textTime";
            this.textTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textTime_KeyDown);
            // 
            // tmrClock
            // 
            this.tmrClock.Enabled = true;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // nfyMain
            // 
            resources.ApplyResources(this.nfyMain, "nfyMain");
            this.nfyMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nfyMain_MouseClick);
            // 
            // tmrReminders
            // 
            this.tmrReminders.Enabled = true;
            this.tmrReminders.Interval = 2000;
            this.tmrReminders.Tick += new System.EventHandler(this.tmrReminders_Tick);
            // 
            // tmrShowBecauseInstance
            // 
            this.tmrShowBecauseInstance.Enabled = true;
            this.tmrShowBecauseInstance.Interval = 500;
            this.tmrShowBecauseInstance.Tick += new System.EventHandler(this.tmrShowBecauseInstance_Tick);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textTime);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ClientSizeChanged += new System.EventHandler(this.frmMain_ClientSizeChanged);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileHide;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuReminders;
        private System.Windows.Forms.ToolStripMenuItem mnuRemindersAddreminder;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions24hour;
        private System.Windows.Forms.ToolStripMenuItem mnuOptionsShowseconds;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpManual;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.TextBox textTime;
        private System.Windows.Forms.Timer tmrClock;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuChimesNone;
        private System.Windows.Forms.ToolStripMenuItem mnuChimesHour;
        private System.Windows.Forms.ToolStripMenuItem mnuChimesHalf;
        private System.Windows.Forms.ToolStripMenuItem mnuChimesQuarter;
        private System.Windows.Forms.ToolStripSeparator tsmiBar;
        private System.Windows.Forms.NotifyIcon nfyMain;
        private System.Windows.Forms.ToolStripMenuItem hideToSystemTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.Timer tmrReminders;
        private System.Windows.Forms.ToolStripMenuItem mnuOptionsStartup;
        private System.Windows.Forms.Timer tmrShowBecauseInstance;
    }
}

