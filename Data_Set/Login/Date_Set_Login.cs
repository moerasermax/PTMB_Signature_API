using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set.Login
{
    public class Date_Set_Login
    {
        public string token { get; set; }
        public string Expired { get; set; }
      
        public Data_Set_Excutre_Result result { get; set; }
    }



    public class Data_Set_ReceiveMainSystem_LoginData
    {
        public string account { get; set; }
        public string password { get; set; }
        public string user_name { get; set; }
        public string isManager { get; set; }
        public string isAdmin { get; set; }
        public string cardAuth { get; set; }
        public string applyFormAuth { get; set; }
        public string assetAuth { get; set; }
        public string financeAuth { get; set; }
        public string advertisementAuth { get; set; }
        public string cityCardAuth { get; set; }
        public string acctbookAuth { get; set; }
        public string publicKey { get; set; }
        public string isAccountOfficer { get; set; }
        public string isAccountCreater { get; set; }
        public string isAssetController { get; set; }
        public string unit { get; set; }
        public string isOtherAcctbook_Loanit { get; set; }
        public string isOtherAcctbook_PTMB { get; set; }
        public Data_Set_Result excute_result { get; set; }
    }
}
