using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public class Data_Set_Mission
    {
        public string m_id { get; set; }
        public string name { get; set; }
        public string mission_type { get; set; }
        public string risk_value { get; set; }
        public string status_id { get; set; } 
        public string company { get; set; } 
        public string binding_project_id { get; set; }
        public string require_sign_json { get; set; }
        public string collect_signed_json { get; set; }
        public string extends { get; set; }
        public Data_Set_Result result { get; set; }


    }

    public class Data_Set_Mission_Details
    {
        public string m_id { get; set; }
        public string name { get; set; }
        public string status_id { get; set; }
        public string mission_type { get; set; }
        public string company { get; set; }
        public string binding_project_id { get; set; }
        public string risk_value { get; set; }
        public List<Data_Set_Mission_require> require_sign { get; set; }
        public List<Data_Set_Mission_Collect_Signed> collect_signed { get; set; }
        public string extends_json { get; set; }
        public string history { get; set; }
        public Data_Set_Result excute_result { get; set; }
        public string create_time { get; set; }


    }

    public class Data_Set_Mission_Status
    {
        public string company { get; set; }
        public string mission_type { get; set; }
        public string department { get; set; }
        public string status_id { get; set; }
        public string summary { get; set; }
        public string extends_json { get; set; }
    }

    public class Data_Set_Mission_Type
    {
        public string company { get; set; }
        public string department { get; set; }
        public string mission_type_id { get; set; }
        public string summary { get; set; }
        public Data_Set_Result excute_result { get; set; }

    }

    public class Data_Set_Mission_require
    {
        public string company { get; set; }
        public string department { get; set; }
        public string employee_level { get; set; }
        public string amount { get; set; }
    }


    public class Data_Set_Mission_Employee_Level
    {
        public string mission_type { get; set; }
        public string employee_level { get; set; }
        public List<Data_Set_Employee_Account> account_list { get; set; }
        public Data_Set_Result excute_result { get; set; }

    }

    public class Data_Set_Employee_Account
    {
        public string account { get; set; }
    }


}
