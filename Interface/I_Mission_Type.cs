using PTMB_Signature_API.Data_Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Interface
{
    public interface I_Mission_Type
    {
        List<Data_Set_Mission_Type> get_mission_type_information(Data_Set_Employee data_set_employee);
        Data_Set_Excutre_Result register_new_mission_type(Data_Set_Mission_Type data_set_mission_type);
        Data_Set_Mission_Type set_mission_type_data(Set_Mission_Type_Option Set_Mission_Type_Option, string value);


    }
}
