using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FMSPRDOC
{
    public partial class FrmLogin : Form
    {
        private DateTime TargetDT;
        private TimeSpan CountDownFrom = TimeSpan.FromSeconds(20);
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            TargetDT = DateTime.Now.Add(CountDownFrom);
            timer1.Start();
            APIAPP_WIN.Process CallProcess = new APIAPP_WIN.Process();
            CallProcess.DisableTimeschedule();
           if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            TimeSpan ts = TargetDT.Subtract(DateTime.Now);
            if (ts.TotalMilliseconds > 0)
                txtTimer.Text = ts.ToString(@"mm\:ss");
            else
            {
                txtTimer.Text = "00:00";
                timer1.Stop();
                //Continue Sign document Process
                APIAPP_WIN.Main showMAIN = new APIAPP_WIN.Main();
                showMAIN.Show();
                showMAIN.WindowState = FormWindowState.Minimized;
                showMAIN.BTN_SIGN.PerformClick();
                //enable task
                APIAPP_WIN.Process CallProcess = new APIAPP_WIN.Process();
                CallProcess.EnableTimeschedule();
                Application.Exit();

            }
        }

        private void BTNLOGIN_OK_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            DataTable DTAUTH = new DataTable();
            DTAUTH = APIAPP_WIN.Connection.READAUTHOR();
            bool SUCC = false;
            if (DTAUTH.Rows.Count > 0)
            {
                for (var i = 0; i <= DTAUTH.Rows.Count - 1; i++)
                {
                    string vUSER = DTAUTH.Rows[i].Field<string>("USER").ToString();
                    string vPASSWORD = DTAUTH.Rows[i].Field<string>("PASSWORD").ToString();

                    if (vUSER == txtUserID.Text)
                    {
                        if (vPASSWORD == txtPassword.Text)
                        {
                            SUCC = true;
                        }

                    }


                }
                if (SUCC == true)
                {
                    APIAPP_WIN.Main showMAIN = new APIAPP_WIN.Main();
                    showMAIN.Show();
                    showMAIN.txtLOGIN.Text = txtUserID.Text;
                    FrmLogin Form = new FrmLogin();
                    //showLogin.Hide();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login failed");
                }
            }
            else
            {
                APIAPP_WIN.Main showMAIN = new APIAPP_WIN.Main();
                showMAIN.Show();
            }

        }


        private void BTNLOGIN_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
