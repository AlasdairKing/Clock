namespace Clock
{
    partial class frmSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchedule));
            this.lblTime = new System.Windows.Forms.Label();
            this.lstTime = new System.Windows.Forms.ListBox();
            this.lblReminder = new System.Windows.Forms.Label();
            this.txtReminder = new System.Windows.Forms.TextBox();
            this.btnDeleteReminder = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(59, 25);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "&Time";
            // 
            // lstTime
            // 
            this.lstTime.FormattingEnabled = true;
            this.lstTime.ItemHeight = 25;
            this.lstTime.Location = new System.Drawing.Point(17, 37);
            this.lstTime.Name = "lstTime";
            this.lstTime.Size = new System.Drawing.Size(401, 29);
            this.lstTime.TabIndex = 1;
            this.lstTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstTime_KeyDown);
            // 
            // lblReminder
            // 
            this.lblReminder.AutoSize = true;
            this.lblReminder.Location = new System.Drawing.Point(12, 81);
            this.lblReminder.Name = "lblReminder";
            this.lblReminder.Size = new System.Drawing.Size(104, 25);
            this.lblReminder.TabIndex = 2;
            this.lblReminder.Text = "&Reminder";
            // 
            // txtReminder
            // 
            this.txtReminder.Location = new System.Drawing.Point(17, 109);
            this.txtReminder.Name = "txtReminder";
            this.txtReminder.Size = new System.Drawing.Size(401, 31);
            this.txtReminder.TabIndex = 3;
            this.txtReminder.TextChanged += new System.EventHandler(this.txtReminder_TextChanged);
            // 
            // btnDeleteReminder
            // 
            this.btnDeleteReminder.Location = new System.Drawing.Point(17, 159);
            this.btnDeleteReminder.Name = "btnDeleteReminder";
            this.btnDeleteReminder.Size = new System.Drawing.Size(238, 44);
            this.btnDeleteReminder.TabIndex = 4;
            this.btnDeleteReminder.Text = "Delete reminder";
            this.btnDeleteReminder.UseVisualStyleBackColor = true;
            this.btnDeleteReminder.Click += new System.EventHandler(this.btnDeleteReminder_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(448, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 50);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(448, 68);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 50);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSchedule
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(568, 216);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDeleteReminder);
            this.Controls.Add(this.txtReminder);
            this.Controls.Add(this.lblReminder);
            this.Controls.Add(this.lstTime);
            this.Controls.Add(this.lblTime);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmSchedule";
            this.Text = "Schedule Reminder";
            this.Activated += new System.EventHandler(this.frmSchedule_Activated);
            this.Load += new System.EventHandler(this.frmSchedule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ListBox lstTime;
        private System.Windows.Forms.Label lblReminder;
        private System.Windows.Forms.TextBox txtReminder;
        private System.Windows.Forms.Button btnDeleteReminder;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}