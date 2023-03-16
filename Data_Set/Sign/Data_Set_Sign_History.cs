using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public class Data_Set_Sign_History
    {
        public string mission_id { get; set;}
        public string signed_time { get; set;}
        public string signed_person_info { get; set;}
        public string extends_json { get; set;}
        public Data_Set_Result excute_result { get; set;}
    }
}
