using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Clock
{
    public partial class frmReminder : Form
    {
        public frmReminder()
        {
            InitializeComponent();
        }
#if USE_SYSTEM_SPEECH
        private System.Speech.Synthesis.SpeechSynthesizer tts;
#else
        private SpeechLib.SpVoice voice;
#endif
        public string reminderText
        {
            get;
            set;
        }

        private void frmReminder_Activated(object sender, EventArgs e)
        {
            tmrRepeat.Enabled = true;   

        }

        private void tmrRepeat_Tick(object sender, EventArgs e)
        {
            tmrRepeat.Enabled = false;
            btnReminder.Text = reminderText;
#if USE_SYSTEM_SPEECH
            tts.SpeakAsync(reminderText);
#else
            try
            {
                voice.Speak(reminderText, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            }
            catch (Exception err)
            {
                System.Diagnostics.EventLog.WriteEntry("Clock tmrRepeat_Tick", "Error using SAPI5 through COM: " + err.Message);
            }
#endif
        }

        private void frmReminder_Load(object sender, EventArgs e)
        {
#if USE_SYSTEM_SPEECH
            tts = new System.Speech.Synthesis.SpeechSynthesizer();
            tts.SpeakCompleted += new EventHandler<System.Speech.Synthesis.SpeakCompletedEventArgs>(tts_SpeakCompleted);
#else
            try
            {
                voice = new SpeechLib.SpVoice();
                voice.EndStream += new SpeechLib._ISpeechVoiceEvents_EndStreamEventHandler(voice_EndStream);
            }
            catch (Exception err)
            {
                System.Diagnostics.EventLog.WriteEntry("Clock frmReminder_Load", "Error using SAPI5 through COM: " + err.Message);
            }
#endif
        }

        
#if USE_SYSTEM_SPEECH
        void tts_SpeakCompleted(object sender, System.Speech.Synthesis.SpeakCompletedEventArgs e)
        {
            tmrRepeat.Enabled = true;
        }
#else
        void voice_EndStream(int StreamNumber, object StreamPosition)
        {
            tmrRepeat.Enabled = true;
        }
#endif

        private void btnReminder_Click(object sender, EventArgs e)
        {
            tmrRepeat.Enabled = false;
#if USE_SYSTEM_SPEECH
            tts.SpeakAsyncCancelAll();
#else
            try
            {
                voice.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            }
            catch
            {
                // SAPI5 problem. Catch when object instantiated or used earlier.
            }
#endif
            Application.DoEvents();
            this.Close();
        }

    }
}
