namespace Clock
{
    partial class frmReminder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReminder));
            this.btnReminder = new System.Windows.Forms.Button();
            this.tmrRepeat = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnReminder
            // 
            this.btnReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReminder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReminder.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReminder.Location = new System.Drawing.Point(15, 17);
            this.btnReminder.Margin = new System.Windows.Forms.Padding(6);
            this.btnReminder.Name = "btnReminder";
            this.btnReminder.Size = new System.Drawing.Size(628, 258);
            this.btnReminder.TabIndex = 0;
            this.btnReminder.UseVisualStyleBackColor = true;
            this.btnReminder.Click += new System.EventHandler(this.btnReminder_Click);
            // 
            // tmrRepeat
            // 
            this.tmrRepeat.Enabled = true;
            this.tmrRepeat.Tick += new System.EventHandler(this.tmrRepeat_Tick);
            // 
            // frmReminder
            // 
            this.AcceptButton = this.btnReminder;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnReminder;
            this.ClientSize = new System.Drawing.Size(658, 288);
            this.Controls.Add(this.btnReminder);
            this.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reminder";
            this.Activated += new System.EventHandler(this.frmReminder_Activated);
            this.Load += new System.EventHandler(this.frmReminder_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReminder;
        private System.Windows.Forms.Timer tmrRepeat;
    }
}