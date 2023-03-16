using PTMB_Signature_API.Data_Set.Login;
using PTMB_Signature_API.Data_Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PTMB_Signature_API.Interface
{
    public  interface I_Login
    {

        string Account { get; set; }
        string Password { get; set; }       
        Date_Set_Login Current_Token { get; set; }
        Data_Set_Excutre_Result TakeNewToken();
        Data_Set_Excutre_Result CompareTokenvalid();
        Data_Set_Excutre_Result login(string account, string password);




    }
}
