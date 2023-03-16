using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public class Data_Set_Employee
    {
        public string e_id { get; set; }
        public string agent { get; set; }
        public string publickey { get; set; }
        public string employee_level { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string extends_json { get; set; }
        public string name { get; set; }
        public Data_Set_Result excute_result { get; set; }
    }



}
