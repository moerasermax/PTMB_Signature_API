using API_SendMail;
using Newtonsoft.Json;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Data_Set.Login;
using PTMB_Signature_API.Informatio_Set;
using PTMB_Signature_API.Interface;
using PTMB_Signature_API.Model.Abstract;
using PTMB_Signature_API.Model.Implement;
using PTMB_Signature_API.Model.Implement.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace PTMB_Signature_API.Model.Abstract
{
    #region

    // 與 Controller_Sign.cs 最大差異在 Compare_New_RiskValue 這支是抽象、另一支是實作；Controller_Sign.cs是要給這支API當作測試
    // 基本上這支 與 Controller_Sign.cs 大同小異
    // 目的在於 知道 演算法結構差異

    #endregion
    public abstract class Controller_Sign_Test_Abstract
    {
        DAO_Post_Process dao_post_process = DAO_Post_Process.get_instance();


        public Data_Set_Employee data_set_employee { get; set; }
        public Data_Set_Mission_Status data_set_mission_status { get; set; }
        public Data_Set_Mission data_set_mission { get; set; }
        public List<Data_Set_Mission_Employee_Level> data_set_mission_employee_level { get; set; }

        public string Account { get; set; }
        public string Password { get; set; }
        public Date_Set_Login Current_Token { get; set; }

        public List<Information_Mission> get_employee_mission_information_all(string e_id)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                Data_Set_Employee employee_data = get_employee_information(e_id);
                List<Data_Set_Mission_Details> list_mission_data = get_mission_information_all();

                List<Information_Mission> list_information_mission = new List<Information_Mission>();
                bool compare_company = false, compare_department = false, compare_emplyee_Level = false;

                foreach (Data_Set_Mission_Details mission_data in list_mission_data)
                {
                    foreach (Data_Set_Mission_require mission_require_signature in mission_data.require_sign)
                    {
                        if (employee_data.employee_level is null) { employee_data.employee_level = get_employee_level(employee_data.e_id, mission_data.mission_type); }
                        if (employee_data.company.Equals(mission_require_signature.company))
                        {
                            compare_company = true;
                        }
                        if (employee_data.department.Equals(mission_require_signature.department) || employee_data.department.ToUpper().Equals("ALL"))
                        {
                            compare_department = true;
                        }
                        if (employee_data.employee_level.Equals(mission_require_signature.employee_level))
                        {
                            compare_emplyee_Level = true;
                        }
                        if ((compare_company == true) && (compare_department == true) && (compare_emplyee_Level == true))
                        {
                            Information_Mission prepare_add_mission = new Information_Mission();
                            prepare_add_mission.mission_id = mission_data.m_id;
                            prepare_add_mission.mission_status = mission_data.status_id;
                            prepare_add_mission.history = mission_data.history;
                            prepare_add_mission.binding_project_id = mission_data.binding_project_id;
                            list_information_mission.Add(prepare_add_mission);
                        }
                        compare_company = false;
                        compare_department = false;
                        compare_emplyee_Level = false;
                    }
                }
                return list_information_mission;
            }
            else
            {
                throw new ArgumentException("請先登入");
            }

        }
        public List<Information_Mission> get_employee_mission_information_requirment(string e_id)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                Data_Set_Employee employee_data = get_employee_information(e_id);
                List<Data_Set_Mission_Details> list_mission_data = get_mission_information_all();

                List<Information_Mission> list_information_mission = new List<Information_Mission>();
                bool compare_company = false, compare_department = false, compare_emplyee_Level = false, compare_sign_status = false;

                /// 過濾已達成的簽核
                filter_Mission_Done_RequireAmount(list_mission_data);


                foreach (Data_Set_Mission_Details mission_data in list_mission_data)
                {
                    if (!Existing_Fail_Signed(mission_data))
                    {
                        mission_data.require_sign.Sort((x, y) => -x.employee_level.CompareTo(y.employee_level));

                        foreach (Data_Set_Mission_require mission_require_signature in mission_data.require_sign.ToArray())
                        {
                            if (employee_data.employee_level is null) { employee_data.employee_level = get_employee_level(employee_data.e_id, mission_data.mission_type); }

                            if (employee_data.company.Equals(mission_require_signature.company))
                            {
                                compare_company = true;
                            }
                            if (employee_data.department.Equals(mission_require_signature.department) || employee_data.department.ToUpper().Equals("ALL"))
                            {
                                compare_department = true;
                            }
                            if (employee_data.employee_level.Equals(mission_require_signature.employee_level))
                            {
                                compare_emplyee_Level = true;
                            }
                            if ((compare_company == true) && (compare_department == true) && (compare_emplyee_Level == true))
                            {
                                /// 將簽核資訊與需求簽核資訊比對
                                if (get_compare_level_sequence(employee_data, mission_data.collect_signed, mission_data.require_sign))  ///判斷是否輪到此職員
                                {
                                    if (mission_data.require_sign.Count != 1)
                                    {
                                        Information_Mission prepare_add_mission = new Information_Mission();
                                        prepare_add_mission.mission_id = mission_data.m_id;
                                        prepare_add_mission.mission_status = mission_data.status_id;
                                        prepare_add_mission.binding_project_id = mission_data.binding_project_id;
                                        list_information_mission.Add(prepare_add_mission);
                                    }
                                }
                            }
                            compare_company = false;
                            compare_department = false;
                            compare_emplyee_Level = false;
                        }
                    }
                }
                return list_information_mission;
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }
        public List<Information_Mission> get_employee_mission_information_requirement_done(string e_id)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                Data_Set_Employee employee_data = get_employee_information(e_id);
                List<Data_Set_Mission_Details> list_mission_data = get_mission_information_all();

                List<Information_Mission> list_information_mission = new List<Information_Mission>();
                bool compare_company = false, compare_department = false, compare_emplyee_Level = false, compare_sign_status = false;

                /// 過濾已達成的簽核
                filter_Mission_Done_RequireAmount(list_mission_data); // 過濾還需要多少簽核


                foreach (Data_Set_Mission_Details mission_data in list_mission_data)
                {
                    if (!Existing_Fail_Signed(mission_data))
                    {
                        mission_data.require_sign.Sort((x, y) => -x.employee_level.CompareTo(y.employee_level));

                        foreach (Data_Set_Mission_require mission_require_signature in mission_data.require_sign.ToArray())
                        {
                            if (employee_data.employee_level is null) { employee_data.employee_level = get_employee_level(employee_data.e_id, mission_data.mission_type); }

                            if (employee_data.company.Equals(mission_require_signature.company))
                            {
                                compare_company = true;
                            }
                            if (employee_data.department.Equals(mission_require_signature.department) || employee_data.department.ToUpper().Equals("ALL"))
                            {
                                compare_department = true;
                            }
                            if (employee_data.employee_level.Equals(mission_require_signature.employee_level))
                            {
                                compare_emplyee_Level = true;
                            }
                            if ((compare_company == true) && (compare_department == true) && (compare_emplyee_Level == true))
                            {
                                /// 將簽核資訊與需求簽核資訊比對
                                if (get_compare_level_sequence(employee_data, mission_data.collect_signed, mission_data.require_sign))  ///判斷是否輪到此職員
                                {
                                    if (mission_data.require_sign.Count == 1)
                                    {
                                        Information_Mission prepare_add_mission = new Information_Mission();
                                        prepare_add_mission.mission_id = mission_data.m_id;
                                        prepare_add_mission.mission_status = mission_data.status_id;
                                        prepare_add_mission.binding_project_id = mission_data.binding_project_id;
                                        list_information_mission.Add(prepare_add_mission);
                                    }
                                }
                            }
                            compare_company = false;
                            compare_department = false;
                            compare_emplyee_Level = false;
                        }
                    }
                }
                return list_information_mission;
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }

        public List<Data_Set_Mission_Details> get_employee_mission_information_fail(Data_Set_Employee employee_data)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                List<Data_Set_Mission_Details> list_fail_mission = new List<Data_Set_Mission_Details>();
                List<Data_Set_Mission_Details> list_mission_data = get_mission_information_all();
                bool compare_company = false, compare_department = false, compare_emplyee_Level = false;

                foreach (Data_Set_Mission_Details mission_data in list_mission_data)
                {
                    foreach (Data_Set_Mission_require mission_require_signature in mission_data.require_sign)
                    {
                        if (employee_data.employee_level is null) { employee_data.employee_level = get_employee_level(employee_data.e_id, mission_data.mission_type); }
                        if (employee_data.company.Equals(mission_require_signature.company))
                        {
                            compare_company = true;
                        }
                        if (employee_data.department.Equals(mission_require_signature.department) || employee_data.department.ToUpper().Equals("ALL"))
                        {
                            compare_department = true;
                        }
                        if (employee_data.employee_level.Equals(mission_require_signature.employee_level))
                        {
                            compare_emplyee_Level = true;
                        }
                        if ((compare_company == true) && (compare_department == true) && (compare_emplyee_Level == true) && (mission_data.status_id.Equals("99")))
                        {
                            list_fail_mission.Add(mission_data);
                        }
                        compare_company = false;
                        compare_department = false;
                        compare_emplyee_Level = false;
                    }
                }




                //List <Data_Set_Mission_Details> list_fail_mission = new List<Data_Set_Mission_Details>();  
                //foreach (Data_Set_Sign_History signed_history in get_employee_signed_information(employee_data.publickey))
                //{
                //    // Show Result
                //    Data_Set_Mission_Details mission_info = get_mission_information(signed_history.mission_id);
                //    if(mission_info.status_id.Equals("4"))
                //    {
                //        list_fail_mission.Add(mission_info);
                //    }
                //}

                return list_fail_mission;
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }

        /// <summary>
        /// 最原版的退回，需要更動其他子公司，從這邊複製後下去改
        /// </summary>
        /// <param name="data_set_employee"></param>
        /// <param name="m_id"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Data_Set_Excutre_Result excute_sign_back(Data_Set_Employee data_set_employee, string m_id, string history)
        {
            /// 這邊邏輯是從現在簽的人 反推，所以跟pass的保存簽的人邏輯相反。
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {
                int current_level;



                Data_Set_Mission_Details mission_data = get_mission_information(m_id);
                List<Data_Set_Mission_Collect_Signed> data_set_sign_collect = mission_data.collect_signed;

                List<Data_Set_Sign> list_data_set_sign = new List<Data_Set_Sign>();

                /// 儲存現在簽的人
                Data_Set_Sign now_sign_person_data = new Data_Set_Sign();
                {
                    now_sign_person_data.e_id = data_set_employee.e_id;
                    now_sign_person_data.public_key = data_set_employee.publickey;
                    now_sign_person_data.company = data_set_employee.company;
                    now_sign_person_data.department = data_set_employee.department;
                    now_sign_person_data.employee_level = data_set_employee.employee_level;
                    now_sign_person_data.sign_time = DateTime.Now.ToString();
                    now_sign_person_data.sign_status = "back";

                    list_data_set_sign.Add(now_sign_person_data);
                    if (data_set_employee.employee_level is null) { current_level = int.Parse(get_employee_level(data_set_employee.e_id, mission_data.mission_type)); } else { current_level = int.Parse(data_set_employee.employee_level); }
                }

                /// 保存已簽過的人
                {
                    foreach (Data_Set_Mission_Collect_Signed collect_signed_data in data_set_sign_collect)
                    {
                        if (!collect_signed_data.public_key.Equals(""))
                        {
                            Data_Set_Sign save_collec_data = new Data_Set_Sign();
                            save_collec_data.e_id = collect_signed_data.e_id;
                            save_collec_data.public_key = collect_signed_data.public_key;
                            save_collec_data.company = collect_signed_data.company;
                            save_collec_data.department = collect_signed_data.department;
                            save_collec_data.employee_level = collect_signed_data.employee_level;
                            save_collec_data.sign_time = collect_signed_data.sign_time;
                            if ((current_level + 1) == (int.Parse(save_collec_data.employee_level))) { save_collec_data.sign_status = "false"; } else { save_collec_data.sign_status = collect_signed_data.sign_status; }
                            list_data_set_sign.Add(save_collec_data);
                        }
                    }
                }


                if (data_set_sign_collect.Count == 1) /// 如果只有一個剛簽的人代表還在第一層，直接駁回。
                {
                    return excute_sign_fail(data_set_employee, m_id, history);
                }
                else
                {
                    /// 保存至DB
                    {
                        string NewhistoryRecord = string.Format("{0} 於 {1} 註記：【{2}】 簽核結果：【{3}】", data_set_employee.name, DateTime.Now, history, "否決");
                        string history_record = mission_data.history + "\r\n" + NewhistoryRecord; // 把舊紀錄與新紀錄串接


                        Record_Signed_History(now_sign_person_data, m_id); // 紀錄簽核紀錄
                        Record_Sign_history_Reason(m_id, history_record); // 紀錄備註
                        Record_Sign_Ststus_ID(m_id, "4");
                        return update_mission_new_signature(m_id, list_data_set_sign); // 更新已蒐集之簽核內容
                    }
                }




            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }
        public Data_Set_Excutre_Result excute_sign_fail(Data_Set_Employee data_set_employee, string m_id, string history)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                Data_Set_Mission_Details mission_data = get_mission_information(m_id);
                List<Data_Set_Mission_Collect_Signed> data_set_sign_collect = mission_data.collect_signed;
                /// 保存已簽過的人
                List<Data_Set_Sign> list_data_set_sign = new List<Data_Set_Sign>();
                {
                    foreach (Data_Set_Mission_Collect_Signed collect_signed_data in data_set_sign_collect)
                    {
                        if (!collect_signed_data.public_key.Equals(""))
                        {
                            Data_Set_Sign save_collec_data = new Data_Set_Sign();
                            save_collec_data.public_key = collect_signed_data.public_key;
                            save_collec_data.company = collect_signed_data.company;
                            save_collec_data.department = collect_signed_data.department;
                            save_collec_data.employee_level = collect_signed_data.employee_level;
                            save_collec_data.sign_time = collect_signed_data.sign_time;
                            save_collec_data.sign_status = collect_signed_data.sign_status;
                            list_data_set_sign.Add(save_collec_data);
                        }
                    }
                }
                /// 儲存現在簽的人
                Data_Set_Sign now_sign_person_data = new Data_Set_Sign();
                {
                    now_sign_person_data.public_key = data_set_employee.publickey;
                    now_sign_person_data.company = data_set_employee.company;
                    now_sign_person_data.department = data_set_employee.department;
                    now_sign_person_data.employee_level = data_set_employee.employee_level;
                    now_sign_person_data.sign_time = DateTime.Now.ToString();
                    now_sign_person_data.sign_status = "fail";
                    list_data_set_sign.Add(now_sign_person_data);
                }

                /// 保存至DB
                {
                    string NewhistoryRecord = string.Format("{0} 於 {1} 註記：【{2}】 簽核結果：【{3}】", data_set_employee.name, DateTime.Now, history, "否決");
                    string history_record = mission_data.history + "\r\n" + NewhistoryRecord; // 把舊紀錄與新紀錄串接


                    Record_Signed_History(now_sign_person_data, m_id); // 紀錄目前所有簽核
                    Record_Sign_history_Reason(m_id, history_record); // 紀錄備註
                    Record_Sign_Ststus_ID(m_id, 99.ToString()); // 紀錄目前狀態
                    return update_mission_new_signature(m_id, list_data_set_sign); ;
                }
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }


        public void Record_Sign_history_Reason(string m_id, string history)
        {
            SQL sql = new SQL();
            sql.update_sign_fail_reason(Sql_Action_Category_Option.UPDATE, Sql_Action_Option.UPDATE_SIGN_FAIL_REASON, string.Format("{0},,,,,{1}", m_id, history));
        }
        public Data_Set_Excutre_Result excute_sign_pass(Data_Set_Employee data_set_employee, string m_id, string history, string risk_value)
        {
            string current_amount = "0";
            Data_Set_Excutre_Result result = CompareTokenvalid();


            if (result.excute_result.isSuccesed)
            {

                Data_Set_Mission_Details mission_data = get_mission_information(m_id);
                List<Data_Set_Mission_Collect_Signed> data_set_sign_collect = mission_data.collect_signed;
                mission_data.require_sign.Sort((x, y) => -x.employee_level.CompareTo(y.employee_level)); /// 排序
                                                                                                         /// 保存已簽過的人
                List<Data_Set_Sign> list_data_set_sign = new List<Data_Set_Sign>();
                {
                    foreach (Data_Set_Mission_Collect_Signed collect_signed_data in data_set_sign_collect)
                    {
                        if (!collect_signed_data.public_key.Equals(""))
                        {
                            Data_Set_Sign save_collec_data = new Data_Set_Sign();
                            save_collec_data.e_id = collect_signed_data.e_id;
                            save_collec_data.public_key = collect_signed_data.public_key;
                            save_collec_data.company = collect_signed_data.company;
                            save_collec_data.department = collect_signed_data.department;
                            save_collec_data.employee_level = collect_signed_data.employee_level;
                            save_collec_data.sign_time = collect_signed_data.sign_time;
                            save_collec_data.sign_status = collect_signed_data.sign_status;
                            current_amount = save_collec_data.extend_data.loan_amount;
                            list_data_set_sign.Add(save_collec_data);
                        }
                    }
                }
                /// 儲存現在簽的人
                Data_Set_Sign now_sign_person_data = new Data_Set_Sign();
                {
                    now_sign_person_data.extend_data = new Data_Set_Extends_LoanIt();
                    now_sign_person_data.e_id = data_set_employee.e_id;
                    now_sign_person_data.public_key = data_set_employee.publickey;
                    now_sign_person_data.company = data_set_employee.company;
                    now_sign_person_data.department = data_set_employee.department;
                    if (data_set_employee.employee_level is null) { now_sign_person_data.employee_level = get_employee_level(data_set_employee.e_id, mission_data.mission_type); } else { now_sign_person_data.employee_level = data_set_employee.employee_level; }
                    now_sign_person_data.sign_time = DateTime.Now.ToString();
                    now_sign_person_data.sign_status = "pass";
                    list_data_set_sign.Add(now_sign_person_data);
                }




                /// 判斷風險值是否改變
                if (!risk_value.Equals(""))
                {
                    string compare_risk_value_result = Compare_New_Risk(risk_value);
                    if (compare_risk_value_result.Split(',')[0].Equals("true"))
                    {
                        string[] Risk_Arr = compare_risk_value_result.Split(',');
                        //if(mission_data)
                        if (!(Risk_Arr[0].Trim() == mission_data.risk_value.Trim()))
                        {
                            string new_require_sign = LOANIT.instance.get_mission_require_signature(int.Parse(Risk_Arr[1]), SubSysNo.CH001);

                            if (update_mission_new_require_sign_rule(m_id, new_require_sign).excute_result.isSuccesed)
                            {
                                /// 更新完新簽核條件後所執行的動作
                                Record_Signed_New_RiskValue(m_id, Risk_Arr[1]); // 記錄目前風險值
                            }
                        }
                    }
                    else
                    {
                        Data_Set_Excutre_Result compare_reisk_result = new Data_Set_Excutre_Result();
                        compare_reisk_result.excute_result = new Data_Set_Result();
                        compare_reisk_result.excute_result.isSuccesed = false;
                        compare_reisk_result.excute_result.isError = true;
                        compare_reisk_result.excute_result.result = "失敗";
                        compare_reisk_result.excute_result.feedb_back_message = "判斷風險值失敗，請聯絡【研發中心-郁宸】";
                        compare_reisk_result.excute_result.fail_reason = "無指定的條件";
                        return compare_reisk_result;
                    }
                }


                /// 保存至DB
                {
                    string NewhistoryRecord = string.Format("{0} 於 {1} 註記：【{2}】 簽核結果：【{3}】", data_set_employee.name, DateTime.Now, history, "通過");
                    string history_record = mission_data.history + "\r\n" + NewhistoryRecord; // 把舊紀錄與新紀錄串接

                    Record_Signed_History(now_sign_person_data, m_id); // 紀錄目前所有簽核
                    Record_Sign_history_Reason(m_id, history_record); // 紀錄備註
                    Record_Sign_Ststus_ID(m_id, 2.ToString()); // 紀錄目前狀態
                    return update_mission_new_signature(m_id, list_data_set_sign); ;
                }
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }

        public abstract string Compare_New_Risk(string amount);


        public Data_Set_Excutre_Result excute_sign_done(Data_Set_Employee data_set_employee, string m_id, string history)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {

                Data_Set_Mission_Details mission_data = get_mission_information(m_id);
                List<Data_Set_Mission_Collect_Signed> data_set_sign_collect = mission_data.collect_signed;
                /// 保存已簽過的人
                List<Data_Set_Sign> list_data_set_sign = new List<Data_Set_Sign>();
                {
                    foreach (Data_Set_Mission_Collect_Signed collect_signed_data in data_set_sign_collect)
                    {
                        if (!collect_signed_data.public_key.Equals(""))
                        {
                            Data_Set_Sign save_collec_data = new Data_Set_Sign();
                            save_collec_data.e_id = collect_signed_data.e_id;
                            save_collec_data.public_key = collect_signed_data.public_key;
                            save_collec_data.company = collect_signed_data.company;
                            save_collec_data.department = collect_signed_data.department;
                            save_collec_data.employee_level = collect_signed_data.employee_level;
                            save_collec_data.sign_time = collect_signed_data.sign_time;
                            save_collec_data.sign_status = collect_signed_data.sign_status;
                            list_data_set_sign.Add(save_collec_data);
                        }
                    }
                }
                /// 儲存現在簽的人
                Data_Set_Sign now_sign_person_data = new Data_Set_Sign();
                {
                    now_sign_person_data.e_id = data_set_employee.e_id;
                    now_sign_person_data.public_key = data_set_employee.publickey;
                    now_sign_person_data.company = data_set_employee.company;
                    now_sign_person_data.department = data_set_employee.department;
                    now_sign_person_data.employee_level = data_set_employee.employee_level;
                    now_sign_person_data.sign_time = DateTime.Now.ToString();
                    now_sign_person_data.sign_status = "pass";
                    list_data_set_sign.Add(now_sign_person_data);
                }

                /// 保存至DB
                {

                    if (get_mission_status_id(list_data_set_sign, mission_data.require_sign))
                    {

                        string NewHistoryRecord = string.Format("{0} 於 {1} 註記：【{2}】 簽核結果：【{3}】", data_set_employee.name, DateTime.Now, history, "通過");
                        string history_record = mission_data.history + "\r\n" + NewHistoryRecord; // 把舊紀錄與新紀錄串接

                        Record_Signed_History(now_sign_person_data, m_id);
                        Record_Sign_history_Reason(m_id, history_record); // 紀錄備註
                        Record_Sign_Ststus_ID(m_id, 3.ToString());
                        return update_mission_new_signature(m_id, list_data_set_sign); ;
                    }
                    else
                    {
                        Data_Set_Excutre_Result sign_done_result = new Data_Set_Excutre_Result();
                        sign_done_result.excute_result = new Data_Set_Result();
                        sign_done_result.excute_result.isSuccesed = false;
                        sign_done_result.excute_result.isError = true;
                        sign_done_result.excute_result.result = "失敗";
                        sign_done_result.excute_result.fail_reason = "尚有簽章未蒐集，請檢查；若無法排除請聯絡【研發中心-郁宸】。";
                        sign_done_result.excute_result.feedb_back_message = "尚未把簽章蒐集完成";
                        return sign_done_result;
                    }

                }
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }


        /// <summary>
        /// 判斷此簽核任務是否已完成【所有需求簽核】，以及人員【是否是已簽核中最高的層級】。
        /// </summary>
        /// <param name="list_mission_collect_sign"></param>
        /// <param name="employee_data"></param>
        /// <returns></returns>
        public bool get_mission_requiresign_employee_level_max(Data_Set_Mission_Details mission_details_data, Data_Set_Employee employee_data)
        {
            if (employee_data.employee_level != null)
            {
                /// 判斷是否是最高層級
                /// 5最小 1最大
                int max_level = 5;
                foreach (Data_Set_Mission_require signed_data in mission_details_data.require_sign)
                {
                    int signed_current_level = int.Parse(signed_data.employee_level);
                    if (signed_current_level < max_level)
                    {
                        max_level = signed_current_level;
                    }

                }

                if (max_level == int.Parse(get_employee_level(employee_data.e_id, mission_details_data.mission_type)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 檢核還有欠缺多少簽
        /// </summary>
        /// <param name="list_mission_collect_sign"></param>
        /// <param name="list_mission_require_sign"></param>
        /// <returns></returns>
        public bool get_mission_status_id(List<Data_Set_Sign> list_mission_collect_sign, List<Data_Set_Mission_require> list_mission_require_sign)
        {
            /// 計算每一層級共簽核了幾個，剩下幾個還沒簽。
            foreach (Data_Set_Mission_require mission_require in list_mission_require_sign)
            {
                foreach (Data_Set_Sign mission_signed in list_mission_collect_sign)
                {
                    if (mission_require.company.Equals(mission_signed.company))
                    {
                        if (mission_require.department.Equals(mission_signed.department))
                        {
                            if ((mission_require.employee_level.Equals(mission_signed.employee_level)) && mission_signed.sign_status.Equals("pass"))
                            {
                                mission_require.amount = (int.Parse(mission_require.amount) - 1).ToString();
                            }
                        }
                    }
                }
            }
            foreach (Data_Set_Mission_require mission_require in list_mission_require_sign.ToArray())
            {
                if (int.Parse(mission_require.amount) == 0)
                {
                    list_mission_require_sign.Remove(mission_require);
                }
            }

            switch (list_mission_require_sign.Count)
            {
                case 0:
                    return true;
                default:
                    return false;
            }

        }


        /// <summary>
        /// 獲取職員相關訊息
        /// </summary>
        /// <param name="e_id">職員代號</param>
        /// <returns>Data_Set_Employee</returns>
        public Data_Set_Employee get_employee_information(string e_id)
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data data = sql.get_employee_information(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_EMPLOYEE_INFORMATION, e_id);
            return dao_post_process.set_Employee_data(data);
        }


        /// <summary>
        /// 獲取職員已簽核完畢之相關訊息
        /// </summary>
        /// <param name="signed_info"> 簽核資訊-> e_id + publickey</param>
        /// <returns>List<Data_Set_Sign_History></returns>
        public List<Data_Set_Sign_History> get_employee_signed_information(string signed_info)
        {
            Data_Set_Excutre_Result result = CompareTokenvalid();

            if (result.excute_result.isSuccesed)
            {
                SQL sql = new SQL();
                Data_Set_DAO_Data data = sql.get_employee_signed_signature(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_EMPLOYEE_SIGNED_SIGNATURE, signed_info);
                return dao_post_process.set_Employee_signed_data(data);
            }
            else
            {
                throw new ArgumentException("請先登入");
            }
        }





        /// <summary>
        /// 獲取職員的代理人之訊息
        /// </summary>
        /// <param name="e_id">職員代號</param>
        /// <returns>string</returns>
        public string get_employee_agent_information(string e_id)
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data data = sql.get_employee_information(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_EMPLOYEE_INFORMATION, e_id);
            return dao_post_process.set_Employee_data(data).agent;
        }

        public List<Data_Set_Mission_Details> get_mission_information_all() ///之後要做 private 此為雛形開發用
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data mission_all_data = sql.get_mission_information_All(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_MISSION_ALL);

            return dao_post_process.set_Mission_All(mission_all_data);
        }

        public Data_Set_Mission_Details get_mission_information(string m_id)
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data mission_data = sql.get_mission_information(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_MISSION_SPECIFIC, m_id);

            return dao_post_process.set_Mission(mission_data);
        }

        public Data_Set_Excutre_Result update_mission_new_signature(string m_id, List<Data_Set_Sign> list_signature)
        {
            SQL sql = new SQL();
            string signature_data = string.Format("{0},,,,,{1}", m_id, JsonConvert.SerializeObject(list_signature));
            Data_Set_DAO_Data sign_result_data = sql.update_mission_signature(Sql_Action_Category_Option.UPDATE, Sql_Action_Option.UPDATE_MISSION_SIGNATURE, signature_data);

            return dao_post_process.set_Sign_Result(sign_result_data);
        }
        public Data_Set_Excutre_Result update_mission_new_require_sign_rule(string m_id, string new_require_sign_rule)
        {
            SQL sql = new SQL();
            string new_require_sign_rule_data = string.Format("{0},,,,,{1}", m_id, new_require_sign_rule);
            Data_Set_DAO_Data sign_result_data = sql.update_mission_new_require_sign(Sql_Action_Category_Option.UPDATE, Sql_Action_Option.UPDATE_MISSION_NEW_REQUIRE_SIGN, new_require_sign_rule_data);

            return dao_post_process.set_Update_Mission_Require_Sign_Result(sign_result_data);
        }
        public void Record_Signed_History(Data_Set_Sign now_sign_person_data, string m_id)
        {
            string value = string.Format("{0},,,,,{1},,,,,{2},,,,,{3}", m_id, now_sign_person_data.public_key, now_sign_person_data.sign_time, now_sign_person_data.sign_status);
            SQL sql = new SQL();
            Data_Set_DAO_Data record_signed_history_result = sql.set_record_signed_history(Sql_Action_Category_Option.SET, Sql_Action_Option.SET_RECORD_SIGNED_HISTORY, value);
        }
        public void Record_Sign_Ststus_ID(string m_id, string status_id)
        {
            SQL sql = new SQL();
            sql.update_mission_status_id(Sql_Action_Category_Option.UPDATE, Sql_Action_Option.UPDATE_MISSION_STATUS_ID, string.Format("{0},,,,,{1}", m_id, status_id));
        }
        public void Record_Signed_New_RiskValue(string m_id, string new_risk_value)
        {
            SQL sql = new SQL();
            sql.update_mission_new_riskvalue(Sql_Action_Category_Option.UPDATE, Sql_Action_Option.UPDATE_MISSION_NEW_RISK_VALUE, String.Format("{0},,,,,{1}", m_id, new_risk_value));
        }

        public string register_new_mission(Mission mission)
        {
            if (!CompareExistingBindingProjectID(mission))
            {
                string register_mission_data =
                mission.data_set_mission.m_id
                + ",,,,," + mission.data_set_mission.name + " 第" + ComputingExistingFailMission(mission).ToString() + "次審批"
                + ",,,,," + mission.data_set_mission.status_id
                + ",,,,," + mission.data_set_mission.company
                + ",,,,," + mission.data_set_mission.binding_project_id
                + ",,,,," + mission.data_set_mission.require_sign_json
                + ",,,,," + mission.data_set_mission.collect_signed_json
                + ",,,,," + mission.data_set_mission.extends
                + ",,,,," + mission.data_set_mission.mission_type
                + ",,,,," + mission.data_set_mission.risk_value;

                SQL sql = new SQL();

                string excute_result = sql.set_register_new_mission(Sql_Action_Category_Option.SET, Sql_Action_Option.REGISTER_NEW_MISSION, register_mission_data);
                return excute_result;
            }
            else
            {
                Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
                result.excute_result = new Data_Set_Result();
                result.excute_result.isSuccesed = false;
                result.excute_result.feedb_back_message = string.Format(ErrorMsg("已存在尚未完成的任務，請先完成已存在任務。"));
                return JsonConvert.SerializeObject(result);
            }
        }
        public bool CompareExistingBindingProjectID(Mission mission)
        {
            Controller_Sign sign = Controller_Sign.getInstance();

            List<Data_Set_Mission_Details> AllMissionList = sign.get_mission_information_all();
            AllMissionList = AllMissionList.Where(x => x.binding_project_id.Equals(mission.data_set_mission.binding_project_id) && x.company.Equals(mission.data_set_mission.company)).ToList();

            foreach (Data_Set_Mission_Details mission_data in AllMissionList)
            {
                if (!mission_data.status_id.Equals("99"))
                {
                    return true;
                }
            }
            return false;
        }
        public int ComputingExistingFailMission(Mission mission)
        {
            int count = 1;
            Controller_Sign sign = Controller_Sign.getInstance();

            List<Data_Set_Mission_Details> AllMissionList = sign.get_mission_information_all();
            AllMissionList = AllMissionList.Where(x => x.binding_project_id.Equals(mission.data_set_mission.binding_project_id) && x.company.Equals(mission.data_set_mission.company)).ToList();

            foreach (Data_Set_Mission_Details mission_data in AllMissionList)
            {
                if (mission_data.status_id.Equals("99"))
                {
                    count++;
                }
            }
            return count;
        }
        public string get_total_mission_count()
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data count_data = sql.get_mission_total_count(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_EMPLOYEE_INFORMATION);
            return dao_post_process.get_Mission_Total_Count(count_data);

        }
        public Data_Set_Excutre_Result register_new_mission_type(Data_Set_Mission_Type data_set_mission_type)
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data register_new_mission_type_result = sql.set_register_new_mission_type(Sql_Action_Category_Option.SET, Sql_Action_Option.REGISTER_NEW_MISSION_TYPE, String.Format("{0},,,,,{1},,,,,{2},,,,,{3}", data_set_mission_type.company, data_set_mission_type.department, data_set_mission_type.mission_type_id, data_set_mission_type.summary));
            return dao_post_process.set_register_new_mission_type_result(register_new_mission_type_result);
        }

        public List<Data_Set_Mission_Type> get_mission_type_information(Data_Set_Employee data_set_employee)
        {
            SQL sql = new SQL();
            Data_Set_DAO_Data get_mission_type_result = sql.get_mission_type_information(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_MISSION_TYPE, String.Format("{0},,,,,{1}", data_set_employee.company, data_set_employee.department));
            return dao_post_process.set_mission_type_result(get_mission_type_result);
        }

        /// <summary>
        /// 過濾剩下需要多少簽，list版本。
        /// </summary>
        /// <param name="list_mission_data"></param>
        /// <returns></returns>
        public List<Data_Set_Mission_Details> filter_Mission_Done_RequireAmount(List<Data_Set_Mission_Details> list_mission_data)
        {

            foreach (Data_Set_Mission_Details mission_data in list_mission_data)
            {
                /// 計算每一層級共簽核了幾個，剩下幾個還沒簽。
                foreach (Data_Set_Mission_require mission_require in mission_data.require_sign)
                {
                    foreach (Data_Set_Mission_Collect_Signed mission_signed in mission_data.collect_signed)
                    {
                        if (mission_require.company.Equals(mission_signed.company))
                        {
                            if (mission_require.department.Equals(mission_signed.department))
                            {
                                if (mission_require.employee_level.Equals(mission_signed.employee_level))
                                {
                                    if (mission_signed.sign_status.Equals("pass"))
                                    {
                                        mission_require.amount = (int.Parse(mission_require.amount) - 1).ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (Data_Set_Mission_require mission_require in mission_data.require_sign.ToArray())
                {
                    if ((int.Parse(mission_require.amount) == 0))
                    {
                        mission_data.require_sign.Remove(mission_require);
                    }
                }
            }
            return list_mission_data;
        }

        /// <summary>
        /// 過濾剩下需要多少簽，單筆資料查詢。
        /// </summary>
        /// <param name="mission_data"></param>
        /// <returns></returns>
        public string filter_Mission_Done_RequireAmount(Data_Set_Mission_Details mission_data)
        {
            /// 計算每一層級共簽核了幾個，剩下幾個還沒簽。
            foreach (Data_Set_Mission_require mission_require in mission_data.require_sign)
            {
                foreach (Data_Set_Mission_Collect_Signed mission_signed in mission_data.collect_signed)
                {
                    if (mission_require.company.Equals(mission_signed.company))
                    {
                        if (mission_require.department.Equals(mission_signed.department))
                        {
                            if (mission_require.employee_level.Equals(mission_signed.employee_level))
                            {
                                if (mission_signed.sign_status.Equals("pass"))
                                {
                                    mission_require.amount = (int.Parse(mission_require.amount) - 1).ToString();
                                }
                            }
                        }
                    }
                }
            }
            foreach (Data_Set_Mission_require mission_require in mission_data.require_sign.ToArray())
            {
                if ((int.Parse(mission_require.amount) == 0))
                {
                    mission_data.require_sign.Remove(mission_require);
                }
            }

            return mission_data.require_sign.Count.ToString();
        }


        public bool Existing_Fail_Signed(Data_Set_Mission_Details mission_data)
        {
            foreach (Data_Set_Mission_Collect_Signed collect_signed_data in mission_data.collect_signed)
            {
                if (collect_signed_data.sign_status.Equals("fail"))
                {
                    return true;
                }
            }
            return false;
        }



        public bool get_compare_level_sequence(Data_Set_Employee employee_data, List<Data_Set_Mission_Collect_Signed> list_mission_collect_signature, List<Data_Set_Mission_require> list_mission_require_signature)
        {

            /// 判斷是否包含他的簽名
            foreach (Data_Set_Mission_Collect_Signed collect_signed in list_mission_collect_signature)
            {
                if ((collect_signed.public_key.Equals(employee_data.publickey) && collect_signed.sign_status.Equals("pass")) && collect_signed.employee_level.Equals(employee_data.employee_level))
                {
                    return false;
                }
            }

            /// 判別是否換此職員簽核
            foreach (Data_Set_Mission_require mission_require in list_mission_require_signature)
            {
                if (float.Parse(mission_require.employee_level) == float.Parse(employee_data.employee_level))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return false;
        }

        public Data_Set_Employee set_employee_mission_information(Data_Set_Employee data_set_employee, string value)
        {
            return new Data_Set_Employee();
        }

        public Data_Set_Employee set_employee_signed_information(Data_Set_Employee data_set_employee, string value)
        {
            return new Data_Set_Employee();
        }

        public Data_Set_Employee set_employee_prepare_sign_information(Data_Set_Employee data_set_employee, string value)
        {
            return new Data_Set_Employee();
        }

        public Data_Set_Employee set_employee_agent_information(Data_Set_Employee data_set_employee, string value)
        {
            return new Data_Set_Employee();
        }

        public string update_mission_data(string m_id)
        {
            return "";
        }

        public string register_mission_status_summary(Data_Set_Mission_Status data_set_mission_status)
        {
            return "";
        }



        public Data_Set_Mission set_mission_object_data(Set_Mission_Data_Option set_option, string value)
        {
            return data_set_mission;
        }

        public string get_mission_require_signature(int risk_value, SubSysNo subSysNo)
        {
            return risk_value.ToString();
        }
        public string get_employee_level(string account, string mission_type)
        {
            foreach (Data_Set_Mission_Employee_Level mission_employee_level_data in data_set_mission_employee_level)
            {
                if (mission_employee_level_data.mission_type.Equals(mission_type))
                {
                    foreach (Data_Set_Employee_Account employee_account in mission_employee_level_data.account_list)
                    {
                        if (employee_account.account.Equals(account))
                        {
                            return mission_employee_level_data.employee_level;
                        }
                    }
                }
            }
            return "此帳號在此案件中沒有對應的簽核層級，請聯絡【研發中心-郁宸】進行調整。";
        }


        private List<Data_Set_Mission_Employee_Level> Load_Mission_Employee()
        {
            List<Data_Set_Mission_Employee_Level> list_mission_employee = new List<Data_Set_Mission_Employee_Level>();

            SQL sql = new SQL();
            Data_Set_DAO_Data get_mission_employee_level_result = sql.get_mission_employee_data(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_MISSION_EMPLOYEE_LEVEL);
            list_mission_employee = dao_post_process.set_mission_employee_level(get_mission_employee_level_result);
            return list_mission_employee;
        }

        public void Load_Employee_Level_Reference()
        {
            data_set_mission_employee_level = Load_Mission_Employee();
        }

        #region 用不到
        public void initail_mission_object()
        {
            throw new NotImplementedException();
        }
        public Data_Set_Mission_Type set_mission_type_data(Set_Mission_Type_Option Set_Mission_Type_Option, string value)
        {
            throw new NotImplementedException();
        }
        public Data_Set_Mission initiail_collect_signed(Data_Set_Mission data_set_mission)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Excutre_Result login(string account, string password)
        {
            Account = account;
            Password = password;
            Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
            SQL sql = new SQL();

            Data_Set_DAO_Data data = sql.get_login_compare(Sql_Action_Category_Option.GET, Sql_Action_Option.GET_LOGIN_COMPARE, String.Format("{0},,,,,{1}", Account, Password));
            Data_Set_ReceiveMainSystem_LoginData employee_data = dao_post_process.set_Login_Info(data);
            if (employee_data.excute_result.isSuccesed)
            {
                result = TakeNewToken();
            }
            else
            {
                result.excute_result = employee_data.excute_result;
            }
            return result;
        }

        public Data_Set_Excutre_Result TakeNewToken()
        {
            Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
            result.excute_result = new Data_Set_Result();


            try
            {
                DAO_Token DaoToken = DAO_Token.getInstance();
                Current_Token.token = DaoToken.EncryptionProcess(Account, Password);
                Current_Token.Expired = DateTime.Now.AddMinutes(5).ToString();

                result.excute_result.isSuccesed = true;
                result.excute_result.result = "成功";
                result.excute_result.feedb_back_message = "成功取得 Token。";

            }
            catch (Exception ex)
            {

                result.excute_result.isSuccesed = false;
                result.excute_result.isError = true;
                result.excute_result.result = "失敗";
                result.excute_result.fail_reason = ex.Message;
            }
            return result;
        }

        public Data_Set_Excutre_Result CompareTokenvalid()
        {
            Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
            result.excute_result = new Data_Set_Result();
            result.excute_result.isSuccesed = true;
            result.excute_result.feedb_back_message = "Token 仍然保持有效";
            result.excute_result.isError = false;
            return result;
            //{
            //    Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
            //    result.excute_result = new Data_Set_Result();
            //    try
            //    {
            //        DAO_Token DaoToken = DAO_Token.getInstance();
            //        Current_Token.token = DaoToken.EncryptionProcess(Account, Password);
            //        DateTime Current_Token_Expired = DateTime.Parse(Current_Token.Expired);
            //        DateTime Current_Time = DateTime.Now;

            //        long delta_millsecond = (long)(Current_Token_Expired - Current_Time).TotalMilliseconds;


            //        if (delta_millsecond > 0)
            //        {
            //            result.excute_result.isSuccesed = true;
            //            result.excute_result.feedb_back_message = "Token 仍然保持有效";
            //            result.excute_result.isError = false;
            //        }
            //        else
            //        {
            //            result.excute_result.isSuccesed = false;
            //            result.excute_result.feedb_back_message = "Token 已過期請重新登入。";
            //            result.excute_result.fail_reason = "Token 時間過期";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        result.excute_result.isSuccesed = false;
            //        result.excute_result.feedb_back_message = String.Format("尚未登入，請登入");
            //        result.excute_result.fail_reason = String.Format("錯誤原因：{0}", ex.Message);
            //    }
            //    return result;
            //}
        }


        public Data_Set_Excutre_Result logout()
        {
            Data_Set_Excutre_Result result = new Data_Set_Excutre_Result();
            result.excute_result = new Data_Set_Result();


            try
            {
                DAO_Token DaoToken = DAO_Token.getInstance();
                Current_Token.token = DaoToken.EncryptionProcess(Account, Password);
                Current_Token.Expired = DateTime.Now.AddMinutes(-10).ToString();

                result.excute_result.isSuccesed = true;
                result.excute_result.result = "成功";
                result.excute_result.feedb_back_message = "成功登出。";

            }
            catch (Exception ex)
            {

                result.excute_result.isSuccesed = false;
                result.excute_result.isError = true;
                result.excute_result.result = "失敗";
                result.excute_result.fail_reason = ex.Message;
            }
            return result;
        }

        public bool get_mission_requiresign_employee_level_max(List<Data_Set_Sign> list_mission_collect_sign, List<Data_Set_Mission_require> list_mission_require_sign, Data_Set_Employee employee_data)
        {
            throw new NotImplementedException();
        }

        public string get_mission_current_sign_amount(string m_d)
        {


            return "";
        }

        #region 錯誤訊息提示
        public string ErrorMsg(string Content)
        {
            return string.Format("{0}\r\n{1}", Content, "若問題持續無法解決，請聯繫【研發中心-郁宸】");
        }
        public string ErrorMsg(string Content, string error_code)
        {
            return string.Format("錯誤代號：{0} \r\n {1} \r\n {2}", error_code, Content, "若問題持續無法解決，請聯繫【研發中心-郁宸】");
        }
        #endregion  




        #endregion

    }
}
