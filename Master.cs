using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace APIAPP_WIN
{
    class Master
    {
        public SqlConnection connection;
        public DataTable dtConfigDB = new DataTable();
        public DataTable dtMapAcct = new DataTable();
        internal SqlCommand Command;
        internal SqlDataAdapter adapter;



        public static DataTable GETDATA_PREQUIS(DateTime DATEFROM, DateTime DATETO)
        {
            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                    return dataSt.Tables[0];
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }


                }


                //sql1 = "SELECT top 1 * " + Environment.NewLine;
                sql1 = " SELECT PREQUIS.PSHNUM_0 " + Environment.NewLine;
                sql1 = sql1 + ", AUTILIS.ADDEML_0 AS SENDEREMAIL " + Environment.NewLine;
                sql1 = sql1 + ", 1 AS ORDER_0 " + Environment.NewLine;
                sql1 = sql1 + ", 1 AS NUMOFSIGNER " + Environment.NewLine;
                sql1 = sql1 + ", ISNULL(AWRKHISSUI.EMETTEUR_0,'') AS USR_0 " + Environment.NewLine;
                sql1 = sql1 + ",  ISNULL((SELECT NOMUSR_0 FROM  " + schemaSRC + "." + folderSRC + ".AUTILIS WHERE USR_0 = AWRKHISSUI.EMETTEUR_0 ),'') AS SIGNERNAME " + Environment.NewLine;
                sql1 = sql1 + ", ISNULL((SELECT ADDEML_0 FROM  " + schemaSRC + "." + folderSRC + ".AUTILIS WHERE USR_0 = AWRKHISSUI.EMETTEUR_0 ),'') AS SIGNEREMAIL " + Environment.NewLine;
                sql1 = sql1 + ", 'Need to sign' AS ACTION_0 " + Environment.NewLine;
                sql1 = sql1 + ",'' AS DOCNAME " + Environment.NewLine;
                sql1 = sql1 + ",'' AS DOCID " + Environment.NewLine;
                sql1 = sql1 + ",'' AS PAGENUM " + Environment.NewLine;
                sql1 = sql1 + ",'' AS X_AXIS " + Environment.NewLine;
                sql1 = sql1 + ",'' AS Y_AXIS " + Environment.NewLine;
                sql1 = sql1 + ",'' AS STAMP " + Environment.NewLine;
                sql1 = sql1 + ",AWRKHISSUI.CHRONO_0" + Environment.NewLine;
                sql1 = sql1 + ",'' AS CREATEBY" + Environment.NewLine;
                sql1 = sql1 + ",'' AS UPLOADTIME " + Environment.NewLine;
                sql1 = sql1 + ",'' AS DOWNLOADTIME " + Environment.NewLine;
                sql1 = sql1 + ", GETDATE() AS TIMESTAMP_0 " + Environment.NewLine;

                sql1 = sql1 + "FROM " + schemaSRC + "." + folderSRC + ".PREQUIS" + Environment.NewLine;
                sql1 = sql1 + "LEFT OUTER JOIN " + schemaSRC + "." + folderSRC + ".AWRKHISSUI AWRKHISSUI ON PREQUIS.PSHNUM_0 = AWRKHISSUI.CLEDEC_0" + Environment.NewLine;
                sql1 = sql1 + " LEFT OUTER JOIN " + schemaSRC + "." + folderSRC + ".AUTILIS AUTILIS ON AUTILIS.USR_0 = PREQUIS.REQUSR_0" + Environment.NewLine;

                sql1 = sql1 + "WHERE PREQUIS.APPFLG_0 = '3'";
                sql1 = sql1 + "AND PREQUIS.PRQDAT_0 BETWEEN '"+ DATEFROM.ToString("yyyyMMdd") + "' AND  '"+ DATETO.ToString("yyyyMMdd") + "'" + Environment.NewLine;
                sql1 = sql1 + "AND AWRKHISSUI.CODWRK_0 = 'PSHSIGNOT'" + Environment.NewLine;
                sql1 = sql1 + "AND PSHNUM_0 NOT IN (SELECT IDENT1_0 FROM " + schemaSRC + "." + folderSRC + ".[AOBJTXT] AOBJTXT WHERE NAM_0 LIKE '%API_PR%')" + Environment.NewLine;

                ////////////
                //sql1 = sql1 + "AND AWRKHISSUI.EMETTEUR_0 = '01445'"; 

                sql1 = sql1 + "ORDER BY AWRKHISSUI.CLEDEC_0 DESC";

                Command = new SqlCommand(sql1, connect);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = Command;

                adapter.Fill(dataSt, "Data");

                connect.Close();
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 45 (GETDATA_PREQUIS):" + ex.Message + sql1);
                // Process.WriteLog("Error 100 : " + ex.Message + sql1);

            }
            dataSt.Tables["Data"].DefaultView.Sort = " PSHNUM_0, CHRONO_0 desc";
            DataTable dt = dataSt.Tables["Data"].DefaultView.ToTable();
            //return dataSt.Tables["Data"];
            return dt;
        }

        public static DataTable GETDATA_FMSPRLOG(DateTime DATEFROM, DateTime DATETO , String Display)
        {
            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                    return dataSt.Tables[0];
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }


                }


                //sql1 = "SELECT top 1 * " + Environment.NewLine;
                sql1 = " SELECT * " + Environment.NewLine;

                sql1 = sql1 + "FROM " + schemaAPP + ".dbo.FMSPRLOG" + Environment.NewLine;
               

                sql1 = sql1 + "WHERE ";
                sql1 = sql1 + " PRDATE BETWEEN '" + DATEFROM.ToString("yyyyMMdd") + "' AND  '" + DATETO.ToString("yyyyMMdd") + "'" + Environment.NewLine;
                if (Display != "")
                {
                    sql1 = sql1 + "AND STA_0 = '" + Display + "'" + Environment.NewLine;
                }


                Command = new SqlCommand(sql1, connect);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = Command;

                adapter.Fill(dataSt, "Data");

                connect.Close();
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 45 (GETDATA_PREQUIS):" + ex.Message + sql1);
                // Process.WriteLog("Error 100 : " + ex.Message + sql1);

            }
            dataSt.Tables["Data"].DefaultView.Sort = " PRDOC";
            DataTable dt = dataSt.Tables["Data"].DefaultView.ToTable();
            //return dataSt.Tables["Data"];
            return dt;
        }

        public static DateTime GET_PRDOC(string PSHNUM_0)
        {
            DateTime Value  = default(DateTime);

            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                  
                return Value;
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }
                }

                connect = Connection.Openconnect();
                sql1 = "SELECT TOP 1 * FROM " + schemaSRC + "." + folderSRC + ".PREQUIS WHERE PSHNUM_0 = '" + PSHNUM_0 + "' ";
                SqlCommand cmd = new SqlCommand(sql1, connect);
                SqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                  
                    Value= DateTime.Parse(result["PRQDAT_0"].ToString());
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 220 (GET_ValuePRQDAT_0):" + ex.Message + sql1);

            }
            return Value;

        }

        #region "Validate"

        public static bool CheckCCE_0(string PSHNUM_0)

        {
            bool CHKCCE = false;
            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                    return CHKCCE;
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }
                }
                
                connect = Connection.Openconnect();
                sql1 = "SELECT TOP 1 * FROM " + Environment.NewLine;
                sql1 = sql1 +  schemaSRC + "." + folderSRC + ".CPTANALIN" + Environment.NewLine;
                sql1 = sql1 + "where VCRNUM_0 = '"+ PSHNUM_0 +"'" + Environment.NewLine;
                sql1 = sql1 + "AND CCE_0 = '3200'";

             
                Command = new SqlCommand(sql1, connect);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = Command;

                adapter.Fill(dataSt, "Data");

                if (dataSt.Tables["Data"].Rows.Count >= 1)
                    CHKCCE = true;
                else
                {
                    CHKCCE = false;
                }

                connect.Close();

            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 170 (CheckCCE_0):" + ex.Message + sql1);

            }

            return CHKCCE;
        }

        public static string GET_USER(string PSHNUM_0)
        {
            string Value = "";
           
            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                  return Value;
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }
                }

                connect = Connection.Openconnect();
                sql1 = "SELECT TOP 1 * FROM " + schemaSRC + "." + folderSRC + ".AWRKHISSUI WHERE CLEDEC_0 = '" + PSHNUM_0 +  "' ";
                SqlCommand cmd = new SqlCommand(sql1, connect);
                SqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    Value = result["EMETTEUR_0"].ToString();
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 220 (GET_Value):" + ex.Message + sql1);

            }
            return Value;
          
        }

        public static string GET_USERVALUE(string USR , string COLNAME)
        {
            string Value = "";

            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.Openconnect();

                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                    return Value;
                else

                {
                    for (var i = 0; i <= dtConfigDB.Rows.Count - 1; i++)
                    {
                        schemaSRC = dtConfigDB.Rows[i].Field<string>("DBSRC").ToString();
                        folderSRC = dtConfigDB.Rows[i].Field<string>("SRCFolder").ToString();
                        schemaAPP = dtConfigDB.Rows[i].Field<string>("DB").ToString();
                        folderAPP = dtConfigDB.Rows[i].Field<string>("APPFolder").ToString();
                    }
                }

                connect = Connection.Openconnect();
                sql1 = "SELECT TOP 1 * FROM " + schemaSRC + "." + folderSRC + ".AUTILIS WHERE USR_0 = '" + USR + "' ";
                SqlCommand cmd = new SqlCommand(sql1, connect);
                SqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    Value = result[COLNAME].ToString();
                }
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 220 (GET_Value):" + ex.Message + sql1);

            }
            return Value;

        }
        #endregion

        #region "Utility" 

        public static DataTable GET_DB()

        {
            string sql1 = "";
            var dataSt = new DataSet();
            SqlConnection connect = new SqlConnection();
            SqlCommand Command = new SqlCommand();

            try
            {
                connect = Connection.OpenconnectDB();
                sql1 = "SELECT  * FROM  sys.databases" + Environment.NewLine;


                Command = new SqlCommand(sql1, connect);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = Command;

                adapter.Fill(dataSt, "Data");
                connect.Close();

            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 80 (GET_DB):" + ex.Message + sql1);

            }
            return dataSt.Tables[0];
        }



        #endregion
    }
}
