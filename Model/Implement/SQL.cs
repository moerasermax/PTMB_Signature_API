using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Interface;
using PTMB_Signature_API.Model;
using PTMB_Signature_API.Model.Abstract;

namespace PTMB_Signature_API.Model.Implement
{
    public class SQL : I_SQL
    {
        public DAO_SQL dao_sql = DAO_SQL.get_instance();
        public string MainSystem = "Formal_MainSystem_Sign";
        public string SubSystem = "Formal_MainSystem";
        public string Formal_MainSystem = "Formal_MainSystem";

        public string get_employee_agent(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            throw new NotImplementedException();
        }



        public Data_Set_DAO_Data get_login_compare(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = "Select * from [HousePricePredit].[dbo].[account] where account = @account AND password = @password";
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_get_login_compare_result(excute_Sql_cmd(category_option, conn, cmd));
            }
        }

        public Data_Set_DAO_Data get_employee_information(Sql_Action_Category_Option category_optiopn, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = "Select * from [Signature].[dbo].[employee_information] where e_id = @e_id";

                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option,cmd,value);
                return dao_sql.trans_get_employee_result(excute_Sql_cmd(category_optiopn, conn, cmd));
            }
        }


        public Data_Set_DAO_Data get_employee_signed_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = "Select * from [Signature].[dbo].[signed_history] where signed_person_info = @signed_person_info";

                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_get_employee_signed_signature(excute_Sql_cmd(category_option, conn, cmd));
            }
        }
        public Data_Set_DAO_Data get_mission_total_count(Sql_Action_Category_Option category_option, Sql_Action_Option action_option)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = "SELECT count(*) as mission_total_count FROM [Signature].[dbo].[mission_information]";
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                return dao_sql.trans_get_mission_total_count(excute_Sql_cmd(category_option, conn, cmd));
            }
        }
        public string set_register_new_mission(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Insert INTO {0} (m_id,name,status_id,company,binding_project_id,require_sign,collect_signed,extends_json,mission_type,risk_value,create_time)"
                    + " Values(@m_id,@name,@status_id,@company,@binding_project_id,@require_sign,@collect_signed,@extends_json,@mission_type,@risk_value,@create_time)", "mission_information");

                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trnas_set_register_new_mission(excute_Sql_cmd(category_option, conn, cmd)).excute_result_json;
            }
        }
        public Data_Set_DAO_Data get_mission_information_All(Sql_Action_Category_Option category_option, Sql_Action_Option action_option)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("SELECT * FROM [Signature].[dbo].[mission_information]");

                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd,"");
                return dao_sql.trnas_get_mission_all(excute_Sql_cmd(category_option, conn, cmd));
            }
        }

        public Data_Set_DAO_Data get_mission_information(Sql_Action_Category_Option category_option, Sql_Action_Option action_option,string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Select * From [Signature].[dbo].[mission_information] where m_id = @m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_get_mission(excute_Sql_cmd(category_option, conn, cmd));
            }
        }

        public Data_Set_DAO_Data update_mission_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option,string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("UPDATE [Signature].[dbo].[mission_information] SET collect_signed = @collect_signed WHERE m_id=@m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_sign_result(excute_Sql_cmd(category_option, conn, cmd));
            }
        }
        public Data_Set_DAO_Data update_mission_new_require_sign(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("UPDATE [Signature].[dbo].[mission_information] SET require_sign = @require_sign WHERE m_id=@m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_update_mission_new_require_result(excute_Sql_cmd(category_option, conn, cmd));
            }
        }


        public Data_Set_DAO_Data set_record_signed_history(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Insert INTO [Signature].[dbo].[signed_history] VALUES ({0},{1},{2},'{3}',{4})", "@mission_id", "@signed_time", "@signed_person_info","''","@sign_status");
                SqlCommand cmd = get_Sql_Command(conn,cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_recrod_signed_history(excute_Sql_cmd(category_option, conn, cmd));
            }
        }

        public void update_sign_fail_reason(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Update [Signature].[dbo].[mission_information] Set history = @history where m_id = @m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                excute_Sql_cmd(category_option, conn, cmd);
            }
        }

        public void update_mission_status_id(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Update [Signature].[dbo].[mission_information] Set status_id = @status_id where m_id = @m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                excute_Sql_cmd(category_option, conn, cmd);
            }
        }
        
        public void update_mission_new_riskvalue(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Update [Signature].[dbo].[mission_information] Set risk_value = @risk_value where m_id = @m_id");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                excute_Sql_cmd(category_option, conn, cmd);
            }
        }

        public Data_Set_DAO_Data set_register_new_mission_type(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            using(SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("Insert INTO [Signature].[dbo].[mission_type_information]"
                    + " (company,department,mission_type_id,summary)"
                    + " VALUES ({0},{1},{2},{3})","@company","@department","@mission_type_id","@summary");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
                return dao_sql.trans_register_mission_type(excute_Sql_cmd(category_option, conn, cmd));
            }
        }
        public Data_Set_DAO_Data get_mission_type_information(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        { 
        string cmd_str = null;
            using(SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                if (value.Contains("PTMB") &&(value.Contains("GA") || value.Contains("ALL")))
                {
                    cmd_str = string.Format("Select * from [Signature].[dbo].[mission_type_information]");
                }
                else if (value.Contains("ALL"))
                {
                    cmd_str = string.Format("Select * from [Signature].[dbo].[mission_type_information] where company = @company");
                }
                else
                {
                    cmd_str = string.Format("Select * from [Signature].[dbo].[mission_type_information] where company = @company AND department = @department");
                }
                
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                set_Sql_Parametert(action_option, cmd, value);
               return dao_sql.trans_mission_type_information(excute_Sql_cmd(category_option, conn, cmd));
            }
        }

        public Data_Set_DAO_Data get_mission_employee_data(Sql_Action_Category_Option category_option, Sql_Action_Option action_option)
        {
            using (SqlConnection conn = get_Sql_Load_Connection(Formal_MainSystem))
            {
                string cmd_str = string.Format("SELECT * FROM [Signature].[dbo].[employee_level]");
                SqlCommand cmd = get_Sql_Command(conn, cmd_str);
                return dao_sql.trans_mission_employee_information(excute_Sql_cmd(category_option, conn, cmd));
            }
        }


        public string get_mission_require_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            throw new NotImplementedException();
        }

        public string update_employee_agent(Sql_Action_Option category_option, Sql_Action_Option action_option, string value)
        {
            throw new NotImplementedException();
        }

        public string update_employee_mission(Sql_Action_Option category_option, Sql_Action_Option action_option, string value)
        {
            throw new NotImplementedException();
        }

        public string update_employee_signed_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value)
        {
            throw new NotImplementedException();
        }


        public DataTable excute_Sql_cmd(Sql_Action_Category_Option category_option, SqlConnection conn, SqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("excute_result");

            try
            {
                switch (category_option)
                {
                    case Sql_Action_Category_Option.GET:

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        dataTable = set_result(dataTable, "Success,");
                        da.Fill(dataTable);
                        conn.Close();
                        da.Dispose();
                        break;
                        
                    case Sql_Action_Category_Option.SET:
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        dataTable = set_result(dataTable, String.Format("Success,新增任務完成,新增完成"));
                        break;

                    case Sql_Action_Category_Option.UPDATE:
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        dataTable = set_result(dataTable, String.Format("Success,簽核已完成,更新完成"));
                        break;
                    case Sql_Action_Category_Option.DELETE:
                        break;
                    default:
                        break;
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                dataTable = set_result(dataTable, String.Format("Fail,執行資料庫查詢時發生問題，請聯絡【研發中心-郁宸】。,{0}", ex.Message.ToString().Replace("\"","")));
                return dataTable;
            }



        }
        public SqlConnection get_Sql_Load_Connection(string System)
        {
            //連接字串產生器
            SqlConnectionStringBuilder sql_conn_str = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[System].ConnectionString);

            //資料庫使用者帳號解密
            sql_conn_str.UserID = Decryption(sql_conn_str.UserID);
            
            //資料庫使用者密碼解密
            sql_conn_str.Password = Decryption(sql_conn_str.Password);


            SqlConnection conn = new SqlConnection(sql_conn_str.ToString());

           return conn;
        }

        public SqlCommand set_Sql_Parametert(Sql_Action_Option action_option, SqlCommand cmd, string value)
        {

            switch (action_option)
            {
                case Sql_Action_Option.GET_EMPLOYEE_INFORMATION:
                    cmd.Parameters.AddWithValue("@e_id", value);
                    break;
                case Sql_Action_Option.GET_EMPLOYEE_SIGNED_SIGNATURE:
                    cmd.Parameters.AddWithValue("@signed_person_info", value);
                    break;
                case Sql_Action_Option.REGISTER_NEW_MISSION:
                    string[] register_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@m_id", register_data_arr[0]);
                    cmd.Parameters.AddWithValue("@name", register_data_arr[1]);
                    cmd.Parameters.AddWithValue("@status_id", register_data_arr[2]);
                    cmd.Parameters.AddWithValue("@company", register_data_arr[3]);
                    cmd.Parameters.AddWithValue("@binding_project_id", register_data_arr[4]);
                    cmd.Parameters.AddWithValue("@require_sign", register_data_arr[5]);
                    cmd.Parameters.AddWithValue("@collect_signed", register_data_arr[6]);
                    cmd.Parameters.AddWithValue("@extends_json", register_data_arr[7]);
                    cmd.Parameters.AddWithValue("@mission_type", register_data_arr[8]);
                    cmd.Parameters.AddWithValue("@risk_value", register_data_arr[9]);
                    cmd.Parameters.AddWithValue("@create_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    break;
                case Sql_Action_Option.GET_MISSION_SPECIFIC:
                    cmd.Parameters.AddWithValue("@m_id", value);
                    break;
                case Sql_Action_Option.UPDATE_MISSION_SIGNATURE:
                    string[] signed_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@collect_signed", signed_data_arr[1]);
                    cmd.Parameters.AddWithValue("@m_id", signed_data_arr[0]);
                    break;
                case Sql_Action_Option.SET_RECORD_SIGNED_HISTORY:
                    string[] record_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@mission_id", record_data_arr[0]);
                    cmd.Parameters.AddWithValue("@signed_person_info", record_data_arr[1]);
                    cmd.Parameters.AddWithValue("@signed_time", record_data_arr[2]);
                    cmd.Parameters.AddWithValue("@sign_status", record_data_arr[3]);
                    break;
                case Sql_Action_Option.UPDATE_SIGN_FAIL_REASON:
                    string[] record_fail_reason_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@m_id", record_fail_reason_data_arr[0]);
                    cmd.Parameters.AddWithValue("@history", string.Format("{0}", record_fail_reason_data_arr[1]));
                    break;
                case Sql_Action_Option.UPDATE_MISSION_STATUS_ID:
                    string[] record_status_id_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@m_id", record_status_id_data_arr[0]);
                    cmd.Parameters.AddWithValue("@status_id", record_status_id_data_arr[1]);
                    break;
                case Sql_Action_Option.REGISTER_NEW_MISSION_TYPE:
                    string[] register_mission_type_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@company", register_mission_type_data_arr[0]);
                    cmd.Parameters.AddWithValue("@department", register_mission_type_data_arr[1]);
                    cmd.Parameters.AddWithValue("@mission_type_id", register_mission_type_data_arr[2]);
                    cmd.Parameters.AddWithValue("@summary", register_mission_type_data_arr[3]);
                    break;
                case Sql_Action_Option.GET_MISSION_TYPE:
                    string[] get_mission_type_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@company", get_mission_type_data_arr[0]);
                    cmd.Parameters.AddWithValue("@department", get_mission_type_data_arr[1]);
                    break;
                case Sql_Action_Option.GET_LOGIN_COMPARE:
                    string[] get_login_data_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@account", get_login_data_arr[0]);
                    cmd.Parameters.AddWithValue("@password", get_login_data_arr[1]);
                    break;
                case Sql_Action_Option.UPDATE_MISSION_NEW_REQUIRE_SIGN:
                    string[] new_require_sign_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@require_sign", new_require_sign_arr[1]);
                    cmd.Parameters.AddWithValue("@m_id", new_require_sign_arr[0]);
                    break;
                case Sql_Action_Option.UPDATE_MISSION_NEW_RISK_VALUE:
                    string[] new_riskvalue_arr = Regex.Split(value, ",,,,,");
                    cmd.Parameters.AddWithValue("@m_id", new_riskvalue_arr[0]);
                    cmd.Parameters.AddWithValue("@risk_value", new_riskvalue_arr[1]);
                    break;

                default:
                    break;
            }


            return cmd;
        }

        public SqlCommand get_Sql_Command(SqlConnection conn, string cmd_str)
        {
            return new SqlCommand(cmd_str,conn);
        }



        private string Decryption(string CipherText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    //加密金鑰(32 Byte)
                    aesAlg.Key = Encoding.Unicode.GetBytes("每個失落的難關，都是善意的安排。");
                    //初始向量(Initial Vector, iv) 類似雜湊演算法中的加密鹽(16 Byte)
                    aesAlg.IV = Encoding.Unicode.GetBytes("以身作則以德服人");
                    //加密器
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    //執行解密
                    byte[] decrypted = decryptor.TransformFinalBlock(Convert.FromBase64String(CipherText), 0, Convert.FromBase64String(CipherText).Length);
                    return Encoding.Unicode.GetString(decrypted);
                }
            }
            catch (Exception)
            {

                return "金鑰、密鑰並匹配，請重新確認。";
            }

        }

        private DataTable set_result(DataTable dt, string result)
        {
            DataRow dr = dt.NewRow();
            dr[0] = result;
            dt.Rows.Add(dr);
            return dt;
        }


    }
}
