using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Security.Cryptography;
using RestSharp;
using System.Data;
using Microsoft.Win32.TaskScheduler;
using Task = Microsoft.Win32.TaskScheduler.Task;

namespace APIAPP_WIN
{
    class Process
    {
        private int i;

        public static DataTable dtLOG = new DataTable();
        public HttpStatusCode OK { get; private set; }


        #region "DATA"


        public static DataTable PROCESSCONDITION(DataTable dtPR)

        {
            //Remove Duplicate records
            for (int i = 0; i < dtPR.Rows.Count - 1; i++)
            {
                if (i >= 1)
                {
                    string Comp_NameLast = dtPR.Rows[i - 1].Field<string>("PSHNUM_0");
                    string Comp_Name = dtPR.Rows[i].Field<string>("PSHNUM_0");
                    if (Comp_NameLast == Comp_Name)
                    {
                        dtPR.Rows[i].Delete();
                        dtPR.AcceptChanges();
                    }
                }

            }

            //Condition Case Signer '01445'
            for (int i = 0; i <= dtPR.Rows.Count - 1; i++)
            {
                string PSHNUM_0 = dtPR.Rows[i].Field<string>("PSHNUM_0").ToString().TrimEnd();
                string Sign01445 = dtPR.Rows[i].Field<string>("USR_0").ToString().TrimEnd();
                if (Sign01445 == "01445")
                {
                    //check CCT
                    if (Master.CheckCCE_0(PSHNUM_0) == false) //true ; CCE_0 = '3200'

                    // re-Value 
                    {

                        string USR_0 = Master.GET_USER(PSHNUM_0);
                        string SIGNERNAME = Master.GET_USERVALUE(USR_0, "NOMUSR_0");
                        string SIGNEREMAIL = Master.GET_USERVALUE(USR_0, "ADDEML_0");
                        dtPR.Rows[i]["USR_0"] = USR_0;
                        dtPR.Rows[i]["SIGNERNAME"] = SIGNERNAME;
                        dtPR.Rows[i]["SIGNEREMAIL"] = SIGNEREMAIL;
                    }
                }
            }
            return dtPR;
        }
        #endregion


        #region "API"
        public void GETMETHODTEST_UPDATESTOCK()
        {
            //GET
            //Test key  

            //string strurltest = string.Format("https://partner.uat.shopeemobile.com/api/v1/items/update_stock");
            string strurltest = string.Format("https://partner.test-stable.shopeemobile.com/api/v1/items/update_stock");
            WebRequest requestObjGet = WebRequest.Create(strurltest);

            var vTimestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            string json = new JavaScriptSerializer().Serialize(new
            {
                item_id = 100008859,
                stock = 10,
                shopid = 2743,
                partner_id = 842614,
                timestamp = Convert.ToInt32(vTimestamp)
            });
            string jsonStr = json;

            //Compute HMAC-SHA256 HASH 
            string AuthorizationStr = "";
            string secretKey = "bd966151af83c6b14222713613fc79684450a9280b1f764dadeef426998eb170";
            string strRequest = strurltest + "|" + jsonStr;
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(strRequest);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                byte[] hashmessagestr = hmacsha256.ComputeHash(messageBytes);

                for (int i = 0; i < hashmessagestr.Length; i++)
                {
                    AuthorizationStr += hashmessagestr[i].ToString("X2"); //convert hex format to string
                }

            }

            // requestObjGet.Headers.Add("Authorization", "c3e9944438cc7ec157e6c4df0aac441743815d3a4f9106c22946ec772744998c"); //Stock = 100
            requestObjGet.Headers.Add("Authorization", AuthorizationStr);
            requestObjGet.Method = "POST";
            requestObjGet.ContentType = "application/json ";
            using (var streamWriter = new StreamWriter(requestObjGet.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            HttpWebResponse responseObjGet = null;
            responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();
            string strresulttest = null;
            using (Stream stream = responseObjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                strresulttest = sr.ReadToEnd();
                MessageBox.Show(strresulttest);
                sr.Close();
            }

        }


        public void BK_API_DOWNLOAD()
        {
            //GET

            string strurltest = string.Format("https://zdox.ginkgosoft.co.th/iDex/api/download/7e6694a8-d4c3-41b2-8abc-08b70a338637?key=7-1unM3xpMrzatYVfdbC20OvNbXlJgeKiDiJ62Fy");
            WebRequest requestObjGet = WebRequest.Create(strurltest);


            requestObjGet.Method = "GET";
            requestObjGet.ContentType = "application/pdf;charset=UTF-8";


            HttpWebResponse responseObjGet = null;
            responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();

            using (Stream stream = responseObjGet.GetResponseStream())
            {

                // Define buffer and buffer size
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                // Read from response and write to file
                FileStream fileStream = File.Create("//192.168.0.128/CENTER/API_DOWNLOAD.pdf");

                while ((bytesRead = stream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                } // end while

                fileStream.Close();

            }

        }
        public void BKAPI_DOWNLOAD()
        {
            System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
            string UPLOADLINK = ((Main)f).txtUploadAPI.Text;
            string KEY = ((Main)f).txtAPIKEY.Text;
            string LINK = UPLOADLINK + KEY;
            string PATH = ((Main)f).txtUploadPath.Text;
            var client = new RestClient(LINK);
            //var client = new RestClient("https://zdox.ginkgosoft.co.th/iDex/api/download/7e6694a8-d4c3-41b2-8abc-08b70a338637?key=7-1unM3xpMrzatYVfdbC20OvNbXlJgeKiDiJ62Fy");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"query\":\"\",\"variables\":{}}",
                       ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            byte[] ExcuteResponse = client.DownloadData(request);
            var SAVE_PATH = "//192.168.0.128/CENTER/API_DOWNLOAD.pdf";
            File.WriteAllBytes(SAVE_PATH, ExcuteResponse);
        }

        public void BKAPI_UPLOAD()
        {

            var client = new RestClient("https://zdox.ginkgosoft.co.th/iDex/api/task/create?key=7-1unM3xpMrzatYVfdbC20OvNbXlJgeKiDiJ62Fy");

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            //request.AddHeader("Content-Type", "multipart/form-data");
            //request.AddFile("files", "D:/Customer/TPBS/202203TPBS/PR DOC API/PR20170100061.xml");
            //request.AddFile("files", "D:/Customer/TPBS/202203TPBS/PR DOC API/PR2017010006.pdf");

            byte[] byteArrayPdf = File.ReadAllBytes(@"../../Files/PR2017010006.pdf");
            byte[] byteArrayXml = File.ReadAllBytes(@"../../Files/PR20170100061.xml");

            request.AddFile("files", byteArrayPdf, "PR2017010006.pdf", "application/pdf");
            request.AddFile("files", byteArrayXml, "PR20170100061.xml", "application/xml");


            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }
        public void API_UPLOAD()
        {
            System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
            string UPLOADLINK = ((Main)f).txtUploadAPI.Text;
            string KEY = ((Main)f).txtAPIKEY.Text;
            string LINK = UPLOADLINK + KEY;
            string PATH = ((Main)f).txtUploadPath.Text;
            var client = new RestClient(LINK);

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);


            DirectoryInfo d = new DirectoryInfo(PATH + @"\Export"); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.pdf"); //Getting Text files
            string str = "";
            string strPDF = "";
            string strXML = "";

            foreach (FileInfo file in Files)
            {
                str = file.Name;

                strPDF = str;
                strXML = str.Replace(".pdf", "") + ".xml";

                string PathPDF = @"" + PATH + @"\Export\" + str;

                string Pathxml = @"" + PATH + @"\Export\" + strXML;


                byte[] byteArrayPdf = File.ReadAllBytes(PathPDF);
                byte[] byteArrayXml = File.ReadAllBytes(Pathxml);


                request.AddFile("files", byteArrayPdf, strPDF, "application/pdf");
                request.AddFile("files", byteArrayXml, strXML, "application/xml");


                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
        }
        public void API_DOWNLOAD()
        {
            System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];
            string DOWNLOADLINK = ((Main)f).txtDownloadAPI.Text;
            string KEY = ((Main)f).txtAPIKEY.Text;
            ///>>>>>>>>>DOCID
            string DOCID = "";
            string LINK = DOWNLOADLINK + DOCID + KEY;
            var client = new RestClient(LINK);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"query\":\"\",\"variables\":{}}",
                       ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            byte[] ExcuteResponse = client.DownloadData(request);
            var SAVE_PATH = "//192.168.0.128/CENTER/API_DOWNLOAD.pdf";
            File.WriteAllBytes(SAVE_PATH, ExcuteResponse);
        }
        public static String API_SIGN()
        {
            String SAVE_PATH = "";
            try
            {

                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                string DOWNLOADLINK = ((Main)f).txtDownloadAPI.Text;
                string KEY = ((Main)f).txtAPIKEY.Text;
                string LINK = DOWNLOADLINK + KEY;
                string PATH = ((Main)f).txtUploadPath.Text;
                string PATHDOWNLOAD = ((Main)f).txtDownloadPath.Text;
                var client = new RestClient(LINK);
                //var client = new RestClient("https://zdox.ginkgosoft.co.th/iDex/api/sign?key=7-1unM3xpMrzatYVfdbC20OvNbXlJgeKiDiJ62Fy");
                client.Timeout = -1;

                DirectoryInfo d = new DirectoryInfo(PATH + @"\Export"); //Assuming Test is your Folder

                FileInfo[] Files = d.GetFiles("*.pdf"); //Getting Text files
                string str = "";
                string strPDF = "";
                string strXML = "";

                foreach (FileInfo file in Files)
                {
                    str = file.Name;

                    strPDF = str;
                    strXML = str.Replace(".pdf", "") + ".xml";

                    string PathPDF = @"" + PATH + @"\Export\" + str;

                    string Pathxml = @"" + PATH + @"\Export\" + strXML;


                    byte[] byteArrayPdf = File.ReadAllBytes(PathPDF);
                    byte[] byteArrayXml = File.ReadAllBytes(Pathxml);

                    var request = new RestRequest(Method.POST);
                    request.AddFile("files", byteArrayPdf, strPDF, "application/pdf");
                    request.AddFile("files", byteArrayXml, strXML, "application/xml");


                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    //SAVE TO PDF 

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        byte[] ExcuteResponse = client.DownloadData(request);

                        SAVE_PATH = @"" + PATH + @"\API\" + @"API_" + str;
                        File.WriteAllBytes(SAVE_PATH, ExcuteResponse);
                        System.IO.File.Move(PathPDF, @"" + PATH + @"\Backup\" + str);
                        System.IO.File.Move(Pathxml, @"" + PATH + @"\Backup\" + strXML);

                        APPLOG(str, SAVE_PATH, "Complete", "Complete");

                    }
                    else
                    {

                        if (System.IO.File.Exists(@"" + PATH + @"\Backup\" + str)) // file exist in system 
                        {
                            System.IO.File.Delete(@"" + PATH + @"\Backup\" + str);
                            System.IO.File.Delete(@"" + PATH + @"\Backup\" + strXML);
                            System.IO.File.Move(PathPDF, @"" + PATH + @"\Backup\" + str);
                            System.IO.File.Move(Pathxml, @"" + PATH + @"\Backup\" + strXML);
                        }
                        else
                        {
                            System.IO.File.Move(PathPDF, @"" + PATH + @"\Backup\" + str);
                            System.IO.File.Move(Pathxml, @"" + PATH + @"\Backup\" + strXML);
                        }
                        APPLOG(str, "", response.Content.ToString() + " " + response.ErrorMessage, "Incomplete");
                    }

                }

            }

            catch (Exception ex)
            {
                Connection.WriteLog("Error 350: API_SIGN " + ex.Message, "EXPORT");
            }

            return SAVE_PATH;
        }

        #endregion

        #region "UTILITY"

        #region "Task Schedule" 

        public static void timeschedule()
        {
            try
            {  // declare
                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Main"];

                using (TaskService tService = new TaskService())
                {
                    TaskDefinition tDefinition = tService.NewTask();
                    tDefinition.RegistrationInfo.Description = "FMS PRDOC";
                    //var trigger1 = new  DailyTrigger();
                    var trigger1 = new TimeTrigger();
                    double timer = int.Parse(((Main)f).txtSchedule.Text);
                    double timerInterval = timer;
                    //trigger1.Repetition.Duration = TimeSpan.FromMinutes(timer);
                    trigger1.Repetition.Interval = TimeSpan.FromMinutes(timerInterval);
                    tDefinition.Triggers.Add(trigger1);
                    //tDefinition.Actions.Add(new ExecAction(Path.GetDirectoryName(Application.ExecutablePath) + @"\FMSPRDOC.exe"));
                    tDefinition.Actions.Add(new ExecAction(Path.GetDirectoryName(Application.ExecutablePath) + @"\FMSPRDOC_AUTO.exe"));
                    tService.RootFolder.RegisterTaskDefinition("FMS PRDOC", tDefinition);
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("ERROR 372 timeschedule() " + ex.Message);
                //IMPORTSAGE.WriteLog("ERROR 285 " + ex.Message, "ERROR");
            }
        }
        public void DisableTimeschedule()
        {
            try
            {
                using (TaskService tService = new TaskService())
                {
                    Task disTaskperiod1 = tService.GetTask("FMS PRDOC");
                    if (disTaskperiod1 == null)
                    {
                    }
                    else
                        disTaskperiod1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("ERROR 420 " + ex.Message, "ERROR");
            }
        }

        public void EnableTimeschedule()
        {
            try
            {
                using (TaskService tService = new TaskService())
                {
                    Task disTaskperiod1 = tService.GetTask("FMS PRDOC");
                    if (disTaskperiod1 == null)
                    {
                    }
                    else
                        disTaskperiod1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("ERROR 440 " + ex.Message, "ERROR");
            }
        }

        #endregion

        public static void CreateFolder(string PathDirectory)
        {
            if (Directory.Exists(PathDirectory) == false)
            {
                Directory.CreateDirectory(PathDirectory);
            }

        }

        public static void APPLOG(String FMSPRDOC, String SAVE_PATH, String STRERR, String STA_0)
        {
            dtLOG.Rows.Clear();
            if (dtLOG.Columns.Count == 0)
            {

                dtLOG.Columns.Add("PRDOC");
                dtLOG.Columns.Add("DESCRIPTION");
                dtLOG.Columns.Add("PRDATE");
                dtLOG.Columns.Add("SRCPATH");
                dtLOG.Columns.Add("STA_0");
            }


            DateTime PRDATE = Master.GET_PRDOC(FMSPRDOC.Replace(".pdf", ""));

            string[] row = new string[] { FMSPRDOC.Replace(".pdf", ""), STRERR, PRDATE.ToString("yyyyMMdd"), SAVE_PATH, STA_0 };
            dtLOG.Rows.Add(row);

            //INSERT 
            if (dtLOG.Rows.Count != 0)
            {
                Dataclass.INSERTFMSPRLOG(dtLOG);
            }
        }



        #endregion

    }
}
