using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace APIAPP_WIN
{
    class Dataclass
    {
        #region "CREATE TABLE"

        public static void CREATEFMSPR()
        {
            try
            {
                string str;
                SqlConnection connect = new SqlConnection();
                connect = Connection.Openconnect("DB");
                str = "   CREATE TABLE [dbo].[FMSPR] " + Environment.NewLine;
                str = str + "  ( " + Environment.NewLine;
                str = str + "  PSHNUM_0 [nvarchar](150), " + Environment.NewLine;
                str = str + " SENDEREMAIL [nvarchar](150), " + Environment.NewLine;
                str = str + " ORDER_0 [numeric](27, 13), " + Environment.NewLine;
                str = str + " NUMOFSIGNER [numeric](27, 13), " + Environment.NewLine;
                str = str + " USR_0 [nvarchar](150), " + Environment.NewLine;
                str = str + " SIGNERNAME [nvarchar](150), " + Environment.NewLine;
                str = str + " SIGNEREMAIL [nvarchar](150), " + Environment.NewLine;
                str = str + " ACTION_0 [nvarchar](15), " + Environment.NewLine;
                str = str + " DOCNAME [nvarchar](250), " + Environment.NewLine;
                str = str + " DOCID [nvarchar](150), " + Environment.NewLine;
                str = str + " PAGENUM [numeric](27, 13), " + Environment.NewLine;
                str = str + " X_AXIS [numeric](27, 13), " + Environment.NewLine;
                str = str + " Y_AXIS [numeric](27, 13), " + Environment.NewLine;
                str = str + " CHRONO_0 [nvarchar](150), " + Environment.NewLine;
                str = str + " CREATEBY [nvarchar](150), " + Environment.NewLine;
                str = str + " UPLOADTIME [datetime], " + Environment.NewLine;
                str = str + " DOWNLOADTIME [datetime], " + Environment.NewLine;
                str = str + " TIMESTAMP_0 [datetime], " + Environment.NewLine;
                str = str + " STAMP [nvarchar](5)" + Environment.NewLine;

                str = str + " )";


                SqlCommand cmd = new SqlCommand(str, connect);
                cmd.ExecuteNonQuery();
                connect.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public static void CREATEFMSPRLOG()
        {
            try
            {
                string str;
                SqlConnection connect = new SqlConnection();
                connect = Connection.Openconnect("DB");
                str = "   CREATE TABLE [dbo].[FMSPRLOG] " + Environment.NewLine;
                str = str + "  ( " + Environment.NewLine;
                str = str + " SEQ [bigint] IDENTITY(1,1) NOT NULL, " + Environment.NewLine;
                str = str + "  PRDOC [nvarchar](150), " + Environment.NewLine;
                str = str + " DESCRIPTION [TEXT], " + Environment.NewLine;
                str = str + " PRDATE [DATE], " + Environment.NewLine;

                str = str + " SRCPATH [TEXT], " + Environment.NewLine;
                str = str + " STA_0  [nvarchar](10), " + Environment.NewLine;
                str = str + " TIMESTAMP [DATETIME] " + Environment.NewLine;

                str = str + " )";


                SqlCommand cmd = new SqlCommand(str, connect);
                cmd.ExecuteNonQuery();
                connect.Close();

            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 85 : " + ex.Message);
            }
        }
        #endregion

        #region "INSERT "

        public static void INSERTFMSPR(DataTable dt)
        {
            try
            {
                string str;
                SqlConnection connect = new SqlConnection();
                connect = Connection.Openconnect("DB");

                str = "";
                string TimeStamp;
                for (var i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    TimeStamp = dt.Rows[i].Field<DateTime>("TIMESTAMP_0").ToString().Replace("/", "-");
                    str = str + "    INSERT INTO " + connect.Database + ".dbo.[FMSPR] " + Environment.NewLine;
                    str = str + "  ([PSHNUM_0]  " + Environment.NewLine;
                    str = str + "          ,[SENDEREMAIL] " + Environment.NewLine;
                    str = str + "          ,[ORDER_0] " + Environment.NewLine;
                    str = str + "          ,[NUMOFSIGNER] " + Environment.NewLine;
                    str = str + "          ,[USR_0] " + Environment.NewLine;
                    str = str + "          ,[SIGNERNAME] " + Environment.NewLine;
                    str = str + "          ,[SIGNEREMAIL] " + Environment.NewLine;
                    str = str + "          ,[ACTION_0] " + Environment.NewLine;
                    str = str + "          ,[DOCNAME] " + Environment.NewLine;
                    str = str + "          ,[DOCID] " + Environment.NewLine;
                    str = str + "          ,[PAGENUM] " + Environment.NewLine;
                    str = str + "          ,[X_AXIS] " + Environment.NewLine;
                    str = str + "          ,[Y_AXIS] " + Environment.NewLine;
                    str = str + "          ,[CHRONO_0] " + Environment.NewLine;
                    str = str + "          ,[CREATEBY] " + Environment.NewLine;
                    str = str + "          ,[UPLOADTIME] " + Environment.NewLine;
                    str = str + "          ,[DOWNLOADTIME] " + Environment.NewLine;
                    str = str + "          ,[TIMESTAMP_0] " + Environment.NewLine;
                    str = str + "          ,[STAMP])" + Environment.NewLine;
                    str = str + "    VALUES " + Environment.NewLine;
                    str = str + "          ('" + dt.Rows[i].Field<string>("PSHNUM_0").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("SENDEREMAIL").ToString() + "' " + Environment.NewLine;
                    str = str + "           ," + dt.Rows[i].Field<int>("ORDER_0") + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<int>("NUMOFSIGNER") + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("USR_0").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("SIGNERNAME").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("SIGNEREMAIL").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("ACTION_0").ToString() + "'" + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("DOCNAME").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("DOCID").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("PAGENUM").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("X_AXIS").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("Y_AXIS").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("CHRONO_0").ToString() + "'" + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("CREATEBY").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("UPLOADTIME").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("DOWNLOADTIME").ToString() + "'  " + Environment.NewLine;
                    str = str + "           ,getdate() " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("STAMP").ToString() + "'" + Environment.NewLine;
                    str = str + "          )";
                }
                SqlCommand cmd = new SqlCommand(str, connect);
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 122 : " + ex.Message);
            }

        }

        public static void INSERTFMSPRLOG(DataTable dt)
        {
            try
            {
                string str;
                SqlConnection connect = new SqlConnection();
                connect = Connection.Openconnect("DB");

                str = "";

                for (var i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    str = str + "    INSERT INTO " + connect.Database + ".dbo.[FMSPRLOG] " + Environment.NewLine;

                    str = str + "         ( [PRDOC] " + Environment.NewLine;
                    str = str + "          ,[DESCRIPTION] " + Environment.NewLine;
                    str = str + "          ,[PRDATE] " + Environment.NewLine;
                    str = str + "          ,[SRCPATH] " + Environment.NewLine;
                    str = str + "          ,[STA_0]" + Environment.NewLine;
                    str = str + "          ,[TIMESTAMP] " + Environment.NewLine;


                    str = str + "    ) VALUES " + Environment.NewLine;
                    str = str + "          (";
                    str = str + "          '" + dt.Rows[i].Field<string>("PRDOC").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("DESCRIPTION").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("PRDATE") + "'" + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("SRCPATH").ToString() + "' " + Environment.NewLine;
                    str = str + "           ,'" + dt.Rows[i].Field<string>("STA_0") + "'" + Environment.NewLine;

                    str = str + "           ,getdate() " + Environment.NewLine;

                    str = str + "          )";
                }
                SqlCommand cmd = new SqlCommand(str, connect);
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            catch (Exception ex)
            {
                Connection.WriteLog("Error 195 : " + ex.Message);
            }

        }

        public static void INSERTAOBJTXT(string PRNUM, string filename)

        {
            var dataSt = new DataSet();
            string str;

            try
            {

                SqlConnection connect = new SqlConnection();
                connect = Connection.Openconnect();
                DataTable dtConfigDB = Connection.ReadConfig();

                string schemaSRC = "";
                string folderSRC = "";
                string schemaAPP = "";
                string folderAPP = "";

                if (dtConfigDB.Rows.Count == 0)
                {

                }

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


                str = @"INSERT INTO " + schemaSRC + "." + folderSRC + ".[AOBJTXT]" + Environment.NewLine;
                str = str + "([UPDTICK_0] " + Environment.NewLine;
                str = str + "           ,[ABREV_0] " + Environment.NewLine;
                str = str + "           ,[IDENT1_0] " + Environment.NewLine;
                str = str + "           ,[IDENT2_0] " + Environment.NewLine;
                str = str + "           ,[IDENT3_0]  " + Environment.NewLine;
                str = str + "           ,[TYPDOC_0] " + Environment.NewLine;
                str = str + "           ,[NAM_0] " + Environment.NewLine;
                str = str + "           ,[MOTCLE_0] " + Environment.NewLine;
                str = str + "           ,[MOTCLE_1] " + Environment.NewLine;
                str = str + "           ,[MOTCLE_2] " + Environment.NewLine;
                str = str + "           ,[MOTCLE_3] " + Environment.NewLine;
                str = str + "           ,[MOTCLE_4] " + Environment.NewLine;
                str = str + "           ,[CAT_0] " + Environment.NewLine;
                str = str + "           ,[IDTSTO_0] " + Environment.NewLine;

                str = str + "           ,[VERSION_0] " + Environment.NewLine;
                str = str + "           ,[IDTCNT_0]  " + Environment.NewLine;
                str = str + "           ,[CREUSR_0] " + Environment.NewLine;
                str = str + "           ,[UPDUSR_0] " + Environment.NewLine;
                str = str + "           ,[CREDATTIM_0] " + Environment.NewLine;
                str = str + "           ,[UPDDATTIM_0] " + Environment.NewLine;
                str = str + "           ,[AUUID_0])" + Environment.NewLine;

                str += @"VALUES
            (1,
            'PSH',
            '" + PRNUM + @"',
            ' ',
            (SELECT COUNT(ROWID) + 1 AS val FROM " + schemaSRC + "." + folderSRC + ".[AOBJTXT] " + "WHERE [IDENT1_0] = '" + PRNUM + @"'),
            'PDF',
            '[ATT]/API/" + filename + @"',
            ' ',
            ' ',
            ' ',
            ' ',
            ' ',
            '1',
            ' ',
            ' ',
            ' ',
            'ADMIN',
            'ADMIN',
            GETDATE(),
            GETDATE(),
            0x113105A621A192438CCA9F0D0E6667DF
		    )";
                Connection.WriteLog("INSERTAOBJTXT " + str);
                SqlCommand cmd = new SqlCommand(str, connect);
                cmd.ExecuteNonQuery();
                connect.Close();
            }

            catch (Exception ex)
            {
                Connection.WriteLog("Error 217 : " + ex.Message);
            }
        }


        #endregion
    }
}
