using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace APIAPP_WIN
{
    public partial class Main : Form
    {
        public SqlConnection connections { get; private set; }

        public Main()
        {
            InitializeComponent();

        }
        private void Main_Load(object sender, EventArgs e)
        {
            Connection.Openconnect();
            //Dataclass.CREATEFMSPR();
            DATEFROM.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DATETO.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            //DATETO.Value = new DateTime(DATETO.Value.Year, DATETO.Value.Month, 1).AddMonths(1).AddDays(-1);
        }

        private void BTN_UPDATE_Click(object sender, EventArgs e)
        {
            //POST UPDATE STOCK 
            //Process callprocess = new Process();
            //callprocess.GETMETHODTEST_UPDATESTOCK();
            Process callprocess = new Process();
            callprocess.API_DOWNLOAD();
        }
        private void BTN_UPLOAD_Click(object sender, EventArgs e)
        {
            Process callprocess = new Process();
            callprocess.API_UPLOAD();
        }
        private void BTN_GETDATA_Click(object sender, EventArgs e)
        {
            DateTime VDATEFROM;
            DateTime VDATETO;
            VDATEFROM = DATEFROM.Value;
            VDATETO = DATETO.Value;

            DataTable dtPR = Master.GETDATA_PREQUIS(VDATEFROM, VDATETO);

            dtPR = Process.PROCESSCONDITION(dtPR);

            DGVMAIN.DataSource = dtPR.DefaultView;
           


            //FrmGENPDF FrmPrintGENPDF = new FrmGENPDF();
            //FrmPrintGENPDF.Show();

        }

        private void BTN_SaveDB_Click(object sender, EventArgs e)
        {
            Connection.SaveConfigDB();
            Process.timeschedule();
            Dataclass.CREATEFMSPR();
            Dataclass.CREATEFMSPRLOG();
           
            Process.CreateFolder(txtUploadPath.Text + @"\Export");
            Process.CreateFolder(txtUploadPath.Text + @"\API");
            Process.CreateFolder(txtUploadPath.Text + @"\Backup");
        

        }

        private void BTN_AUTHNEW_Click(object sender, EventArgs e)
        {
            txtAuthorUserID.Text = "***NEW***";
            txtAuthorUser.Text = "";
            txtAuthorPassword.Text = "";
            Lb_ConfirmPass.Visible = true;
            txtConfirmPass.Visible = true;
            txtAuthorUser.ReadOnly = false;
            txtAuthorPassword.ReadOnly = false;
            txtConfirmPass.ReadOnly = false;
        }

        private void BTN_AUTHEDIT_Click(object sender, EventArgs e)
        {
            txtAuthorUser.ReadOnly = false;
            txtAuthorPassword.ReadOnly = false;
            txtConfirmPass.ReadOnly = false;
        }

        private void BTN_AUTHDEL_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogOK = MessageBox.Show("Do you want to delete user ?", "Waring ", MessageBoxButtons.OKCancel);
                if (dialogOK == DialogResult.OK)
                {
                    DataTable DTAPP = Connection.READAUTHOR();
                    for (var i = 0; i <= DTAPP.Rows.Count - 1; i++)
                    {
                        if (DTAPP.Rows[i].Field<int>("ID").ToString().TrimEnd() == txtAuthorUserID.Text.TrimEnd())
                        {
                            DTAPP.Rows[i].Delete();
                            DTAPP.AcceptChanges();
                            break;
                        }
                    }

                    Connection.SAVEAUTHOR(DTAPP);
                    txtAuthorUserID.Text = "";
                    txtAuthorUser.Text = "";
                    txtAuthorPassword.Text = "";
                    txtConfirmPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 405 " + ex.Message);
                //WriteLog("Error 180" + ex.Message);
            }
        }

        private void BTN_AuthorNext_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTAPP = new DataTable();
                using (DTAPP = Connection.READAUTHOR())
                {
                    DTAPP.DefaultView.Sort = "ID ASC";
                    DTAPP = DTAPP.DefaultView.ToTable();
                    if (DTAPP.Rows.Count > 0)
                    {
                        for (var i = 0; i <= DTAPP.Rows.Count - 1; i++)
                        {
                            if (txtAuthorUserID.Text == "")
                            {
                                txtAuthorUserID.Text = DTAPP.Rows[0].Field<int>("ID").ToString();
                                txtAuthorUser.Text = DTAPP.Rows[0].Field<string>("USER").ToString();
                                txtAuthorPassword.Text = DTAPP.Rows[0].Field<string>("PASSWORD").ToString();
                            }
                            else
                            {
                                // Dim index As Integer
                                var rowIndex = DTAPP.AsEnumerable().Select(r => r.Field<int>("ID")).ToList().FindIndex(col => col == int.Parse(txtAuthorUserID.Text));
                                //Process.WriteLog(rowIndex)

                                switch (rowIndex + 1)
                                {
                                    case object _ when rowIndex + 1 < DTAPP.Rows.Count - 1:
                                        {
                                            txtAuthorUserID.Text = DTAPP.Rows[rowIndex + 1].Field<int>("ID").ToString();
                                            txtAuthorUser.Text = DTAPP.Rows[rowIndex + 1].Field<string>("USER").ToString();
                                            txtAuthorPassword.Text = DTAPP.Rows[rowIndex + 1].Field<string>("PASSWORD").ToString();
                                            break;
                                        }

                                    case object _ when rowIndex + 1 == DTAPP.Rows.Count - 1:
                                        {
                                            txtAuthorUserID.Text = DTAPP.Rows[rowIndex + 1].Field<int>("ID").ToString();
                                            txtAuthorUser.Text = DTAPP.Rows[rowIndex + 1].Field<string>("USER").ToString();
                                            txtAuthorPassword.Text = DTAPP.Rows[rowIndex + 1].Field<string>("PASSWORD").ToString();
                                            break;
                                        }

                                    default:
                                        {
                                            txtAuthorUserID.Text = DTAPP.Rows[0].Field<int>("ID").ToString();
                                            txtAuthorUser.Text = DTAPP.Rows[0].Field<string>("USER").ToString();
                                            txtAuthorPassword.Text = DTAPP.Rows[0].Field<string>("PASSWORD").ToString();
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    }
                    else
                        Connection.WriteLog("Records Not found");
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 355 : " + ex.Message);
            }
        }

        private void txtDBSRC_MouseClick(object sender, MouseEventArgs e)
        {
            txtDBSRC.Items.Clear();
            DataTable dtGETDB = Master.GET_DB();
            for (var i = 0; i <= dtGETDB.Rows.Count - 1; i++)
            {
                var DBNAME = dtGETDB.Rows[i].Field<string>("NAME").ToString();

                txtDBSRC.Items.Add(DBNAME);
            }

        }

        private void txtDB_MouseClick(object sender, MouseEventArgs e)
        {
            txtDB.Items.Clear();
            DataTable dtGETDB = Master.GET_DB();
            for (var i = 0; i <= dtGETDB.Rows.Count - 1; i++)
            {
                var DBNAME = dtGETDB.Rows[i].Field<string>("NAME").ToString();

                txtDB.Items.Add(DBNAME);
            }

        }

        private void BTN_BrowseUP_Click(object sender, EventArgs e)
        {

        }

        private void BTN_BrowseDown_Click(object sender, EventArgs e)
        {

        }

        private void BTN_SIGN_Click(object sender, EventArgs e)
        {
            try
            {
                string PRNUM = "";
                string FILENAME = "";
                //1.GET DATA

                DateTime VDATEFROM;
                DateTime VDATETO;
                VDATEFROM = DATEFROM.Value;
                VDATETO = DATETO.Value;

                DataTable dtPR = Master.GETDATA_PREQUIS(VDATEFROM, VDATETO);

                dtPR = Process.PROCESSCONDITION(dtPR);

                DGVMAIN.DataSource = dtPR.DefaultView;

                
               

                FrmGENPDF FrmPrintGENPDF = new FrmGENPDF();
                FrmPrintGENPDF.Show();

                if (WindowState != FormWindowState.Minimized)
                {
                    MessageBox.Show("Finished. Please Checking the result.");
                }
               

            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 250  BTN_SIGN_Click() " + ex.Message);
               
            }
        }

        private void CBX_SignALL_CheckedChanged(object sender, EventArgs e)
        {
            switch (CBX_SignALL.CheckState)
            {
                case CheckState.Checked:
                    {
                        CBX_SignLast.CheckState = CheckState.Unchecked;
                        break;
                    }
            }
        }

        private void CBX_SignLast_CheckedChanged(object sender, EventArgs e)
        {
            switch (CBX_SignALL.CheckState)
            {
                case CheckState.Checked:
                    {
                        CBX_SignALL.CheckState = CheckState.Unchecked;
                        break;
                    }
            }
        }

        private void BTNLOG_COMPLETE_CheckedChanged(object sender, EventArgs e)
        {
            if (BTNLOG_COMPLETE.Checked == true)
            {
              DataTable dtLogDisplay =  Master.GETDATA_FMSPRLOG(DATEFROM.Value, DATETO.Value, "Complete");
                DGV_LOG.DataSource = dtLogDisplay.DefaultView;
            }
        }

        private void BTNLOG_ERR_CheckedChanged(object sender, EventArgs e)
        {
            if (BTNLOG_ERR.Checked == true)
            {
                DataTable dtLogDisplay = Master.GETDATA_FMSPRLOG(DATEFROM.Value, DATETO.Value, "Incomplete");
                DGV_LOG.DataSource = dtLogDisplay.DefaultView;
            }
        }

        private void DATETO_ValueChanged(object sender, EventArgs e)
        {
            //Master.GETDATA_FMSPRLOG(DATEFROM.Value, DATETO.Value, "");
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            APIAPP_WIN.Process CallProcess = new APIAPP_WIN.Process();
            CallProcess.EnableTimeschedule();
            Application.Exit();
        }

       
    }


}
