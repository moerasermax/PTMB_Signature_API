using API_SendMail;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Interface
{
    public interface IMail
    {
        void SendRegisterMissionMail(Mission mission, int risk_value);
        void SetMailData(SET_OPTION SetOption, string Data);
        void SendMail();
        void SetPassMailBodyData(Data_Set_Mission_Details mission_Details);
        void SetBackMailBodyData(Data_Set_Mission_Details mission_Details);
        void SetDoneMailBodyData(Data_Set_Mission_Details mission_Details);
        void SetFailMailBodyData(Data_Set_Mission_Details mission_Details);
        void SetCreatedMailBodyData(SET_OPTION SetOption, string Data);
        List<string> GetAllSendMailTarget(Data_Set_Mission_Details mission);
        List<string> GetCurrentMailTarget(Data_Set_Mission_Details mission);
        List<string> GetTheoreticallyRequireSignTarget(Data_Set_Mission_Details mission, int RiskValue);
    }


    public interface IMail_LOANIT
    {
        void SendNextSignMissionMail(Data_Set_Mission_Details mission_data, int risk_value);
        void SendFailMissionMail(Data_Set_Mission_Details mission);
        void SendDoneMissionMail(Data_Set_Mission_Details mission, int risk_value);
    }
}
