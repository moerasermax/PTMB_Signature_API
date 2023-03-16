using PTMB_Signature_API.Data_Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Interface
{
    public interface I_SQL_Logic
    {

        Data_Set_DAO_Data get_login_compare(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        string set_register_new_mission(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        string get_employee_agent(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        string get_mission_require_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        Data_Set_DAO_Data get_mission_total_count(Sql_Action_Category_Option category_option, Sql_Action_Option action_option);
        Data_Set_DAO_Data get_employee_signed_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        Data_Set_DAO_Data get_employee_information(Sql_Action_Category_Option category_optiopn, Sql_Action_Option action_option,string value);
        Data_Set_DAO_Data get_mission_information_All(Sql_Action_Category_Option category_option, Sql_Action_Option action_option);
        Data_Set_DAO_Data get_mission_information(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        Data_Set_DAO_Data update_mission_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        Data_Set_DAO_Data update_mission_new_require_sign(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);

        Data_Set_DAO_Data set_record_signed_history(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);


        Data_Set_DAO_Data get_mission_type_information(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        Data_Set_DAO_Data set_register_new_mission_type(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        
        
        
        void update_sign_fail_reason(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);
        void update_mission_status_id(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);

        string update_employee_agent(Sql_Action_Option category_option, Sql_Action_Option action_option, string value);
        string update_employee_mission(Sql_Action_Option category_option, Sql_Action_Option action_option, string value);
        string update_employee_signed_signature(Sql_Action_Category_Option category_option, Sql_Action_Option action_option, string value);


    }
}
