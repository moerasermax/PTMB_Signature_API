using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public class Data_Set_Mission_Collect_Signed
    {
        public string e_id { get; set; }    
        public string company { get; set; }
        public string department { get; set; }
        public string employee_level { get; set; }
        public string public_key { get; set; }
        public string sign_time { get; set; }
        public string sign_status { get; set; }
        public Data_Set_Extends_LoanIt extend_data { get; set; }

    }
}
