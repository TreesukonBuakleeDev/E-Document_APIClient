namespace FMSPRDOC
{
    partial class FrmLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BTNLOGIN_OK = new System.Windows.Forms.Button();
            this.BTNLOGIN_Cancel = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "User ID : ";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtUserID.Location = new System.Drawing.Point(126, 39);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(208, 27);
            this.txtUserID.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtPassword.Location = new System.Drawing.Point(126, 89);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(208, 27);
            this.txtPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password : ";
            // 
            // BTNLOGIN_OK
            // 
            this.BTNLOGIN_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.BTNLOGIN_OK.Location = new System.Drawing.Point(362, 34);
            this.BTNLOGIN_OK.Name = "BTNLOGIN_OK";
            this.BTNLOGIN_OK.Size = new System.Drawing.Size(123, 37);
            this.BTNLOGIN_OK.TabIndex = 4;
            this.BTNLOGIN_OK.Text = "OK";
            this.BTNLOGIN_OK.UseVisualStyleBackColor = true;
            this.BTNLOGIN_OK.Click += new System.EventHandler(this.BTNLOGIN_OK_Click);
            // 
            // BTNLOGIN_Cancel
            // 
            this.BTNLOGIN_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.BTNLOGIN_Cancel.Location = new System.Drawing.Point(362, 89);
            this.BTNLOGIN_Cancel.Name = "BTNLOGIN_Cancel";
            this.BTNLOGIN_Cancel.Size = new System.Drawing.Size(123, 37);
            this.BTNLOGIN_Cancel.TabIndex = 5;
            this.BTNLOGIN_Cancel.Text = "Cancel";
            this.BTNLOGIN_Cancel.UseVisualStyleBackColor = true;
            this.BTNLOGIN_Cancel.Click += new System.EventHandler(this.BTNLOGIN_Cancel_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // txtTimer
            // 
            this.txtTimer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTimer.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtTimer.Location = new System.Drawing.Point(194, 8);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.ReadOnly = true;
            this.txtTimer.Size = new System.Drawing.Size(100, 15);
            this.txtTimer.TabIndex = 6;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 148);
            this.Controls.Add(this.txtTimer);
            this.Controls.Add(this.BTNLOGIN_Cancel);
            this.Controls.Add(this.BTNLOGIN_OK);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.label1);
            this.Name = "FrmLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BTNLOGIN_OK;
        private System.Windows.Forms.Button BTNLOGIN_Cancel;
        public System.Windows.Forms.TextBox txtTimer;
        public System.Windows.Forms.Timer timer1;
    }
}