using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMB_Signature_API.Data_Set
{
    public enum Sql_Action_Option
    {
        GET_EMPLOYEE_INFORMATION,
        GET_EMPLOYEE_SIGNED_SIGNATURE,

        GET_MISSION_ALL,
        GET_MISSION_SPECIFIC,
        GET_MISSION_FOREMPLOYEE,
        GET_MISSION_TYPE,
        GET_LOGIN_COMPARE,
        GET_MISSION_EMPLOYEE_LEVEL,

        REGISTER_NEW_MISSION,
        REGISTER_NEW_MISSION_TYPE,

        SET_RECORD_SIGNED_HISTORY,
        

        
        UPDATE_MISSION_SIGNATURE,
        UPDATE_SIGN_FAIL_REASON,
        UPDATE_MISSION_STATUS_ID,
        UPDATE_MISSION_NEW_REQUIRE_SIGN,
        UPDATE_MISSION_NEW_RISK_VALUE

    }
    public enum Sql_Action_Category_Option
    {
        GET,
        SET,
        UPDATE,
        DELETE
    }
    public enum Set_Mission_Data_Option
    {
        NAME,
        COMPANY,
        TYPE,
        BINDING_PROJECT_ID,
        RISK_VALUE,
        require_SIGNATURE
    }
    public enum Set_Mission_Type_Option
    {
        COMPANY,
        DEPARTMENT,
        MISSION_TYPE_ID,
        SUMMARY
    }



    /// <summary>
    /// Mission_Type
    /// </summary>
    public enum SubSysNo
    {
        CH001, // 貸前
        CH002, // 貸中
        CH003,  // 撥款前
        LS002,

















        AT001 /// 自動化排程
    }

}
