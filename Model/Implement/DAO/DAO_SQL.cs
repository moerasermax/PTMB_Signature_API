using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Data_Set.Login;
using PTMB_Signature_API.Model.Implement.DAO;

namespace PTMB_Signature_API.Model
{
    public class DAO_SQL
    {
        public static DAO_SQL instance = new DAO_SQL();
        public static DAO_SQL get_instance()
        {
            return instance;
        }

        private DAO_SQL()
        {

        }

        public Data_Set_DAO_Data trans_get_login_compare_result(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            Data_Set_ReceiveMainSystem_LoginData data_set_receivemainsystem_login = new Data_Set_ReceiveMainSystem_LoginData();
            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail") && dt.Rows.Count > 1)
            {
                data_set_receivemainsystem_login.account = dt.Rows[1]["account"].ToString();
                data_set_receivemainsystem_login.password = dt.Rows[1]["password"].ToString();
                data_set_receivemainsystem_login.user_name = dt.Rows[1]["user_name"].ToString();
                data_set_receivemainsystem_login.publicKey = dt.Rows[1]["publicKey"].ToString();

            }
            else
            {
                data_set_receivemainsystem_login.account = "";
                data_set_receivemainsystem_login.password = "";
                data_set_receivemainsystem_login.user_name = "";
                data_set_receivemainsystem_login.publicKey = "";
            }

            // 執行結果
            data_set_receivemainsystem_login.excute_result = new Data_Set_Result();
            data_set_receivemainsystem_login.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            data_set_receivemainsystem_login.excute_result.fail_reason = "";
            data_set_receivemainsystem_login.excute_result.result = "查詢成功";
            data_set_receivemainsystem_login.excute_result.isSuccesed = true;
            data_set_receivemainsystem_login.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(data_set_receivemainsystem_login);

            return dao_data;
        }

        public Data_Set_DAO_Data trans_get_employee_result(DataTable dt)
        {
            Data_Set_Employee employee_data = new Data_Set_Employee();
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail") && dt.Rows.Count > 1)
            {
                employee_data.e_id = dt.Rows[1]["e_id"].ToString();
                employee_data.agent = dt.Rows[1]["agent"].ToString();
                employee_data.publickey = dt.Rows[1]["publickey"].ToString();
                //employee_data.employee_level = dt.Rows[1]["employee_level"].ToString(); //新表要再寫新的讀入
                employee_data.company = dt.Rows[1]["company"].ToString();
                employee_data.department = dt.Rows[1]["department"].ToString();
                employee_data.extends_json = dt.Rows[1]["extends_json"].ToString();
                employee_data.name = dt.Rows[1]["name"].ToString();


            }
            else
            {
                employee_data.e_id = "";
                employee_data.agent = "";
                employee_data.publickey = "";
                //employee_data.employee_level = dt.Rows[1]["employee_level"].ToString(); 新表要再寫新的讀入
                employee_data.company = "";
                employee_data.department = "";
                employee_data.extends_json = "";
                employee_data.name = "";
            }

            // 執行結果
            employee_data.excute_result = new Data_Set_Result();
            employee_data.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            employee_data.excute_result.fail_reason = "";
            employee_data.excute_result.result = "查詢成功";
            employee_data.excute_result.isSuccesed = true;
            employee_data.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(employee_data);

            return dao_data;
        }
        public Data_Set_DAO_Data trans_get_employee_signed_signature(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();

            List<Data_Set_Sign_History> list_sign_history = new List<Data_Set_Sign_History>();

            
            if (dt.Rows.Count == 1)
            {
                Data_Set_Sign_History single_sign_history = new Data_Set_Sign_History();
                single_sign_history.mission_id = "不存在完成的簽核任務";
                single_sign_history.signed_time = "無資料";
                single_sign_history.signed_person_info = "無資料";
                single_sign_history.extends_json = "無資料";

                //紀錄結果
                single_sign_history.excute_result = new Data_Set_Result();
                single_sign_history.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                single_sign_history.excute_result.fail_reason = "無存在的資料，請聯繫【研發中心-郁宸】";
                single_sign_history.excute_result.result = "查詢成功";
                single_sign_history.excute_result.isSuccesed = true;
                single_sign_history.excute_result.isError = false;
                list_sign_history.Add(single_sign_history);
            }
            else
            {
                if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail"))
                {
                    for (int i = 1; i <= dt.Rows.Count - 1; i++)
                    {
                        Data_Set_Sign_History single_sign_history = new Data_Set_Sign_History();
                        single_sign_history.mission_id = dt.Rows[i]["mission_id"].ToString();
                        single_sign_history.signed_time = dt.Rows[i]["signed_time"].ToString();
                        single_sign_history.signed_person_info = dt.Rows[i]["signed_person_info"].ToString();
                        single_sign_history.extends_json = dt.Rows[i]["extends_json"].ToString();

                        single_sign_history.excute_result = new Data_Set_Result();
                        single_sign_history.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                        single_sign_history.excute_result.fail_reason = "";
                        single_sign_history.excute_result.result = "查詢成功";
                        single_sign_history.excute_result.isSuccesed = true;
                        single_sign_history.excute_result.isError = false;
                        list_sign_history.Add(single_sign_history);
                    }
                }
            }
            dao_data.excute_result_json = JsonConvert.SerializeObject(list_sign_history);

            return dao_data;
        }
        public Data_Set_DAO_Data trans_get_mission_total_count(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();

            temp_mission_data temp_missiopn_data = new temp_mission_data();

            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail"))
            {
                temp_missiopn_data.mission_total_count = dt.Rows[1]["mission_total_count"].ToString();

                temp_missiopn_data.excute_result = new Data_Set_Result();
                temp_missiopn_data.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                temp_missiopn_data.excute_result.fail_reason = "";
                temp_missiopn_data.excute_result.result = "查詢成功";
                temp_missiopn_data.excute_result.isSuccesed = true;
                temp_missiopn_data.excute_result.isError = false;
            }
            dao_data.excute_result_json = JsonConvert.SerializeObject(temp_missiopn_data);
            return dao_data;
        }
        public Data_Set_DAO_Data trnas_set_register_new_mission(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            // 執行結果
            Data_Set_Excutre_Result excute_result = new Data_Set_Excutre_Result();
            excute_result.excute_result = new Data_Set_Result();
            excute_result.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            excute_result.excute_result.fail_reason = "";
            excute_result.excute_result.result = "查詢成功";
            excute_result.excute_result.isSuccesed = true;
            excute_result.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(excute_result);

            return dao_data;
        }

        public Data_Set_DAO_Data trnas_get_mission_all(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            List<Data_Set_Mission_Details> list_mission_detials = new List<Data_Set_Mission_Details>();

            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail"))
            {
                for (int i = 1; i <= dt.Rows.Count-1; i++)
                {
                    Data_Set_Mission_Details single_mission_details = new Data_Set_Mission_Details();

                    single_mission_details.m_id = dt.Rows[i]["m_id"].ToString();
                    single_mission_details.name = dt.Rows[i]["name"].ToString();
                    single_mission_details.status_id = dt.Rows[i]["status_id"].ToString();
                    single_mission_details.company = dt.Rows[i]["company"].ToString();
                    single_mission_details.mission_type = dt.Rows[i]["mission_type"].ToString();
                    single_mission_details.risk_value = dt.Rows[i]["risk_value"].ToString();
                    single_mission_details.binding_project_id = dt.Rows[i]["binding_project_id"].ToString();
                    single_mission_details.require_sign = JsonConvert.DeserializeObject<List<Data_Set_Mission_require>>(dt.Rows[i]["require_sign"].ToString());
                    single_mission_details.collect_signed = JsonConvert.DeserializeObject<List<Data_Set_Mission_Collect_Signed>>(dt.Rows[i]["collect_signed"].ToString());
                    single_mission_details.extends_json = dt.Rows[i]["extends_json"].ToString();
                    single_mission_details.history = dt.Rows[i]["history"].ToString();

                    // 執行結果
                    single_mission_details.excute_result = new Data_Set_Result();
                    single_mission_details.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                    single_mission_details.excute_result.fail_reason = "";
                    single_mission_details.excute_result.result = "查詢成功";
                    single_mission_details.excute_result.isSuccesed = true;
                    single_mission_details.excute_result.isError = false;

                    list_mission_detials.Add(single_mission_details);
                }
            }
            dao_data.excute_result_json = JsonConvert.SerializeObject(list_mission_detials);
            return dao_data;
        }

        public Data_Set_DAO_Data trans_get_mission(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            Data_Set_Mission_Details mission_detats = new Data_Set_Mission_Details();
            

            if (dt.Rows.Count > 1) 
            {
                if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail"))
                {
                    mission_detats.m_id = dt.Rows[1]["m_id"].ToString();
                    mission_detats.name = dt.Rows[1]["name"].ToString();
                    mission_detats.status_id = dt.Rows[1]["status_id"].ToString();
                    mission_detats.company = dt.Rows[1]["company"].ToString();
                    mission_detats.mission_type = dt.Rows[1]["mission_type"].ToString();
                    mission_detats.binding_project_id = dt.Rows[1]["binding_project_id"].ToString();
                    mission_detats.risk_value = dt.Rows[1]["risk_value"].ToString();
                    mission_detats.require_sign = JsonConvert.DeserializeObject<List<Data_Set_Mission_require>>(dt.Rows[1]["require_sign"].ToString());
                    mission_detats.collect_signed = JsonConvert.DeserializeObject<List<Data_Set_Mission_Collect_Signed>>(dt.Rows[1]["collect_signed"].ToString());
                    mission_detats.extends_json = dt.Rows[1]["extends_json"].ToString();
                    mission_detats.history = dt.Rows[1]["history"].ToString();
                    mission_detats.create_time = dt.Rows[1]["create_time"].ToString();

                    // 執行結果
                    mission_detats.excute_result = new Data_Set_Result();
                    mission_detats.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                    mission_detats.excute_result.fail_reason = "";
                    mission_detats.excute_result.result = "查詢成功";
                    mission_detats.excute_result.isSuccesed = true;
                    mission_detats.excute_result.isError = false;
                }
            }
            else
            {
                mission_detats.m_id = "不存在完成的簽核任務";
                mission_detats.status_id = "4";
            }

            dao_data.excute_result_json = JsonConvert.SerializeObject(mission_detats);
            return dao_data;
        }

        public Data_Set_DAO_Data trans_sign_result(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();

            // 執行結果
            Data_Set_Excutre_Result excute_result = new Data_Set_Excutre_Result();
            excute_result.excute_result = new Data_Set_Result();
            excute_result.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            excute_result.excute_result.fail_reason = "";
            excute_result.excute_result.result = "查詢成功";
            excute_result.excute_result.isSuccesed = true;
            excute_result.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(excute_result);


            return dao_data;
        }

        public Data_Set_DAO_Data trans_update_mission_new_require_result(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();

            // 執行結果
            Data_Set_Excutre_Result excute_result = new Data_Set_Excutre_Result();
            excute_result.excute_result = new Data_Set_Result();
            excute_result.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            excute_result.excute_result.fail_reason = "";
            excute_result.excute_result.result = "查詢成功";
            excute_result.excute_result.isSuccesed = true;
            excute_result.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(excute_result);


            return dao_data;
        }
        public Data_Set_DAO_Data trans_recrod_signed_history(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            // 執行結果
            Data_Set_Excutre_Result excute_result = new Data_Set_Excutre_Result();
            excute_result.excute_result = new Data_Set_Result();
            excute_result.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            excute_result.excute_result.fail_reason = "";
            excute_result.excute_result.result = "查詢成功";
            excute_result.excute_result.isSuccesed = true;
            excute_result.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(excute_result);

            return dao_data;
        }

        public Data_Set_DAO_Data trans_register_mission_type(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();

            // 執行結果
            Data_Set_Excutre_Result excute_result = new Data_Set_Excutre_Result();
            excute_result.excute_result = new Data_Set_Result();
            excute_result.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
            excute_result.excute_result.fail_reason = "";
            excute_result.excute_result.result = "查詢成功";
            excute_result.excute_result.isSuccesed = true;
            excute_result.excute_result.isError = false;

            dao_data.excute_result_json = JsonConvert.SerializeObject(excute_result);

            return dao_data;
        }

        public Data_Set_DAO_Data trans_mission_type_information(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            List<Data_Set_Mission_Type> list_mission_type = new List<Data_Set_Mission_Type>();
            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail") && dt.Rows.Count > 1)
            {
                for (int i = 1; i <= dt.Rows.Count - 1; i++)
                {
                    Data_Set_Mission_Type single_mission_type = new Data_Set_Mission_Type();
                    single_mission_type.company = dt.Rows[i]["company"].ToString();
                    single_mission_type.department = dt.Rows[i]["department"].ToString();
                    single_mission_type.mission_type_id = dt.Rows[i]["mission_type_id"].ToString();
                    single_mission_type.summary = dt.Rows[i]["summary"].ToString();

                    single_mission_type.excute_result = new Data_Set_Result();
                    single_mission_type.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                    single_mission_type.excute_result.fail_reason = "";
                    single_mission_type.excute_result.result = "查詢成功";
                    single_mission_type.excute_result.isSuccesed = true;
                    single_mission_type.excute_result.isError = false;

                    list_mission_type.Add(single_mission_type);
                }
            }

            dao_data.excute_result_json = JsonConvert.SerializeObject(list_mission_type);
            return dao_data;
        }

        public Data_Set_DAO_Data trans_mission_employee_information(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            List<Data_Set_Mission_Employee_Level> list_mission_employee = new List<Data_Set_Mission_Employee_Level>();
            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail") && dt.Rows.Count > 1)
            {
                for (int i = 1; i <= dt.Rows.Count - 1; i++)
                {
                    Data_Set_Mission_Employee_Level single_mission_employee = new Data_Set_Mission_Employee_Level();
                    single_mission_employee.mission_type = dt.Rows[i]["mission_type"].ToString();
                    single_mission_employee.employee_level = dt.Rows[i]["employee_level"].ToString();
                    single_mission_employee.account_list = JsonConvert.DeserializeObject<List<Data_Set_Employee_Account>>(dt.Rows[i]["account_list"].ToString()); 

                    single_mission_employee.excute_result = new Data_Set_Result();
                    single_mission_employee.excute_result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                    single_mission_employee.excute_result.fail_reason = "";
                    single_mission_employee.excute_result.result = "查詢成功";
                    single_mission_employee.excute_result.isSuccesed = true;
                    single_mission_employee.excute_result.isError = false;

                    list_mission_employee.Add(single_mission_employee);
                }
            }
            else
            {
                Data_Set_Mission_Employee_Level single_mission_employee = new Data_Set_Mission_Employee_Level();

                single_mission_employee.excute_result = new Data_Set_Result();
                single_mission_employee.excute_result.feedb_back_message = "查詢時發生問題，請聯絡【研發中心-郁宸】";
                single_mission_employee.excute_result.fail_reason = dt.Rows[0]["excute_result"].ToString();
                single_mission_employee.excute_result.result = "查詢失敗";
                single_mission_employee.excute_result.isSuccesed = false;
                single_mission_employee.excute_result.isError = true;
            }

            dao_data.excute_result_json = JsonConvert.SerializeObject(list_mission_employee);
            return dao_data;
        }


        public Data_Set_DAO_Data trans_excute_result(string excute_result, Data_Set_DAO_Data dao_data)
        {
            dao_data.excute_result_json += string.Format("\"{0}\":{1}", "excute_result", "{");
            if (excute_result.Contains("Fail"))
            {
                string[] fail_data = excute_result.Split(',');
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\",", "feedb_back_message", fail_data[0]);
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\",", "fail_reason", fail_data[1]);
                dao_data.excute_result_json += string.Format("\"{0}\":{1},", "isSuccesed", "false");
                dao_data.excute_result_json += string.Format("\"{0}\":{1},", "isError", "true");
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\"", "result", fail_data[2]);

            }
            else
            {
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\",", "feedb_back_message", excute_result);
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\",", "fail_reason", "");
                dao_data.excute_result_json += string.Format("\"{0}\":{1},", "isSuccesed", "true");
                dao_data.excute_result_json += string.Format("\"{0}\":{1},", "isError", "false");
                dao_data.excute_result_json += string.Format("\"{0}\":\"{1}\"", "result", "查詢成功");

            }
            if (dao_data.excute_result_json.Substring(0, 1).Trim().Equals("["))
            {
                dao_data.excute_result_json += string.Format("{0}{1}{2}", "}", "}","]");
            }
            else
            {
                dao_data.excute_result_json += string.Format("{0}{1}", "}", "}");
            }

            return dao_data;
        }

        public Data_Set_DAO_Data trans_excute_result(DataTable dt)
        {
            Data_Set_DAO_Data dao_data = new Data_Set_DAO_Data();
            Data_Set_Result result = new Data_Set_Result();
            if (!dt.Rows[0]["excute_result"].ToString().Contains("Fail") )
            {
                    result.feedb_back_message = dt.Rows[0]["excute_result"].ToString();
                    result.fail_reason = "";
                    result.result = "查詢成功";
                    result.isSuccesed = true;
                    result.isError = false;
            }
            else
            {
                Data_Set_Mission_Employee_Level single_mission_employee = new Data_Set_Mission_Employee_Level();

                result.feedb_back_message = "查詢時發生問題，請聯絡【研發中心-郁宸】";
                result.fail_reason = dt.Rows[0]["excute_result"].ToString();
                result.result = "查詢失敗";
                result.isSuccesed = false;
                result.isError = true;
            }
            dao_data.excute_result_json = JsonConvert.SerializeObject(result);
            return dao_data;
        }
    }
}
