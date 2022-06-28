using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace APIAPP_WIN
{
    public partial class FrmGENPDF : Form
    {

        struct Parameter
        {
            public string ParameterName;
            public string ParameterValue;

        }

        struct ReportCondition
        {
            public string ConnectionString;
            public string ReportFile;
            public string Fomula;

        }
        ReportCondition ReportAttribute;
        Parameter[] ReportAttributeParameter = new Parameter[1];
        internal string conStr;
        internal SqlConnection connection;
        internal DataSet dataSt;
        internal SqlDataAdapter adapter;
       
        public FrmGENPDF()
        {
            InitializeComponent();
        }

        private void FrmGENPDF_Load(object sender, EventArgs e)
        {
            try
            {
                //1. Gen PDF 
                string PRNUM;
                DateTime VDATEFROM;
                DateTime VDATETO;



                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                VDATEFROM = ((Main)f).DATEFROM.Value;
                VDATETO = ((Main)f).DATETO.Value;
                DataTable dt = Master.GETDATA_PREQUIS(VDATEFROM, VDATETO);
                //Condition Last User = '01445'
                dt = Process.PROCESSCONDITION(dt);

                for (var i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string genFILENAME = "";
                    string FILENAME = "";
                    PRNUM = "";
                    PRNUM = dt.Rows[i].Field<string>("PSHNUM_0").ToString().TrimEnd();
                    string GENDOC = GenReportValidate_Load(PRNUM);
                    string vdocname = Path.GetFileName(GENDOC);
                    PdfReader pdfReader = new PdfReader(GENDOC);
                    pdfReader.Dispose();
                    pdfReader.Close();
                    int numberOfPages = pdfReader.NumberOfPages;
                    string vpageNum = numberOfPages.ToString();
                    string vStamp = "";
                    if (((Main)f).CBX_SignALL.CheckState == CheckState.Checked)
                    {
                        vStamp = "AP";
                    }

                    dt.Rows[i][8] = GENDOC;
                    dt.Rows[i][9] = vdocname;
                    dt.Rows[i][10] = vpageNum;
                    dt.Rows[i][11] = ((Main)f).txtScaleX.Text;
                    dt.Rows[i][12] = ((Main)f).txtScaleY.Text;
                    dt.Rows[i][13] = vStamp;


                    GENXMLSIGN(PRNUM, dt);


                    //2.call Insert 

                    Dataclass.INSERTFMSPR(dt);

                    //3.SIGN
                    //Process callprocess = new Process();
                    //callprocess.API_SIGN();

                    genFILENAME = Process.API_SIGN();

                    if (genFILENAME != "")
                    {
                        //4.UPLOAD SAGE X3 

                        // COPY GEN EXPORT FILE 
                        FILENAME = Path.GetFileName(genFILENAME);

                        // INSERT SAGE X3 

                        Dataclass.INSERTAOBJTXT(PRNUM, FILENAME);
                        // MOVE FILE TO SAGE PATH 

                        //System.IO.File.Move(genFILENAME, "C:" + @"\Sage\SAGEX3\folders\DEV03\" + FILENAME);
                        string X3PATH = ((Main)f).txtDownloadPath.Text.TrimEnd();
                        System.IO.File.Move(genFILENAME, X3PATH + @"\ATT\API\" + FILENAME);

                    }

                    else
                    {

                    }

                }

                Close();
                
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 110 FrmGENPDF_Load() " + ex.Message);

            }

        }

        public static string GenReportValidate_Load(string PRNUM)
        {
            string PDF_ExportPathAUTO = "";
            System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
            DataTable dtConfigDB = new DataTable();

            FrmGENPDF clsRptViewer = new FrmGENPDF();
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            if (dtConfigDB.Rows.Count == 0)
                dtConfigDB = Connection.ReadConfig();
            else
            {
            }

            string strcon = "Data Source= " + dtConfigDB.Rows[0].Field<string>("ServerName") + "  ;Initial Catalog=" + dtConfigDB.Rows[0].Field<string>("DBSRC") + " ;User ID=" + dtConfigDB.Rows[0].Field<string>("User") + " ;Password= " + dtConfigDB.Rows[0].Field<string>("Pass") + ";Connect Timeout=0 ";

            string conStr = "Data Source= " + dtConfigDB.Rows[0].Field<string>("ServerName") + ";Initial Catalog= " + dtConfigDB.Rows[0].Field<string>("DBSRC") + ";User ID= " + dtConfigDB.Rows[0].Field<string>("User") + ";Password= " + dtConfigDB.Rows[0].Field<string>("Pass") + ";Connect Timeout=0";
            SqlConnection connection = new SqlConnection(conStr);


            System.Data.SqlClient.SqlConnectionStringBuilder conStrREPORT = new System.Data.SqlClient.SqlConnectionStringBuilder(conStr);

            rpt.Load(Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\BONDA.rpt");



            DateTime DATEF = DateTime.Now;

            try
            {

                clsRptViewer.ReportAttributeParameter[0].ParameterName = "PRNUM";
                clsRptViewer.ReportAttributeParameter[0].ParameterValue = PRNUM;

            }

            catch (Exception ex)
            {
                Connection.WriteLog("Error 275 (BTN_GET_Click): " + ex.Message);
            }



            CrystalDecisions.Shared.TableLogOnInfo crTableLogonInfo;
            CrystalDecisions.Shared.ConnectionInfo ConnInfo = new CrystalDecisions.Shared.ConnectionInfo();

            ConnInfo.ServerName = conStrREPORT.DataSource;
            ConnInfo.UserID = conStrREPORT.UserID;
            ConnInfo.Password = conStrREPORT.Password;
            ConnInfo.DatabaseName = conStrREPORT.InitialCatalog;
            ConnInfo.IntegratedSecurity = false;

            try
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in rpt.Database.Tables)
                {
                    crTableLogonInfo = crTable.LogOnInfo;
                    crTableLogonInfo.ConnectionInfo = ConnInfo;
                    crTable.ApplyLogOnInfo(crTableLogonInfo);
                }
                if ((clsRptViewer.ReportAttributeParameter != null))
                {
                    foreach (Parameter obj in clsRptViewer.ReportAttributeParameter)
                    {

                        rpt.SetParameterValue(obj.ParameterName, obj.ParameterValue);
                    }
                }
                // Open Crystal report viewer' 
                using (System.Windows.Forms.Form objForm = new System.Windows.Forms.Form())
                {
                    objForm.StartPosition = FormStartPosition.CenterScreen;
                    objForm.WindowState = FormWindowState.Maximized;

                    using (CrystalDecisions.Windows.Forms.CrystalReportViewer rptViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer())
                    {

                        // rptViewer.DisplayGroupTree = False
                        rptViewer.ShowCloseButton = false;
                        rptViewer.ShowGroupTreeButton = true;
                        rptViewer.ShowTextSearchButton = true;
                        rptViewer.ShowZoomButton = true;

                        rptViewer.Dock = DockStyle.Fill;

                        objForm.Controls.Add(rptViewer);

                        rptViewer.ReportSource = rpt;

                        //Show Report
                        //objForm.ShowDialog();

                        // Export to PDF 

                        string PATH = ((Main)f).txtUploadPath.Text;

                        PDF_ExportPathAUTO = PATH + @"\Export\" + PRNUM.TrimEnd() + ".pdf";
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, PDF_ExportPathAUTO);
                        rpt.Dispose();
                        rpt.Close();
                        objForm.Dispose();
                        objForm.Close();

                        rptViewer.Dispose();


                    }

                }

            }

            catch (Exception ex)
            {
                Connection.WriteLog("Error 175 : " + ex.Message);
            }
            return PDF_ExportPathAUTO;
        }

        public void GENXML(string PRNUM, DataTable dt)
        {
            try
            {
                for (var i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    XElement task = new XElement("task");
                    /////////////////////////
                    XElement taskName = new XElement("taskName", PRNUM);


                    /////////////////////////////////

                    string vsenderEmail = dt.Rows[i].Field<string>("SENDEREMAIL").ToString().TrimEnd();
                    XElement senderEmail = new XElement("senderEmail", vsenderEmail);

                    ////////////////apvLevel
                    XElement apvLevel = new XElement("apvLevel");
                    XElement level = new XElement("level");

                    XElement order = new XElement("order", 1);

                    XElement numOfSigner = new XElement("numOfSigner", 1);


                    XElement signerList = new XElement("signerList");
                    XElement signer = new XElement("signer");

                    string vsignerName = dt.Rows[i].Field<string>("SIGNERNAME").ToString().TrimEnd();
                    XElement signerName = new XElement("signerName", vsignerName);

                    string vsignerEmail = dt.Rows[i].Field<string>("SIGNEREMAIL").ToString().TrimEnd();
                    XElement signerEmail = new XElement("signerEmail", vsignerEmail);

                    XElement action = new XElement("action", "Need to Sign");

                    signer.Add(signerName);
                    signer.Add(signerEmail);
                    signer.Add(action);
                    signerList.Add(signer);

                    level.Add(order);
                    level.Add(numOfSigner);
                    level.Add(signerList);

                    apvLevel.Add(level);

                    ////////////////docList
                    XElement docList = new XElement("docList");
                    XElement doc = new XElement("doc");


                    string PATHDOCNAME = dt.Rows[i].Field<string>("DOCNAME").ToString().TrimEnd();
                    string vdocname = Path.GetFileName(PATHDOCNAME);
                    XElement docName = new XElement("docName", vdocname);


                    XElement signItems = new XElement("signItems");
                    XElement item = new XElement("item");

                    string vdocsigner = dt.Rows[i].Field<string>("SIGNEREMAIL").ToString().TrimEnd();
                    XElement docsigner = new XElement("signer", vdocsigner);


                    string vfieldType = "signature";
                    XElement fieldType = new XElement("fieldType", vfieldType);


                    //PdfReader pdfReader = new PdfReader(PATHDOCNAME);
                    //int numberOfPages = pdfReader.NumberOfPages;
                    //string vpageNum = numberOfPages.ToString();

                    string vpageNum = dt.Rows[i].Field<string>("PAGENUM").ToString().TrimEnd();

                    XElement pageNum = new XElement("pageNum", vpageNum);

                    string vStamp = dt.Rows[i].Field<string>("STAMP").ToString().TrimEnd();

                    System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                    //string vcoordinateX = ((Main)f).txtScaleX.Text;
                    string vcoordinateX = dt.Rows[i].Field<string>("X_AXIS").ToString().TrimEnd();
                    XElement coordinateX = new XElement("coordinateX", vcoordinateX);


                    //string vcoordinateY = ((Main)f).txtScaleY.Text;
                    string vcoordinateY = dt.Rows[i].Field<string>("Y_AXIS").ToString().TrimEnd();
                    XElement coordinateY = new XElement("coordinateY", vcoordinateY);


                    item.Add(docsigner);
                    item.Add(fieldType);
                    item.Add(pageNum);
                    if (vStamp == "AP")
                    {
                        XElement pageType = new XElement("pageType", vStamp);
                        item.Add(pageType);
                    }
                    item.Add(coordinateX);
                    item.Add(coordinateY);

                    signItems.Add(item);

                    doc.Add(docName);
                    doc.Add(signItems);

                    docList.Add(doc);


                    ////////task
                    task.Add(taskName);
                    task.Add(senderEmail);
                    task.Add(apvLevel);
                    task.Add(docList);


                    // Generate xml file
                    var reader = task.CreateReader();
                    reader.ReadInnerXml();
                    reader.MoveToContent();


                    XmlWriterSettings settingPath = new XmlWriterSettings();
                    settingPath.Indent = true;

                    string pathaddr;
                    string PATH = ((Main)f).txtUploadPath.Text;
                    pathaddr = PATH + @"\Export\" + PRNUM.TrimEnd() + ".xml";
                    System.IO.StreamWriter path = new StreamWriter(pathaddr);

                    using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(path))
                    {
                        writer.WriteStartDocument();
                        writer.WriteRaw(reader.ReadOuterXml());

                    }
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 300: " + ex.Message, "EXPORT");
            }
        }


        public void GENXMLSIGN(string PRNUM, DataTable dt)
        {
            try
            {
                for (var i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (PRNUM == dt.Rows[i].Field<string>("PSHNUM_0").ToString().TrimEnd())
                    {

                        XElement task = new XElement("task");
                        /////////////////////////
                        XElement taskName = new XElement("taskName", PRNUM);


                        /////////////////////////////////

                        string vsenderEmail = dt.Rows[i].Field<string>("SENDEREMAIL").ToString().TrimEnd();
                        XElement senderEmail = new XElement("senderEmail", vsenderEmail);

                        ////////////////apvLevel
                        XElement apvLevel = new XElement("apvLevel");
                        XElement level = new XElement("level");

                        XElement order = new XElement("order", 1);

                        XElement numOfSigner = new XElement("numOfSigner", 1);


                        XElement signerList = new XElement("signerList");
                        XElement signer = new XElement("signer");

                        string vsignerName = dt.Rows[i].Field<string>("SIGNERNAME").ToString().TrimEnd();
                        XElement signerName = new XElement("signerName", vsignerName);

                        string vsignerEmail = dt.Rows[i].Field<string>("SIGNEREMAIL").ToString().TrimEnd();

                        XElement signerEmail = new XElement("signerEmail", vsignerEmail);

                        XElement action = new XElement("action", "Need to Sign");

                        signer.Add(signerName);
                        signer.Add(signerEmail);
                        signer.Add(action);
                        signerList.Add(signer);

                        level.Add(order);
                        level.Add(numOfSigner);
                        level.Add(signerList);

                        apvLevel.Add(level);

                        ////////////////docList
                        XElement docList = new XElement("docList");
                        XElement doc = new XElement("doc");


                        string PATHDOCNAME = dt.Rows[i].Field<string>("DOCNAME").ToString().TrimEnd();
                        string vdocname = Path.GetFileName(PATHDOCNAME);
                        XElement docName = new XElement("docName", vdocname);


                        XElement signItems = new XElement("signItems");
                        XElement item = new XElement("item");

                        string vdocsigner = dt.Rows[i].Field<string>("SIGNEREMAIL").ToString().TrimEnd();
                        XElement docsigner = new XElement("signer", vdocsigner);


                        string vfieldType = "signature";
                        XElement fieldType = new XElement("fieldType", vfieldType);


                        PdfReader pdfReader = new PdfReader(PATHDOCNAME);
                        int numberOfPages = pdfReader.NumberOfPages;
                        string vpageNum = numberOfPages.ToString();


                        XElement pageNum = new XElement("pageNum", vpageNum);
                        string vStamp = dt.Rows[i].Field<string>("STAMP").ToString().TrimEnd();

                        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                        string vcoordinateX = ((Main)f).txtScaleX.Text;

                        XElement coordinateX = new XElement("coordinateX", vcoordinateX);


                        string vcoordinateY = ((Main)f).txtScaleY.Text;

                        XElement coordinateY = new XElement("coordinateY", vcoordinateY);


                        //item.Add(docsigner);
                        item.Add(fieldType);
                        item.Add(pageNum);

                        if (vStamp == "AP")
                        {
                            XElement pageType = new XElement("pageType", vStamp);
                            item.Add(pageType);
                        }

                        item.Add(coordinateX);
                        item.Add(coordinateY);

                        signItems.Add(item);

                        doc.Add(docName);
                        doc.Add(signItems);

                        docList.Add(doc);


                        ////////task
                        task.Add(taskName);
                        task.Add(signerName);
                        task.Add(signerEmail);
                        //task.Add(senderEmail);
                        //task.Add(apvLevel);
                        task.Add(docList);


                        // Generate xml file
                        var reader = task.CreateReader();
                        reader.ReadInnerXml();
                        reader.MoveToContent();


                        XmlWriterSettings settingPath = new XmlWriterSettings();
                        settingPath.Indent = true;

                        string pathaddr;
                        string PATH = ((Main)f).txtUploadPath.Text;
                        pathaddr = PATH + @"\Export\" + PRNUM.TrimEnd() + ".xml";
                        System.IO.StreamWriter path = new StreamWriter(pathaddr);

                        using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(path))
                        {
                            writer.WriteStartDocument();
                            writer.WriteRaw(reader.ReadOuterXml());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 300: " + ex.Message, "EXPORT");
            }
        }
    }
}
