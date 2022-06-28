using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
namespace APIAPP_WIN
{
    class Connection
    {

        #region "CONNECTION"
        public DataTable dtMapAcct = new DataTable();
        private static SqlConnection connect;

        public static SqlConnection Openconnect(string DBAPP = "")
        {
            DataTable dtConfigDB = new DataTable();
            try
            {
                string DB = "Source";
                if (dtConfigDB.Rows.Count == 0)
                    dtConfigDB = ReadConfig();

                else
                {
                }

                for (int i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                {
                    string ServerName = dtConfigDB.Rows[0].Field<string>("ServerName");
                    string DatabaseName = dtConfigDB.Rows[0].Field<string>("DBSRC"); // + "." + dtConfigDB.Rows[0].Field<string>("SRCFolder");
                    string DatabaseAPP = dtConfigDB.Rows[0].Field<string>("DB");
                    string UserName = dtConfigDB.Rows[0].Field<string>("User");
                    string Password = dtConfigDB.Rows[0].Field<string>("Pass");
                    string connectionStringSource = "Data Source= " + ServerName + " ; Initial Catalog=" + DatabaseName + " ;User ID=" + UserName + " ;Password= " + Password + ";Connect Timeout=0 ";
                    string connectionString = "Data Source= " + ServerName + " ; Initial Catalog=" + DatabaseAPP + " ;User ID=" + UserName + " ;Password= " + Password + ";Connect Timeout=0 ";

                    if (DB == "")
                        connect = new SqlConnection(connectionStringSource);
                    else
                        connect = new SqlConnection(connectionString);
                    if (connect.State == ConnectionState.Closed)
                        connect.Open();
                    else
                    {
                    }
                }


            }

            catch (Exception ex)
            {
                //WriteLog("Error 210 :" + ex.Message);
                WriteLog("Error 50 : " + ex.Message);
            }
            return connect;
        }

        public static SqlConnection OpenconnectDB()
        {
            DataTable dtConfigDB = new DataTable();
            try
            {
                string DB = "Source";

                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];



                string ServerName = ((Main)f).txtServer.Text;
                string DatabaseName = ((Main)f).txtDB.Text;
                string UserName = ((Main)f).txtUser.Text;
                string Password = ((Main)f).txtPassword.Text;
                string connectionStringSource = "Data Source= " + ServerName + " ; Initial Catalog=" + DatabaseName + " ;User ID=" + UserName + " ;Password= " + Password + ";Connect Timeout=0 ";

                if (DB == "Source")
                    connect = new SqlConnection(connectionStringSource);
                else
                    connect = new SqlConnection(connectionStringSource);
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                else
                {
                }


            }

            catch (Exception ex)
            {
                //WriteLog("Error 210 :" + ex.Message);
                WriteLog("Error 50 : " + ex.Message);
            }
            return connect;
        }
        public static void SaveConfigDB()
        {
            try
            {
                string FILE_text1 = Path.GetDirectoryName(Application.ExecutablePath) + @"\Configure\Config.ini";
                string[] aryText = new string[16];
                int i;

                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
                string txtServer = ((Main)f).txtServer.Text;
                string txtDB = ((Main)f).txtDB.Text;
                string txtUser = ((Main)f).txtUser.Text;
                string txtPassword = ((Main)f).txtPassword.Text;
                string txtDBSRC = ((Main)f).txtDBSRC.Text;
                string txtSRCFolder = ((Main)f).txtSRCFolder.Text;
                string txtAPPFolder = ((Main)f).txtAPPFolder.Text;

                string txtUploadAPI = ((Main)f).txtUploadAPI.Text;
                string txtDownloadAPI = ((Main)f).txtDownloadAPI.Text;
                string txtAPIKEY = ((Main)f).txtAPIKEY.Text;
                string txtUploadPath = ((Main)f).txtUploadPath.Text;
                string txtDownloadPath = ((Main)f).txtDownloadPath.Text;
                string txtSchedule = ((Main)f).txtSchedule.Text;
                string txtScaleX = ((Main)f).txtScaleX.Text;
                string txtScaleY = ((Main)f).txtScaleY.Text;

                string txtStamp = "";

                if (((Main)f).CBX_SignALL.CheckState == CheckState.Checked)
                {
                    txtStamp = "AP";
                }


                aryText[0] = "ServerName:" + EncryptDecrypt_Class.Encrypt(txtServer, "FMS123");
                aryText[1] = "User:" + EncryptDecrypt_Class.Encrypt(txtUser, "FMS123");
                aryText[2] = "Pass:" + EncryptDecrypt_Class.Encrypt(txtPassword, "FMS123");
                aryText[3] = "DBSRC:" + EncryptDecrypt_Class.Encrypt(txtDBSRC, "FMS123");
                aryText[4] = "SRCFolder:" + EncryptDecrypt_Class.Encrypt(txtSRCFolder, "FMS123");
                aryText[5] = "DB:" + EncryptDecrypt_Class.Encrypt(txtDB, "FMS123");
                aryText[6] = "APPFolder:" + EncryptDecrypt_Class.Encrypt(txtAPPFolder, "FMS123");

                aryText[7] = "UploadAPI:" + EncryptDecrypt_Class.Encrypt(txtUploadAPI, "FMS123");
                aryText[8] = "DownloadAPI:" + EncryptDecrypt_Class.Encrypt(txtDownloadAPI, "FMS123");
                aryText[9] = "APIKEY:" + EncryptDecrypt_Class.Encrypt(txtAPIKEY, "FMS123");
                aryText[10] = "UploadPath:" + EncryptDecrypt_Class.Encrypt(txtUploadPath, "FMS123");
                aryText[11] = "DownloadPath:" + EncryptDecrypt_Class.Encrypt(txtDownloadPath, "FMS123");
                aryText[12] = "Schedule:" + EncryptDecrypt_Class.Encrypt(txtSchedule, "FMS123");
                aryText[13] = "ScaleX:" + EncryptDecrypt_Class.Encrypt(txtScaleX, "FMS123");
                aryText[14] = "ScaleY:" + EncryptDecrypt_Class.Encrypt(txtScaleY, "FMS123");
                aryText[15] = "STAMP:" + EncryptDecrypt_Class.Encrypt(txtStamp, "FMS123");


                System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_text1);
                for (i = 0; i <= 15; i++)
                    objWriter.WriteLine(aryText[i]);
                objWriter.Close();
                Console.Read();
            }
            catch (Exception ex)
            {
                WriteLog("Error 165  SaveConfigDB(): " + ex.Message);
            }
        }
        public static DataTable ReadConfig()
        {
            Main frmMain = new Main();
            DataTable dtConfigDB = new DataTable();

            try
            {
                string filename = Path.GetDirectoryName(Application.ExecutablePath) + @"\Configure\Config.ini";
                using (StreamReader fileReader = new StreamReader(filename))
                {
                    // >> Read text push to datatable

                    string stringReader1;
                    stringReader1 = fileReader.ReadLine();
                    if (stringReader1 != "" && stringReader1 != null)
                    {
                        string[] SpVat1;
                        SpVat1 = stringReader1.Split(':');
                        string ServerName = "";
                        for (var i = 1; i <= SpVat1.Length - 1; i++)
                            ServerName = ServerName + SpVat1[i].ToString();
                        ServerName = EncryptDecrypt_Class.Decrypt(ServerName, "FMS123");

                        string stringReader2;
                        stringReader2 = fileReader.ReadLine();
                        string[] SpVat2;
                        SpVat2 = stringReader2.Split(':');
                        string User = "";
                        for (var i = 1; i <= SpVat2.Length - 1; i++)
                            User = User + SpVat2[i].ToString();
                        User = EncryptDecrypt_Class.Decrypt(User, "FMS123");

                        string stringReader3;
                        stringReader3 = fileReader.ReadLine();
                        string[] SpVat3;
                        SpVat3 = stringReader3.Split(':');
                        string Pass = "";
                        for (var i = 1; i <= SpVat3.Length - 1; i++)
                            Pass = Pass + SpVat3[i].ToString();
                        Pass = EncryptDecrypt_Class.Decrypt(Pass, "FMS123");

                        string stringReader4;
                        stringReader4 = fileReader.ReadLine();
                        string[] SpVat4;
                        SpVat4 = stringReader4.Split(':');
                        string DBSRC = "";
                        for (var i = 1; i <= SpVat4.Length - 1; i++)
                            DBSRC = DBSRC + SpVat4[i].ToString();
                        DBSRC = EncryptDecrypt_Class.Decrypt(DBSRC, "FMS123");

                        string stringReader5;
                        stringReader5 = fileReader.ReadLine();
                        string[] SpVat5;
                        SpVat5 = stringReader5.Split(':');
                        string SRCFOLDER = "";
                        for (var i = 1; i <= SpVat5.Length - 1; i++)
                            SRCFOLDER = SRCFOLDER + SpVat5[i].ToString();
                        SRCFOLDER = EncryptDecrypt_Class.Decrypt(SRCFOLDER, "FMS123");

                        string stringReader6;
                        stringReader6 = fileReader.ReadLine();
                        string[] SpVat6;
                        SpVat6 = stringReader6.Split(':');
                        string DB = "";
                        for (var i = 1; i <= SpVat6.Length - 1; i++)
                            DB = DB + SpVat6[i].ToString();
                        DB = EncryptDecrypt_Class.Decrypt(DB, "FMS123");

                        string stringReader7;
                        stringReader7 = fileReader.ReadLine();
                        string[] SpVat7;
                        SpVat7 = stringReader7.Split(':');
                        string APPFOLDER = "";
                        for (var i = 1; i <= SpVat7.Length - 1; i++)
                            APPFOLDER = APPFOLDER + SpVat7[i].ToString();
                        APPFOLDER = EncryptDecrypt_Class.Decrypt(APPFOLDER, "FMS123");

                        //-------API-----------------------

                        string stringAPIReader1;
                        stringAPIReader1 = fileReader.ReadLine();
                        string[] SpAPI1;
                        SpAPI1 = stringAPIReader1.Split(':');
                        string UPLOADAPI = "";
                        for (var i = 1; i <= SpAPI1.Length - 1; i++)
                            UPLOADAPI = UPLOADAPI + SpAPI1[i].ToString();
                        UPLOADAPI = EncryptDecrypt_Class.Decrypt(UPLOADAPI, "FMS123");

                        string stringAPIReader2;
                        stringAPIReader2 = fileReader.ReadLine();
                        string[] SpAPI2;
                        SpAPI2 = stringAPIReader2.Split(':');
                        string DOWNLOADAPI = "";
                        for (var i = 1; i <= SpAPI2.Length - 1; i++)
                            DOWNLOADAPI = DOWNLOADAPI + SpAPI2[i].ToString();
                        DOWNLOADAPI = EncryptDecrypt_Class.Decrypt(DOWNLOADAPI, "FMS123");



                        string stringAPIReader3;
                        stringAPIReader3 = fileReader.ReadLine();
                        string[] SpAPI3;
                        SpAPI3 = stringAPIReader3.Split(':');
                        string KEYAPI = "";
                        for (var i = 1; i <= SpAPI3.Length - 1; i++)
                            KEYAPI = KEYAPI + SpAPI3[i].ToString();
                        KEYAPI = EncryptDecrypt_Class.Decrypt(KEYAPI, "FMS123");

                        string stringAPIReader4;
                        stringAPIReader4 = fileReader.ReadLine();
                        string[] SpAPI4;
                        SpAPI4 = stringAPIReader4.Split(':');
                        string UPLOADPATH = "";
                        for (var i = 1; i <= SpAPI4.Length - 1; i++)
                            UPLOADPATH = UPLOADPATH + SpAPI4[i].ToString();
                        UPLOADPATH = EncryptDecrypt_Class.Decrypt(UPLOADPATH, "FMS123");

                        string stringAPIReader5;
                        stringAPIReader5 = fileReader.ReadLine();
                        string[] SpAPI5;
                        SpAPI5 = stringAPIReader5.Split(':');
                        string DOWNLOADPATH = "";
                        for (var i = 1; i <= SpAPI5.Length - 1; i++)
                            DOWNLOADPATH = DOWNLOADPATH + SpAPI5[i].ToString();
                        DOWNLOADPATH = EncryptDecrypt_Class.Decrypt(DOWNLOADPATH, "FMS123");


                        string stringSchedule1;
                        stringSchedule1 = fileReader.ReadLine();
                        string[] SpSchedule1;
                        SpSchedule1 = stringSchedule1.Split(':');
                        string ScheduleWeek = "";
                        for (var i = 1; i <= SpSchedule1.Length - 1; i++)
                            ScheduleWeek = ScheduleWeek + SpSchedule1[i].ToString();
                        ScheduleWeek = EncryptDecrypt_Class.Decrypt(ScheduleWeek, "FMS123");

                        string stringScale1;
                        stringScale1 = fileReader.ReadLine();
                        string[] SpScale1;
                        SpScale1 = stringScale1.Split(':');
                        string Scale1 = "";
                        for (var i = 1; i <= SpScale1.Length - 1; i++)
                            Scale1 = Scale1 + SpScale1[i].ToString();
                        Scale1 = EncryptDecrypt_Class.Decrypt(Scale1, "FMS123");

                        string stringScale2;
                        stringScale2 = fileReader.ReadLine();
                        string[] SpScale2;
                        SpScale2 = stringScale2.Split(':');
                        string Scale2 = "";
                        for (var i = 1; i <= SpScale2.Length - 1; i++)
                            Scale2 = Scale2 + SpScale2[i].ToString();
                        Scale2 = EncryptDecrypt_Class.Decrypt(Scale2, "FMS123");

                        string stringSTAMP;
                        stringSTAMP = fileReader.ReadLine();
                        string[] SpstringSTAMP;
                        SpstringSTAMP = stringSTAMP.Split(':');
                        string STAMP = "";
                        for (var i = 1; i <= SpstringSTAMP.Length - 1; i++)
                            STAMP = STAMP + SpstringSTAMP[i].ToString();
                        STAMP = EncryptDecrypt_Class.Decrypt(STAMP, "FMS123");



                        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                        ((Main)f).txtServer.Text = ServerName;
                        ((Main)f).txtDB.Text = DB;
                        ((Main)f).txtUser.Text = User;
                        ((Main)f).txtPassword.Text = Pass;
                        ((Main)f).txtDBSRC.Text = DBSRC;
                        ((Main)f).txtSRCFolder.Text = SRCFOLDER;
                        ((Main)f).txtAPPFolder.Text = APPFOLDER;


                        ((Main)f).txtUploadAPI.Text = UPLOADAPI;
                        ((Main)f).txtDownloadAPI.Text = DOWNLOADAPI;
                        ((Main)f).txtAPIKEY.Text = KEYAPI;
                        ((Main)f).txtUploadPath.Text = UPLOADPATH;
                        ((Main)f).txtDownloadPath.Text = DOWNLOADPATH;
                        ((Main)f).txtSchedule.Text = ScheduleWeek;
                        ((Main)f).txtScaleX.Text = Scale1;
                        ((Main)f).txtScaleY.Text = Scale2;

                        if (STAMP == "AP")
                        {
                            ((Main)f).CBX_SignALL.Checked = true;
                        }
                        else
                        {
                            ((Main)f).CBX_SignLast.Checked = true;
                        }


                        dtConfigDB.Rows.Clear();
                        dtConfigDB.Columns.Clear();

                        dtConfigDB.Columns.Add("ServerName");
                        dtConfigDB.Columns.Add("User");
                        dtConfigDB.Columns.Add("Pass");
                        dtConfigDB.Columns.Add("DBSRC");
                        dtConfigDB.Columns.Add("SRCFolder");
                        dtConfigDB.Columns.Add("DB");
                        dtConfigDB.Columns.Add("APPFolder");

                        dtConfigDB.Columns.Add("UploadAPI");
                        dtConfigDB.Columns.Add("DownloadAPI");
                        dtConfigDB.Columns.Add("APIKEY");
                        dtConfigDB.Columns.Add("UploadPath");
                        dtConfigDB.Columns.Add("DownloadPath");
                        dtConfigDB.Columns.Add("Schedule");
                        dtConfigDB.Columns.Add("ScaleX");
                        dtConfigDB.Columns.Add("ScaleY");
                        dtConfigDB.Columns.Add("STAMP");


                        string[] row = new string[] { ServerName, User, Pass, DBSRC, SRCFOLDER, DB, APPFOLDER, UPLOADAPI, DOWNLOADAPI, KEYAPI, UPLOADPATH, DOWNLOADPATH, ScheduleWeek, Scale1, Scale2, STAMP };
                        dtConfigDB.Rows.Add(row);

                    }
                    else

                        fileReader.Close();
                    fileReader.Dispose();
                }
            }

            catch (Exception ex)
            {
                WriteLog("Error 397 ReadConfig() : " + ex.Message);
            }
            return dtConfigDB;
        }

        #endregion

        #region "AUTHOR"
        public static DataTable READAUTHOR()
        {

            DataTable DT = new DataTable();

            //If DT.Columns.Count = 0 Or DT Is Nothing = True Then
            DT.Columns.Add("ID", typeof(int));
            DT.Columns.Add("USER");
            DT.Columns.Add("PASSWORD");
            // DT.Columns.Add("AUTHOR")

            // End If

            XmlDocument xmlDoc = new XmlDocument(); // For loading xml file to read

            string ImportFilename = Path.GetDirectoryName(Application.ExecutablePath) + @"\Configure\APPAUTHORIZED.xml";
            xmlDoc.Load(ImportFilename); // loading the xml file, insert your file here
            int RND = xmlDoc.Schemas.Count;


            // COUNT  

            XmlNodeList ArticleNodeList; // For getting the list of main/parent nodes
            ArticleNodeList = xmlDoc.GetElementsByTagName("row"); // Setting all <People> node to list
            foreach (XmlNode articlenode in ArticleNodeList) // Looping through <People> node           
                DT.Rows.Add();
            DT.Rows.Add();

            for (var J = 0; J <= DT.Rows.Count - 1; J++)
            {
                ArticleNodeList = xmlDoc.GetElementsByTagName("row"); // Setting all <People> node to list
                RND = 0;
                foreach (XmlNode articlenode in ArticleNodeList) // Looping through <People> node
                {
                    RND = RND + 1;
                    foreach (XmlNode basenode in articlenode) // Looping all <People> childnodes
                    {
                        string result = "";
                        result = basenode.Name; // use 
                        switch (result)
                        {
                            case "ID":
                                {
                                    if (J == RND)
                                        DT.Rows[J]["ID"] = basenode.InnerText;
                                    break;
                                }

                            case "USER":
                                {
                                    if (J == RND)
                                        DT.Rows[J]["USER"] = basenode.InnerText;
                                    break;
                                }

                            case "PASSWORD":
                                {
                                    if (J == RND)
                                        DT.Rows[J]["PASSWORD"] = basenode.InnerText;
                                    break;
                                }
                        }
                    }
                }
            }

            DT.Rows[0].Delete();

            return DT;

        }

        public static void SAVEAUTHOR(DataTable DTAPP)
        {
            try
            {
                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
                int ID = 0;
                string USER = ((Main)f).txtAuthorUser.Text;
                string PASSWORD = ((Main)f).txtAuthorPassword.Text;

                if (((Main)f).txtAuthorUserID.Text == "***NEW***" == true)
                {
                    if (((Main)f).txtAuthorPassword.Text.TrimEnd() == ((Main)f).txtConfirmPass.Text)
                    {
                        DataRow[] ROWID;
                        if (DTAPP.Rows.Count == 0)
                            ID = 1;
                        else
                        {
                            ROWID = DTAPP.Select("ID = MAX(ID)");
                            if (ROWID.Length > 0)
                                ID = System.Convert.ToInt32((ROWID[0].Field<int>("ID").ToString())) + 1;

                        }
                        string[] row = new string[] { ID.ToString(), USER, PASSWORD };
                        DTAPP.Rows.Add(row);
                        ((Main)f).txtAuthorUserID.Text = ID.ToString();
                    }
                    else
                    {
                        WriteLog("Warning! Please check and try again. Mismatch Password and Confirm Password. ");


                    }

                }
                else
                    // CASE EDIT 
                    for (var i = 0; i <= DTAPP.Rows.Count - 1; i++)
                    {
                        if (DTAPP.Rows[i].Field<int>("ID").ToString().TrimEnd() == ((Main)f).txtAuthorUserID.Text.TrimEnd())
                        {
                            DTAPP.Rows[i].Delete();
                            DTAPP.AcceptChanges();
                            ID = int.Parse(((Main)f).txtAuthorUserID.Text);
                            string[] row = new string[] { ID.ToString(), USER, PASSWORD };
                            DTAPP.Rows.Add(row);
                            break;
                        }
                    }

                DTAPP.DefaultView.Sort = "ID ASC";
                DTAPP = DTAPP.DefaultView.ToTable();
                XElement BOM = new XElement("BOM");
                XElement BO = new XElement("BO");

                if (DTAPP.Rows.Count != 0)
                {
                    XElement DocumentsLine = new XElement("Document_Lines");
                    for (var j = 0; j <= DTAPP.Rows.Count - 1; j++)
                    {
                        XElement Lrow = new XElement("row");
                        string vID = DTAPP.Rows[j].Field<int>("ID").ToString();
                        string vUSER = DTAPP.Rows[j].Field<string>("USER").ToString();
                        string vPASSWORD = DTAPP.Rows[j].Field<string>("PASSWORD").ToString();
                        // Dim vAuthor As String = DTAPP.Rows(j).Item("AUTHOR").ToString

                        XElement xmlID = new XElement("ID", vID);
                        XElement xmlUSER = new XElement("USER", vUSER);
                        XElement xmlPASSWORD = new XElement("PASSWORD", vPASSWORD);
                        // Dim xmlAuthor As XElement = New XElement("AUTHOR", vAuthor)

                        Lrow.Add(xmlID);
                        Lrow.Add(xmlUSER);
                        Lrow.Add(xmlPASSWORD);

                        DocumentsLine.Add(Lrow);
                    }
                    BO.Add(DocumentsLine);

                    BOM.Add(BO);
                    // Generate xml file
                    var reader = BOM.CreateReader();
                    reader.ReadInnerXml();
                    reader.MoveToContent();

                    XmlWriterSettings settingPath = new XmlWriterSettings();
                    settingPath.Indent = true;

                    string pathaddr;

                    pathaddr = Path.GetDirectoryName(Application.ExecutablePath) + @"\Configure\APPAUTHORIZED.xml";
                    System.IO.StreamWriter path = new StreamWriter(pathaddr);

                    using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(path))
                    {
                        writer.WriteStartDocument();
                        writer.WriteRaw(reader.ReadOuterXml());
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                WriteLog("ERROR 582 READAUTHOR() : " + ex.Message);
            }
        }

        #endregion

        #region "Log"
        public static void WriteLog(string strlog, string EXPORT = "")
        {
            try
            {
                // Generate log file
                string FILE_LOG = "";
                switch (EXPORT)
                {
                    case "":
                        {
                            FILE_LOG = Path.GetDirectoryName(Application.ExecutablePath) + @"\LOG.ini";
                            break;
                        }
                }

                var lineCount = File.ReadAllLines(FILE_LOG).Length;
                if (lineCount > 10000)
                {
                    lineCount = 1;
                    System.IO.File.WriteAllText(FILE_LOG, "Log");
                }

                // Read old log 
                //StreamReader reader = My.Computer.FileSystem.OpenTextFileReader(FILE_LOG);
                System.IO.StreamReader objReader;
                objReader = new System.IO.StreamReader(FILE_LOG);
                string oldLog = "";

                for (var cntLine = 0; cntLine <= lineCount - 1; cntLine++)
                    oldLog += objReader.ReadLine() + Environment.NewLine;
                objReader.Close();

                // Write log
                System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_LOG);
                objWriter.WriteLine(DateTime.Now + " " + strlog);
                objWriter.WriteLine(oldLog);
                objWriter.Close();
            }

            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
