using API_SendMail;
using API_SendMail.DataSet;
using Finance;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTMB_Signature_API.Model.Abstract
{
    public abstract class Abstract_PlgunFunction : IMail
    {
        #region 簽核信件區


        public void SetMailData(SET_OPTION SetOption, string Data)
        {
            MailController mailController = MailController.getInstance();
            mailController.SetSendInfo(SetOption, Data);
        }

        public void SystemIDSendMail()
        {
            MailController mailController = MailController.getInstance();
            mailController.SystemIDSendMail();
        }


        public DataSet_ExcuteResult Dev_login(string[] args)
        {
            DataSet_ExcuteResult result = new DataSet_ExcuteResult();
            MainSystemData mainsystemdata = MainSystemData.getInstance();
            mainsystemdata.setAuth(SubSysInfoController.getInstance().getInfo_GetFromMainSystem(args[0].Trim()));

            if (VerifyTools.getInstance().verify(mainsystemdata.authDatas.timeStmp, mainsystemdata.authDatas.token))
            {
                result.isSucess = true;
                result.FeedbackMsg = "主系帳號統驗證成功。";
            }
            else
            {
                result.isSucess = false;
                result.FeedbackMsg = "主系統驗證失敗。";
                mainsystemdata.setAuth(null);
            }
            return result;
        }
        public abstract void SetBackMailBodyData(Data_Set_Mission_Details mission_Details);
        public abstract void SetCreatedMailBodyData(SET_OPTION SetOption, string Data);
        public abstract void SetDoneMailBodyData(Data_Set_Mission_Details mission_Details);
        public abstract void SetFailMailBodyData(Data_Set_Mission_Details mission_Details);
        public abstract void SetPassMailBodyData(Data_Set_Mission_Details mission_Details);
        public abstract List<string> GetAllSendMailTarget(Data_Set_Mission_Details mission);
        public abstract List<string> GetCurrentMailTarget(Data_Set_Mission_Details mission);

        public void SendMail()
        {
            MailController mailController = MailController.getInstance();
            mailController.SendMail();
        }

        public abstract List<string> GetTheoreticallyRequireSignTarget(Data_Set_Mission_Details mission, int RiskValue);
        public abstract void SendRegisterMissionMail(Mission mission, int risk_value);



        #endregion
    }

    public class DemoPluginFunction : Abstract_PlgunFunction,IMail_LOANIT
    {
        public override void SendRegisterMissionMail(Mission mission, int risk_value)
        {
            /// Controller_Sign.getInstance().get_mission_information("M0130")

            //string[] test_token = { "{'cardAuth':false,'applyFormAuth':false,'assetAuth':false,'financeAuth':true,'advertisementAuth':false,'cityCardAuth':false,'acctbook':true,'isAccountOfficer':false,'isAccountCreater':false,'user_name':'林郁宸','isManager':false,'isAdmin':false,'isAssetController':false,'isOtherAcctbook_Loanit':false,'isOtherAcctbook_PTMB':false,'account':'TKFLYC0509','public_key':'','timeStmp':'20221226171709','token':'7842EC243001D543522D86F3DD5C5F13F8B790F4E0F999898C0827B0DD7952D7'}" };
            //Dev_login(test_token);
            List<string> EmailTarget_List = GetTheoreticallyRequireSignTarget(Controller_Sign.getInstance().get_mission_information(mission.data_set_mission.m_id), risk_value); ///獲取所有還尚未簽名的人

            string EmailBodyData = string.Format(
                "<p>[開發測試中][審批系統-送審通知]<br>" +
                "&nbsp;&nbsp; {0} 已於 {1} 完成進件，<br><br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;此通知信件於 {2} 由系統發出。<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*此為系統自動發信，請勿回覆*"
                , mission.data_set_mission.binding_project_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), DateTime.Now.ToString());


            SetMailData(SET_OPTION.SUBJEECT, "【測試】審批系統通知信");
            SetMailData(SET_OPTION.BODY, EmailBodyData);
            foreach (string EmailTarget in EmailTarget_List)
            {
                SetMailData(SET_OPTION.RECEIVE_EMAIL, EmailTarget);
                SystemIDSendMail();
            }
        }
        public  void SendNextSignMissionMail(Data_Set_Mission_Details mission_data, int risk_value)
        {
            //string[] test_token = { "{'cardAuth':false,'applyFormAuth':false,'assetAuth':false,'financeAuth':true,'advertisementAuth':false,'cityCardAuth':false,'acctbook':true,'isAccountOfficer':false,'isAccountCreater':false,'user_name':'林郁宸','isManager':false,'isAdmin':false,'isAssetController':false,'isOtherAcctbook_Loanit':false,'isOtherAcctbook_PTMB':false,'account':'TKFLYC0509','public_key':'','timeStmp':'20221226171709','token':'7842EC243001D543522D86F3DD5C5F13F8B790F4E0F999898C0827B0DD7952D7'}" };
            //Dev_login(test_token);

            string EmailBodyData;
            if (mission_data.status_id.Equals("99"))
            {
                SendFailMissionMail(mission_data);
            }
            else if (mission_data.status_id.Equals("3"))
            {
                SendDoneMissionMail(mission_data, risk_value);
            }
            else
            {
                Controller_Sign sign_controller = Controller_Sign.getInstance();
                List<string> EmailTarget_List = GetCurrentMailTarget(mission_data);
                Data_Set_Mission_Collect_Signed collect_sign = sign_controller.get_Previous_SignPassData(mission_data);
                if (collect_sign != null)
                {
                    EmailBodyData = string.Format("【審批系統-系統通知】<br><br>" +
                            "您有一則審批任務【{0}】需要您至【審批系統】進行審批。<br><br>" +
                            "上一層級簽核人員為【{1}】已於 {2} 完成簽核。<br>" +

                                                                    "審批時間為：【開發中】<br>" +
                                                                    "總審批時間為：【開發中】<br><br>" +
                                                                                "此通知信件於 {3} 發出<br><br><br>" +
                                                    "*此為系統自動發信，請勿回覆*", mission_data.binding_project_id, collect_sign.e_id, collect_sign.sign_time, DateTime.Now.ToString());
                }
                else
                {
                    EmailBodyData = string.Format("【審批系統-系統通知】<br><br>" +
                            "您有一則審批任務【{0}】需要您至【審批系統】進行審批。<br><br>" +

                                                                    "審批時間為：【開發中】<br>" +
                                                                    "總審批時間為：【開發中】<br><br>" +
                                                                                "此通知信件於 {1} 發出<br><br><br>" +
                                                    "*此為系統自動發信，請勿回覆*", mission_data.binding_project_id, DateTime.Now.ToString());
                }
                SetMailData(SET_OPTION.SUBJEECT, "【測試】審批系統通知信");
                SetMailData(SET_OPTION.BODY, EmailBodyData);
                foreach (string EmailTarget in EmailTarget_List)
                {
                    SetMailData(SET_OPTION.RECEIVE_EMAIL, EmailTarget);
                    SystemIDSendMail();
                }
            }






        }
        public  void SendFailMissionMail(Data_Set_Mission_Details mission)
        {
            /// Controller_Sign.getInstance().get_mission_information("M0130")

            //string[] test_token = { "{'cardAuth':false,'applyFormAuth':false,'assetAuth':false,'financeAuth':true,'advertisementAuth':false,'cityCardAuth':false,'acctbook':true,'isAccountOfficer':false,'isAccountCreater':false,'user_name':'林郁宸','isManager':false,'isAdmin':false,'isAssetController':false,'isOtherAcctbook_Loanit':false,'isOtherAcctbook_PTMB':false,'account':'TKFLYC0509','public_key':'','timeStmp':'20221226171709','token':'7842EC243001D543522D86F3DD5C5F13F8B790F4E0F999898C0827B0DD7952D7'}" };
            //Dev_login(test_token);
            Controller_Sign sign_controller = Controller_Sign.getInstance();
            List<string> EmailTarget_List = GetAllSendMailTarget(Controller_Sign.getInstance().get_mission_information(mission.m_id)); ///獲取所有還尚未簽名的人
            Data_Set_Mission_Collect_Signed collect_sign = sign_controller.get_Previous_SignFailData(mission);

            string EmailBodyData = string.Format(
                "<p>[開發測試中][審批系統-送審通知]<br>" +
                "&nbsp;&nbsp; {0} 已於 {1} 由 {2} 進行【拒絕審批】，<br><br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;此通知信件於 {3} 由系統發出。<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*此為系統自動發信，請勿回覆*"
                , mission.binding_project_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), collect_sign.e_id, DateTime.Now.ToString());


            SetMailData(SET_OPTION.SUBJEECT, "【測試】審批系統通知信【拒絕通知】");
            SetMailData(SET_OPTION.BODY, EmailBodyData);
            foreach (string EmailTarget in EmailTarget_List)
            {
                SetMailData(SET_OPTION.RECEIVE_EMAIL, EmailTarget);
                SystemIDSendMail();
            }
        }
        public  void SendDoneMissionMail(Data_Set_Mission_Details mission, int risk_value)
        {
            /// Controller_Sign.getInstance().get_mission_information("M0130")

            //string[] test_token = { "{'cardAuth':false,'applyFormAuth':false,'assetAuth':false,'financeAuth':true,'advertisementAuth':false,'cityCardAuth':false,'acctbook':true,'isAccountOfficer':false,'isAccountCreater':false,'user_name':'林郁宸','isManager':false,'isAdmin':false,'isAssetController':false,'isOtherAcctbook_Loanit':false,'isOtherAcctbook_PTMB':false,'account':'TKFLYC0509','public_key':'','timeStmp':'20221226171709','token':'7842EC243001D543522D86F3DD5C5F13F8B790F4E0F999898C0827B0DD7952D7'}" };
            //Dev_login(test_token);
            Controller_Sign sign_controller = Controller_Sign.getInstance();
            List<string> EmailTarget_List = GetTheoreticallyRequireSignTarget(Controller_Sign.getInstance().get_mission_information(mission.m_id), risk_value); ///獲取所有還尚未簽名的人
            Data_Set_Mission_Collect_Signed collect_sign = sign_controller.get_Previous_SignPassData(mission);

            string EmailBodyData = string.Format(
                "<p>[開發測試中][審批系統-送審通知]<br>" +
                "&nbsp;&nbsp; {0} 已於 {1} 由 {2} 進行【核定審批】，<br><br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;此通知信件於 {3} 由系統發出。<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;*此為系統自動發信，請勿回覆*"
                , mission.binding_project_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), collect_sign.e_id, DateTime.Now.ToString());


            SetMailData(SET_OPTION.SUBJEECT, "【測試】審批系統通知信【核定通知】");
            SetMailData(SET_OPTION.BODY, EmailBodyData);
            foreach (string EmailTarget in EmailTarget_List)
            {
                SetMailData(SET_OPTION.RECEIVE_EMAIL, EmailTarget);
                SystemIDSendMail();
            }
        }
        public override List<string> GetAllSendMailTarget(Data_Set_Mission_Details mission_data)
        {

            MailController mailController = MailController.getInstance();
            List<MailAddressReference> AllEmployeeEmail = mailController.getAllMailAdress();
            List<string> RequireSendEMail_MainSysAccount_List = Controller_Sign.getInstance().GetAllRequireSignTarget(mission_data);
            List<string> RequireSendEmail_List = new List<string>();

            foreach (MailAddressReference EmailChart in AllEmployeeEmail)
            {
                foreach (string RequireSendEMail in RequireSendEMail_MainSysAccount_List)
                {
                    if (EmailChart.MainSystemAccount.Equals(RequireSendEMail))
                    {
                        RequireSendEmail_List.Add(EmailChart.MailAddress);
                    }
                }
            }

            return RequireSendEmail_List;
        }

        public override List<string> GetCurrentMailTarget(Data_Set_Mission_Details mission_data)
        {
            MailController mailController = MailController.getInstance();
            List<MailAddressReference> AllEmployeeEmail = mailController.getAllMailAdress();
            List<string> RequireSendEMail_MainSysAccount_List = Controller_Sign.getInstance().GetNextSignedTarget(mission_data);
            List<string> RequireSendEmail_List = new List<string>();

            foreach (MailAddressReference EmailChart in AllEmployeeEmail)
            {
                foreach (string RequireSendEMail in RequireSendEMail_MainSysAccount_List)
                {
                    if (EmailChart.MainSystemAccount.Equals(RequireSendEMail))
                    {
                        RequireSendEmail_List.Add(EmailChart.MailAddress);
                    }
                }
            }
            return RequireSendEmail_List;
        }

        public override List<string> GetTheoreticallyRequireSignTarget(Data_Set_Mission_Details mission_data,int TheoreticallyRiskValue)
        {
            MailController mailController = MailController.getInstance();
            List<MailAddressReference> AllEmployeeEmail = mailController.getAllMailAdress();
            List<string> RequireSendEMail_MainSysAccount_List = Controller_Sign.getInstance().GetTheoreticallyRequireSignTarget(mission_data, TheoreticallyRiskValue);
            List<string> RequireSendEmail_List = new List<string>();

            foreach (MailAddressReference EmailChart in AllEmployeeEmail)
            {
                foreach (string RequireSendEMail in RequireSendEMail_MainSysAccount_List)
                {
                    if (EmailChart.MainSystemAccount.Equals(RequireSendEMail))
                    {
                        RequireSendEmail_List.Add(EmailChart.MailAddress);
                    }
                }
            }

            return RequireSendEmail_List;
        }


        public override void SetBackMailBodyData(Data_Set_Mission_Details mission_Details)
        {
            throw new NotImplementedException();
        }

        public override void SetCreatedMailBodyData(SET_OPTION SetOption, string Data)
        {
            throw new NotImplementedException();
        }

        public override void SetDoneMailBodyData(Data_Set_Mission_Details mission_Details)
        {
            throw new NotImplementedException();
        }

        public override void SetFailMailBodyData(Data_Set_Mission_Details mission_Details)
        {
            throw new NotImplementedException();
        }

        public override void SetPassMailBodyData(Data_Set_Mission_Details mission_Details)
        {
            throw new NotImplementedException();
        }
    }
}
