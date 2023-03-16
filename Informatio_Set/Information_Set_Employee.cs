using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Informatio_Set
{
    public class Information_Signature
    {
        public string signed_signature { get; set; }
        public string prepare_signature { get; set; }
    }

    public class Information_Mission
    {
        public string mission_id { get; set; }
        public string binding_project_id { get; set; }
        public string mission_type { get; set; }
        public string mission_status { set; get; }
        public string mission_status_summary { get; set; }
        public string history { get; set; }

    }
}
