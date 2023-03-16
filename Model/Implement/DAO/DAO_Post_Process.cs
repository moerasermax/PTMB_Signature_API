using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Data_Set.Login;

namespace PTMB_Signature_API.Model.Implement.DAO
{
    public class DAO_Post_Process
    {
        public Data_Set_ReceiveMainSystem_LoginData set_Login_Info(Data_Set_DAO_Data dao_data)
        {
            Data_Set_ReceiveMainSystem_LoginData login_data = JsonConvert.DeserializeObject<Data_Set_ReceiveMainSystem_LoginData>(dao_data.excute_result_json);

            if (login_data.account == "")
            {
                login_data.excute_result = new Data_Set_Result();
                login_data.excute_result.feedb_back_message = "請重新確認帳號密碼。";
                login_data.excute_result.isSuccesed = false;
                login_data.excute_result.fail_reason = "帳號密碼錯誤";
                login_data.excute_result.isError = false;
            }
            else
            {
                login_data.excute_result = new Data_Set_Result();
                login_data.excute_result.feedb_back_message = "登入成功。";
                login_data.excute_result.isSuccesed = true;
            }

            return login_data;
        }
        public Data_Set_Employee set_Employee_data(Data_Set_DAO_Data dao_data)
        {
            Data_Set_Employee employee_data = JsonConvert.DeserializeObject<Data_Set_Employee>(dao_data.excute_result_json);
            if (employee_data.Equals("")) { employee_data.agent = "無代理人"; }
            if (employee_data.e_id.Equals("") || employee_data.e_id is null)
            { 
                employee_data.excute_result.isSuccesed = false;
                employee_data.excute_result.feedb_back_message = "帳號輸入錯誤，請重新確認";
                employee_data.excute_result.fail_reason = "簽核資料庫不存在此員工的工號";
                employee_data.excute_result.isError = true;
                employee_data.excute_result.result = "失敗";
            }
            
            return employee_data;
        }

        public List<Data_Set_Sign_History> set_Employee_signed_data(Data_Set_DAO_Data dao_data)
        {
            List<Data_Set_Sign_History> history_data = JsonConvert.DeserializeObject<List<Data_Set_Sign_History>>(dao_data.excute_result_json);
            return history_data;
        }

        public string get_Mission_Total_Count(Data_Set_DAO_Data dao_data)
        {
            temp_mission_data data_set_mission = JsonConvert.DeserializeObject<temp_mission_data>(dao_data.excute_result_json);
            return string.Format("M{0}",(int.Parse(data_set_mission.mission_total_count) + 1).ToString().PadLeft(4, '0'));
        }

        public List<Data_Set_Mission_Details> set_Mission_All(Data_Set_DAO_Data dao_data)
        {
            List<Data_Set_Mission_Details> list_data_set_mission = JsonConvert.DeserializeObject<List<Data_Set_Mission_Details>>(dao_data.excute_result_json);
            return list_data_set_mission;
        }

        public Data_Set_Mission_Details set_Mission(Data_Set_DAO_Data dao_data)
        {
            Data_Set_Mission_Details data_set_mission = JsonConvert.DeserializeObject<Data_Set_Mission_Details>(dao_data.excute_result_json);
            return data_set_mission;
        }

        public Data_Set_Excutre_Result set_Sign_Result(Data_Set_DAO_Data dao_data)
        {
            Data_Set_Excutre_Result data_set_result = JsonConvert.DeserializeObject<Data_Set_Excutre_Result>(dao_data.excute_result_json);
            return data_set_result;
        }

        public Data_Set_Excutre_Result set_Update_Mission_Require_Sign_Result(Data_Set_DAO_Data dao_data)
        {
            Data_Set_Excutre_Result data_set_result = JsonConvert.DeserializeObject<Data_Set_Excutre_Result>(dao_data.excute_result_json);
            return data_set_result;
        }

        public Data_Set_Excutre_Result set_register_new_mission_type_result(Data_Set_DAO_Data dao_data)
        {
            Data_Set_Excutre_Result data_set_result = JsonConvert.DeserializeObject<Data_Set_Excutre_Result>(dao_data.excute_result_json);
            return data_set_result;
        }

        public List<Data_Set_Mission_Type> set_mission_type_result(Data_Set_DAO_Data dao_data)
        {
            List<Data_Set_Mission_Type> data_set_mission_type_result = JsonConvert.DeserializeObject<List<Data_Set_Mission_Type>>(dao_data.excute_result_json);
            return data_set_mission_type_result;
        }

        public List<Data_Set_Mission_Employee_Level> set_mission_employee_level(Data_Set_DAO_Data dao_data)
        {
            List<Data_Set_Mission_Employee_Level> data_set_mission_employee_result = JsonConvert.DeserializeObject<List<Data_Set_Mission_Employee_Level>>(dao_data.excute_result_json);
            return data_set_mission_employee_result;
        }


        public static DAO_Post_Process instance = new DAO_Post_Process();
        public static DAO_Post_Process get_instance()
        {
            return instance;
        }
        private DAO_Post_Process()
        {

        }
    }




    public class temp_mission_data
    {
        public string mission_total_count { get; set; }
        public Data_Set_Result excute_result { get; set; }
    }
}
