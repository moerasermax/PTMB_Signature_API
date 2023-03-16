using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public class Data_Set_Excutre_Result
    {
        public Data_Set_Result excute_result { get; set; }
    }    
    
    public class Data_Set_Result
    {
        public string result { get; set; }
        public string fail_reason { get; set; }
        public bool isSuccesed { get; set; }
        public bool isError { get; set; }
        public string feedb_back_message { get; set; }
    }
}
