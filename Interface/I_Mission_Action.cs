using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Model.Abstract;
namespace PTMB_Signature_API.Interface
{
    public interface I_Mission_Action
    {
        Data_Set_Mission_Status data_set_mission_status { get; set; }
        List<Data_Set_Mission_Employee_Level> data_set_mission_employee_level { get; set; }
        string get_total_mission_count();
        string register_new_mission(Mission mission);
        string update_mission_data(string m_id);
        string register_mission_status_summary(Data_Set_Mission_Status data_set_mission_status);
        List<Data_Set_Mission_Details> get_mission_information_all();
        Data_Set_Mission_Details get_mission_information(string m_id);

        string filter_Mission_Done_RequireAmount(Data_Set_Mission_Details mission_data);
        List<Data_Set_Mission_Details> filter_Mission_Done_RequireAmount(List<Data_Set_Mission_Details> list_mission_data);
    }
}
