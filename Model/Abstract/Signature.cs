using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Informatio_Set;
using PTMB_Signature_API.Interface;

namespace PTMB_Signature_API.Model.Abstract
{
    public class Signature : I_Signature
    {
        public Data_Set_Employee data_set_employee { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Compare_New_Risk(string amount)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Excutre_Result excute_sign_fail(Data_Set_Employee data_set_employee, string m_id, string history)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Excutre_Result excute_sign_pass(Data_Set_Employee data_set_employee, string m_id, string record_data,string risk_value)
        {
            throw new NotImplementedException();
        }

        public bool Existing_Fail_Signed(Data_Set_Mission_Details mission_data)
        {
            throw new NotImplementedException();
        }



        public bool get_compare_level_sequence(Data_Set_Employee employee_data, List<Data_Set_Mission_Collect_Signed> list_mission_collect_signature, List<Data_Set_Mission_require> list_mission_require_signature)
        {
            throw new NotImplementedException();
        }

        public string get_employee_agent_information(string e_id)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee get_employee_information(string e_id)
        {
            throw new NotImplementedException();
        }

        public List<Information_Mission> get_employee_mission_information_requirment(string e_id)
        {
            throw new NotImplementedException();
        }

        public List<Information_Mission> get_employee_mission_information_all(string e_id)
        {
            throw new NotImplementedException();
        }

        public List<Data_Set_Mission_Details> get_employee_mission_information_fail(Data_Set_Employee employee_data)
        {
            throw new NotImplementedException();
        }

        public List<Data_Set_Sign_History> get_employee_signed_information(string signed_info)
        {
            throw new NotImplementedException();
        }
        public bool get_mission_requiresign_employee_level_max(Data_Set_Mission_Details mission_data, Data_Set_Employee employee_data)
        {
            throw new NotImplementedException();
        }

        public string get_mission_require_signature(int risk_value, SubSysNo subSysNo)
        {
            throw new NotImplementedException();
        }

        public bool get_mission_status_id(List<Data_Set_Sign> list_mission_collect_sign, List<Data_Set_Mission_require> list_mission_require_sign)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee login(string e_id)
        {
            throw new NotImplementedException();
        }

        public void Record_Signed_History(Data_Set_Sign now_sign_person_data, string m_id)
        {
            throw new NotImplementedException();
        }

        public void Record_Signed_New_RiskValue(string m_id, string new_risk_value)
        {
            throw new NotImplementedException();
        }

        public void Record_Sign_history_Reason(string m_id, string history)
        {
            throw new NotImplementedException();
        }

        public void Record_Sign_Ststus_ID(string m_id, string status_id)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee set_employee_agent_information(Data_Set_Employee data_set_employee, string value)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee set_employee_mission_information(Data_Set_Employee data_set_employee, string value)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee set_employee_prepare_sign_information(Data_Set_Employee data_set_employee, string value)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Employee set_employee_signed_information(Data_Set_Employee data_set_employee, string value)
        {
            throw new NotImplementedException();
        }

        public List<Information_Mission> get_employee_mission_information_requirement_done(string e_id)
        {
            throw new NotImplementedException();
        }

        public string get_mission_current_sign_amount(string e_id)
        {
            throw new NotImplementedException();
        }
    }
}
