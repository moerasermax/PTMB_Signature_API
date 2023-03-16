using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Informatio_Set;


namespace PTMB_Signature_API.Interface
{
    public interface I_Signature_Employee
    {
        Data_Set_Employee data_set_employee { get; set; }

        string get_employee_agent_information(string e_id);
        Data_Set_Employee get_employee_information(string e_id);
        List<Information_Mission> get_employee_mission_information_all(string e_id);
        List<Information_Mission> get_employee_mission_information_requirment(string e_id);
        List<Information_Mission> get_employee_mission_information_requirement_done(string e_id);
        List<Data_Set_Sign_History> get_employee_signed_information(string signed_info);
        List<Data_Set_Mission_Details> get_employee_mission_information_fail(Data_Set_Employee employee_data);
        Data_Set_Employee set_employee_mission_information(Data_Set_Employee data_set_employee,string value);
        Data_Set_Employee set_employee_signed_information(Data_Set_Employee data_set_employee, string value);
        Data_Set_Employee set_employee_prepare_sign_information(Data_Set_Employee data_set_employee, string value);
        Data_Set_Employee set_employee_agent_information(Data_Set_Employee data_set_employee, string value);

    }
}
