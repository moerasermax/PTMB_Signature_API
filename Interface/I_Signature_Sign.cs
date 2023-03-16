using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTMB_Signature_API.Data_Set;
namespace PTMB_Signature_API.Interface
{
    public interface I_Signature_Sign
    {
        Data_Set_Excutre_Result excute_sign_pass(Data_Set_Employee data_set_employee, string m_id,string record_data,string risk_value);
        Data_Set_Excutre_Result excute_sign_fail(Data_Set_Employee data_set_employee, string m_id, string fail_reason);

        void Record_Signed_New_RiskValue(string m_id, string new_risk_value);
        void Record_Signed_History(Data_Set_Sign now_sign_person_data, string m_id);
        void Record_Sign_history_Reason(string m_id, string history);
        void Record_Sign_Ststus_ID(string m_id, string status_id);
        bool get_mission_status_id(List<Data_Set_Sign> list_mission_collect_sign, List<Data_Set_Mission_require> list_mission_require_sign);
        bool get_compare_level_sequence(Data_Set_Employee employee_data, List<Data_Set_Mission_Collect_Signed> list_mission_collect_signature, List<Data_Set_Mission_require> list_mission_require_signature);
        bool Existing_Fail_Signed(Data_Set_Mission_Details mission_data);
        bool get_mission_requiresign_employee_level_max(Data_Set_Mission_Details mission_data, Data_Set_Employee employee_data);
        string get_mission_require_signature(int risk_value, SubSysNo subSysNo);
        string Compare_New_Risk(string amount);
        string get_mission_current_sign_amount(string e_id);
    }
}
