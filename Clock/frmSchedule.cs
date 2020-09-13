using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Clock
{
    public partial class frmSchedule : Form
    {
        public bool Is24HourClock
        {
            get;
            set;
        }

        public bool DeleteReminder
        {
            get;
            set;
        }
        
        public clsReminderEntry NewReminder
        {
            get
            {
                clsReminderEntry re = new clsReminderEntry();
                re.reminder = txtReminder.Text;
                re.when = System.DateTime.Parse( lstTime.Text).ToString("hh:mm");
                return re;
            }
            set
            {
                txtReminder.Text = value.reminder;
                for (int i = 0; i < lstTime.Items.Count; i++)
                {
                    if (lstTime.Items[i].ToString() == value.when)
                    {
                        lstTime.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public frmSchedule()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        private void frmSchedule_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 24 * 60 + 1; i++)
            {
                System.DateTime newTime = System.DateTime.Now.AddMinutes(i);
                if (Is24HourClock)
                {
                    lstTime.Items.Add(newTime.ToString("H:mm"));
                }
                else
                {
                    lstTime.Items.Add(newTime.ToString("h:mm tt"));
                }
            }
        }

        private void lstTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown)
            {
                if (lstTime.SelectedIndex < lstTime.Items.Count - 10)
                {
                    lstTime.SelectedIndex = lstTime.SelectedIndex + 10;
                }
                else
                {
                    lstTime.SelectedIndex = lstTime.Items.Count - 1;
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (lstTime.SelectedIndex > 0)
                {
                    lstTime.SelectedIndex = lstTime.SelectedIndex - 10;
                }
                else
                {
                    lstTime.SelectedIndex = 0;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // TODO What about the Delete button?
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            clsReminderEntry re = new clsReminderEntry();
            re.when = System.DateTime.Parse( lstTime.Text).ToString("hh:mm");
            re.reminder = txtReminder.Text;
            this.Hide();
        }

        private void frmSchedule_Activated(object sender, EventArgs e)
        {
            lstTime.SelectedIndex = 0;
            lstTime.Focus();
        }

        private void btnDeleteReminder_Click(object sender, EventArgs e)
        {
            this.DeleteReminder = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        private void txtReminder_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (txtReminder.Text.Trim().Length > 0);
        }
    }
}
