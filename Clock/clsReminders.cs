using System;
using System.Collections.Generic;
using System.Text;

namespace Clock
{
    /// <summary>
    /// Handles saved reminders - times when a message should be displayed to the user.
    /// </summary>
    public class clsReminders
    {
        
        private System.Collections.ArrayList mReminders;

        public System.Collections.ArrayList Reminders
        {
            get
            {
                return mReminders;
            }
        }

        public clsReminders()
        {
            mReminders = new System.Collections.ArrayList();
        }

        private string ReminderFilePath()
        {
            return (new System.IO.DirectoryInfo(System.Windows.Forms.Application.UserAppDataPath)).Parent.FullName + "\\reminders.xml";
        }

        public void LoadReminders()
        {
            mReminders.Clear();
            System.Xml.XmlDocument reminders = new System.Xml.XmlDocument();
            string reminderPath = ReminderFilePath();
            if (!System.IO.File.Exists(reminderPath))
            {
                // Have we got a previous version?
                reminderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WebbIE\\Clock\\1\\reminders.xml";
            }
            if (System.IO.File.Exists(reminderPath))
            {
                reminders.Load(reminderPath);
                foreach (System.Xml.XmlNode reminder in reminders.DocumentElement.SelectNodes("reminderItem"))
                {
                    clsReminderEntry re = new clsReminderEntry();
                    re.when = reminder.SelectSingleNode("when").InnerText;
                    re.fired = reminder.SelectSingleNode("fired").InnerText;
                    re.reminder = reminder.SelectSingleNode("reminder").InnerText;
                    mReminders.Add(re);
                }
                mReminders.Sort();
            }
        }

        public void SaveReminders()
        {
            System.Xml.XmlDocument reminders = new System.Xml.XmlDocument();
            reminders.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><reminders />");
            for (int i = 0; i < mReminders.Count; i++)
            {
                System.Xml.XmlNode rem = reminders.CreateNode(System.Xml.XmlNodeType.Element, "reminderItem", "");
                rem.AppendChild(reminders.CreateNode(System.Xml.XmlNodeType.Element, "when", ""));
                rem.AppendChild(reminders.CreateNode(System.Xml.XmlNodeType.Element, "fired", ""));
                rem.AppendChild(reminders.CreateNode(System.Xml.XmlNodeType.Element, "reminder", ""));
                clsReminderEntry re = (clsReminderEntry)mReminders[i];
                rem.SelectSingleNode("when").InnerText = re.when;
                rem.SelectSingleNode("fired").InnerText = re.fired;
                rem.SelectSingleNode("reminder").InnerText = re.reminder;
                reminders.DocumentElement.AppendChild(rem);
            }
            reminders.Save(ReminderFilePath());
        }

        public void AddReminder(clsReminderEntry reminder)
        {
            mReminders.Add(reminder);
            mReminders.Sort();
        }

        public void Check(I18N i18n)
        {
            string nowString = System.DateTime.Now.ToString("hh:mm");
            string report = "";
            for (int i = 0; i < mReminders.Count; i++)
            {
                clsReminderEntry r = (clsReminderEntry)mReminders[i];
                if (r.fired != nowString) // Also okay for r.fired = ""
                {
                    if (r.when == nowString)
                    {
                        report += r.reminder + "\n";
                        r.fired = nowString;
                    }
                }
            }
            if (report.Length > 0)
            {
                frmReminder rem = new frmReminder();
                i18n.DoForm(rem);
                rem.reminderText = report;
                rem.Show();
            }
        }

        public void DeleteReminder(clsReminderEntry re)
        {
            mReminders.Remove(re);
        }
    
//Public Sub SetReminders(doc As DOMDocument30)
//    On Error Resume Next
//    Dim n As IXMLDOMNode
//    Dim r As clsReminder
    
//    For Each n In doc.documentElement.selectNodes("reminderItem")
//        If mReminders Is Nothing Then Set mReminders = New Collection
//        Set r = New clsReminder
//        Call r.ParseXML(n)
//        Call mReminders.Add(r)
//    Next n
//End Sub

//Public Sub DeleteReminder(r As clsReminder)
//    On Error Resume Next
//    Dim i As Long
//    If Not (mReminders Is Nothing) Then
//        For i = 1 To mReminders.Count
//            If mReminders.Item(i) Is r Then
//                Call mReminders.Remove(i)
//                Exit For
//            End If
//        Next i
//    End If
//End Sub


    }

    public class clsReminderEntry:IComparable
    {
        
        int IComparable.CompareTo(object obj)
        {
            clsReminderEntry re = (clsReminderEntry)obj;
            System.DateTime thisWhen = System.DateTime.Parse(this.when);
            System.DateTime thatWhen = System.DateTime.Parse(re.when);
            return System.DateTime.Compare(thisWhen, thatWhen);
        }

        public string whenString
        {
            get
            {
                if (Properties.Settings.Default.TwentyFourHour)
                {
                    return this.when;
                }
                else
                {
                    System.DateTime dt = System.DateTime.Parse(this.when);
                    return dt.ToString("h:mm tt");
                }
            }
        }

        public string when
        {
            get;
            set;
        }
        public string fired
        {
            get;
            set;
        }
        public string reminder
        {
            get;
            set;
        }

    }

}
