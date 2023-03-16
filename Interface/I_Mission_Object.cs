using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTMB_Signature_API.Data_Set;
namespace PTMB_Signature_API.Interface
{
    public interface I_Mission_Object
    {
        Data_Set_Mission data_set_mission { get; set; }
        Data_Set_Mission set_mission_object_data(Set_Mission_Data_Option set_option, string value);
        string get_mission_require_signature(int risk_value, SubSysNo subSysNo);
        Data_Set_Mission initiail_collect_signed(Data_Set_Mission data_set_mission);
        void initail_mission_object();





    }
}
