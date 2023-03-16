using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PTMB_Signature_API.Data_Set;
using PTMB_Signature_API.Informatio_Set;
using PTMB_Signature_API.Model.Abstract;
using PTMB_Signature_API.Model.Implement;

namespace PTMB_Signature_API
{
    public partial class Development_Demo_Form : Form
    {
        public Development_Demo_Form()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string PlainText = textBox1.Text;
            using (Aes aesAlg = Aes.Create())
            {
                //加密金鑰(32 Byte)
                aesAlg.Key = Encoding.Unicode.GetBytes("每個失落的難關，都是善意的安排。");
                //初始向量(Initial Vector, iv) 類似雜湊演算法中的加密鹽(16 Byte)
                aesAlg.IV = Encoding.Unicode.GetBytes("以身作則以德服人");
                //加密器
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                //執行加密
                byte[] encrypted = encryptor.TransformFinalBlock(Encoding.Unicode.GetBytes(PlainText), 0,
                Encoding.Unicode.GetBytes(PlainText).Length);

                textBox1.Text =  Convert.ToBase64String(encrypted);
            }

        }

        private string Decryption(string CipherText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    //加密金鑰(32 Byte)
                    aesAlg.Key = Encoding.Unicode.GetBytes("每個失落的難關，都是善意的安排。");
                    //初始向量(Initial Vector, iv) 類似雜湊演算法中的加密鹽(16 Byte)
                    aesAlg.IV = Encoding.Unicode.GetBytes("以身作則以德服人");
                    //加密器
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    //執行解密
                    byte[] decrypted = decryptor.TransformFinalBlock(Convert.FromBase64String(CipherText), 0, Convert.FromBase64String(CipherText).Length);
                    return Encoding.Unicode.GetString(decrypted);
                }
            }
            catch (Exception)
            {

                return "金鑰、密鑰並匹配，請重新確認。";
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            //連接字串產生器
            SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SubSystem_Sign"].ConnectionString);

            //資料庫使用者帳號解密
            sqlsb.UserID = Decryption(sqlsb.UserID);

            //資料庫使用者密碼解密
            sqlsb.Password = Decryption(sqlsb.Password);

            //簡單連線資料庫查詢SQL Server版本資料
            using (SqlConnection conn = new SqlConnection(sqlsb.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select @@version", conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    MessageBox.Show($"{table.Rows[0][0]}");
                }
            }
        }

        private void Get_Employee_Information_Click(object sender, EventArgs e)
        {
            // Example 
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee employee_data = sign.get_employee_information("P0023");

            // Show Result
            MessageBox.Show(string.Format("{0},{1},{2},{3},{4},{5}",employee_data.e_id,employee_data.company,employee_data.employee_level,employee_data.department,employee_data.excute_result.result,employee_data.publickey));
        }
        private void Get_Employee_Signed_Information_Button_Click_1(object sender, EventArgs e)
        {
            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            foreach (Data_Set_Sign_History signed_history in sign.get_employee_signed_information("51A89957B0F0F1113FBCD90DE84E4AE68F7AD28B"))
            {
                // Show Result
                MessageBox.Show(string.Format("{0} 已前核的任務有：{1}", "P0023", signed_history.mission_id));
            }
        }
        private void Get_Employee_Signed_Information_Button_Click(object sender, EventArgs e)
        {
            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            string agent_data = sign.get_employee_agent_information("P0023");

            // Show Result
            MessageBox.Show(string.Format("{0} 今日代理人為：{1}","P0023",agent_data));
        }

        private void Mission_Register_Button_Click(object sender, EventArgs e)
        {
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Excutre_Result result = sign.CompareTokenvalid();
            if (result.excute_result.isSuccesed)
            {
                try
                {
                    LOANIT loanit = LOANIT.getinstance();

                    loanit.initail_mission_object();
                    loanit.set_mission_object_data(Set_Mission_Data_Option.NAME, Mission_Name_Textbox.Text);
                    loanit.set_mission_object_data(Set_Mission_Data_Option.TYPE, SubSysNo.CH001.ToString());
                    loanit.set_mission_object_data(Set_Mission_Data_Option.COMPANY, Mission_Company_Textbox.Text);
                    loanit.set_mission_object_data(Set_Mission_Data_Option.BINDING_PROJECT_ID, Mission_Binding_ProjectID_Textbox.Text);
                    loanit.set_mission_object_data(Set_Mission_Data_Option.RISK_VALUE, comboBox1.SelectedItem.ToString());
                    loanit.set_mission_object_data(Set_Mission_Data_Option.require_SIGNATURE, loanit.get_mission_require_signature(int.Parse(comboBox1.SelectedItem.ToString()), SubSysNo.CH001)); //填入風險係數
                    //loanit.set_mission_object_data(Set_Mission_Data_Option.require_SIGNATURE, loanit.get_mission_require_signature(5)); //填入風險係數

                    Data_Set_Excutre_Result excute_result = JsonConvert.DeserializeObject<Data_Set_Excutre_Result>(sign.register_new_mission(loanit));
                    MessageBox.Show(String.Format("任務 {0} 已新增完成", loanit.data_set_mission.name));

                    DemoPluginFunction demoPlugin = new DemoPluginFunction();
                    demoPlugin.SendRegisterMissionMail(loanit, 3);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("錯誤\r\n原因為：{0}", ex.Message));
                }
            }
            else
            {
                Login_ConsoleLog_Textbox.Text += String.Format("{0}\r\n", result.excute_result.feedb_back_message);
            }
        }

        private void Get_Mission_Information_Button_Click(object sender, EventArgs e)
        {
            // Example            Console.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            Controller_Sign sign = Controller_Sign.getInstance();
            List<Data_Set_Mission_Details> list_mission_data = sign.get_mission_information_all();

            // Show Result
            foreach (Data_Set_Mission_Details mission in list_mission_data)
            {
                Console.Text += String.Format("-----任務資訊-----\r\n");
                Console.Text += String.Format("任務代號：{0}\r\n", mission.m_id);
                Console.Text += String.Format("任務名稱：{0}\r\n",mission.name);
                Console.Text += String.Format("任務公司：{0}\r\n", mission.company);
                Console.Text += String.Format("任務對應專案：{0}\r\n", "需有以下人員的簽核");

                foreach (Data_Set_Mission_require require_signature in mission.require_sign)
                {
                    Console.Text += String.Format("{1}.公司：{0} ||| ", require_signature.company, mission.require_sign.IndexOf(require_signature)+1);
                    Console.Text += String.Format("部門：{0} ||| ", require_signature.department);
                    Console.Text += String.Format("職員層級：{0} ||| ", require_signature.employee_level);
                    Console.Text += String.Format("簽章數量：{0} ||| \r\n", require_signature.amount);
                }

                if(mission.collect_signed.Count >= 1 && !mission.collect_signed[0].company.Equals(""))
                {
                    foreach (Data_Set_Mission_Collect_Signed collect_signed in mission.collect_signed)
                    {
                        Console.Text += String.Format("{1}.公司：{0} ||| ", collect_signed.company, mission.collect_signed.IndexOf(collect_signed) + 1);
                        Console.Text += String.Format("部門：{0} ||| ", collect_signed.department);
                        Console.Text += String.Format("職員層級：{0} ||| ", collect_signed.employee_level);
                    }
                }
                else
                {
                    Console.Text += String.Format("目前尚未有人簽核，請盡快通知。\r\n\r\n");
                }

            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Console2.Text = "";
            Infromation_Mission_Listbox.Items.Clear();
            Mission_Require_Sign_Listbox.Items.Clear();
            Mission_Signed_Sign_Listbox.Items.Clear();
            Mission_Fail_Sign_Listbox.Items.Clear();

            // Example 
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee employee_data = sign.get_employee_information(employee_id.Text);
            List<Informatio_Set.Information_Mission> list_information_mission = sign.get_employee_mission_information_all(employee_id.Text);
            List<Informatio_Set.Information_Mission> list_information_mission_requirment_signature = sign.get_employee_mission_information_requirment(employee_id.Text);


            // Show Result

            // 全部任務框
            Console2.Text += string.Format("全部的案件如下：\r\n");
            foreach (Informatio_Set.Information_Mission information_mission in list_information_mission)
            {
                Console2.Text += string.Format("{1}. 案件編號：{0} |||", information_mission.mission_id, list_information_mission.IndexOf(information_mission) + 1);
                Console2.Text += string.Format(" 對應專案：{0} |||", information_mission.binding_project_id);
                Console2.Text += string.Format(" 案件狀態：{0}\r\n", information_mission.mission_status);

                Infromation_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}", information_mission.mission_id, information_mission.binding_project_id));
            }

            // 需簽核框
            foreach (Informatio_Set.Information_Mission information_mission_requirment_signature in list_information_mission_requirment_signature)
            {
                Mission_Require_Sign_Listbox.Items.Add(string.Format("{0}. C_KEY：{1}", information_mission_requirment_signature.mission_id, information_mission_requirment_signature.binding_project_id));
            }

            // 已完成框
            foreach (Data_Set_Sign_History signed_history in sign.get_employee_signed_information(employee_data.publickey))
            {
                // Show Result
                Mission_Signed_Sign_Listbox.Items.Add(signed_history.mission_id);
            }

            // 簽核失敗
            foreach (Data_Set_Mission_Details fail_mission in sign.get_employee_mission_information_fail(employee_data))
            {
                // Show Result
                Mission_Fail_Sign_Listbox.Items.Add(fail_mission.m_id);
            }

            foreach (Information_Mission inoformation_mission_require_done in sign.get_employee_mission_information_requirement_done(employee_data.e_id))
            {
                Mission_Require_Done_Listbox.Items.Add(inoformation_mission_require_done.mission_id);
            }

        }

        private void Get_Mission_Requirment_Signature_Button_Click(object sender, EventArgs e)
        {
            Requirment_Signature_Mission_Listbox.Items.Clear();

            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee employee_data = sign.get_employee_information(employee_id_2.Text);
            List<Informatio_Set.Information_Mission> list_information_mission_requirment_signature = sign.get_employee_mission_information_requirment(employee_id_2.Text);
            List<Informatio_Set.Information_Mission> list_information_mission_done_signature = sign.get_employee_mission_information_requirement_done(employee_id_2.Text);
            // ShowResult
            foreach (Informatio_Set.Information_Mission information_mission_requirment_signature in list_information_mission_requirment_signature)
            {
                Requirment_Signature_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}", information_mission_requirment_signature.mission_id, information_mission_requirment_signature.binding_project_id));
            }
            foreach (Informatio_Set.Information_Mission information_mission_requirment_done_signaturein in list_information_mission_done_signature)
            {
                Requirment_Signature_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}   需核定", information_mission_requirment_done_signaturein.mission_id, information_mission_requirment_done_signaturein.binding_project_id));
            }
            Employee_Company_Label.Text = employee_data.company;
            Employee_Department_Label.Text = employee_data.department;
            Employee_Level_Label.Text = employee_data.employee_level;
        }

        private void Sign_Mission_Pass_Click(object sender, EventArgs e)
        {
            string mission_summary_data = Requirment_Signature_Mission_Listbox.SelectedItem.ToString();
            string[] mission_data_arr = mission_summary_data.Split('.');
            string record_data = string.Format("{0},,,,,{1},,,,,{2},,,,,{3},,,,,{4},,,,,{5}", history_Textbox.Text, Edit_Amount_Textbox.Text, Loan_Rate_Textbox.Text, Process_Ratio_Textbox.Text,Advise_Textbox.Text,Suggestion_Texrbox.Text);
            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee data_set_employee = sign.get_employee_information(employee_id_2.Text);
            Data_Set_Excutre_Result sign_result = sign.excute_sign_pass(data_set_employee, mission_data_arr[0],record_data,sign.Compare_New_Risk("5000000")); /// 簽核執行

            // ShowResult
            
            // 簽核結果提示
            MessageBox.Show(sign_result.excute_result.feedb_back_message);

            // Refresh 原先的簽核清單
            List<Informatio_Set.Information_Mission> list_information_mission_requirment_signature = sign.get_employee_mission_information_requirment(employee_id_2.Text);
            Requirment_Signature_Mission_Listbox.Items.Clear();
            foreach (Informatio_Set.Information_Mission information_mission_requirment_signature in list_information_mission_requirment_signature)
            {
                Requirment_Signature_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}", information_mission_requirment_signature.mission_id, information_mission_requirment_signature.binding_project_id));
            }

        }

        private void Sign_Mission_Done_Click(object sender, EventArgs e)
        {
            string mission_summary_data = Requirment_Signature_Mission_Listbox.SelectedItem.ToString();
            string[] mission_data_arr = mission_summary_data.Split('.');

            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee data_set_employee = sign.get_employee_information(employee_id_2.Text);
            Data_Set_Excutre_Result sign_result = sign.excute_sign_done(data_set_employee, mission_data_arr[0], history_Textbox.Text); /// 簽核執行

            // ShowResult

            // 簽核結果提示
            MessageBox.Show(sign_result.excute_result.feedb_back_message);

            // Refresh 原先的簽核清單
            List<Informatio_Set.Information_Mission> list_information_mission_requirment_signature = sign.get_employee_mission_information_requirment(employee_id_2.Text);
            Requirment_Signature_Mission_Listbox.Items.Clear();
            foreach (Informatio_Set.Information_Mission information_mission_requirment_signature in list_information_mission_requirment_signature)
            {
                Requirment_Signature_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}", information_mission_requirment_signature.mission_id, information_mission_requirment_signature.binding_project_id));
            }
        }


        private void Sign_Mission_Back_Click(object sender, EventArgs e)
        {
            string mission_summary_data = Requirment_Signature_Mission_Listbox.SelectedItem.ToString();
            string[] mission_data_arr = mission_summary_data.Split('.');

            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee data_set_employee = sign.get_employee_information(employee_id_2.Text);
            Data_Set_Excutre_Result sign_result = sign.excute_sign_back(data_set_employee, mission_data_arr[0],history_Textbox.Text); /// 簽核執行


            // ShowResult

            // 簽核結果提示
            MessageBox.Show(sign_result.excute_result.feedb_back_message);

            // Refresh 原先的簽核清單
            List<Informatio_Set.Information_Mission> list_information_mission_requirment_signature = sign.get_employee_mission_information_requirment(employee_id_2.Text);
            Requirment_Signature_Mission_Listbox.Items.Clear();
            history_Textbox.Text = "";
            foreach (Informatio_Set.Information_Mission information_mission_requirment_signature in list_information_mission_requirment_signature)
            {
                Requirment_Signature_Mission_Listbox.Items.Add(string.Format("{0}. 案件名稱：{1}", information_mission_requirment_signature.mission_id, information_mission_requirment_signature.binding_project_id));
            }
        }

        private void Mission_Type_Login_Click(object sender, EventArgs e)
        {
            Mission_Type_Select_Combobox.Items.Clear();

            if (Login_Account.SelectedItem.ToString().Equals("郁宸"))
            {
                Company_Label.Text = "PTMB";
                Dempartment_Lable.Text = "RD";
                Level_Label.Text = "4";
            }
            else if (Login_Account.SelectedItem.ToString().Equals("David")){
                Company_Label.Text = "PTMB";
                Dempartment_Lable.Text = "RD";
                Level_Label.Text = "1";
            }
            else if (Login_Account.SelectedItem.ToString().Equals("蕭邦"))
            {
                Company_Label.Text = "PTMB";
                Dempartment_Lable.Text = "RISK";
                Level_Label.Text = "1";
            }
            
            if(Level_Label.Text == "1")
            {
                Register_New_Mission_Type_Groupbox.Visible = true;
                Register_New_Mission_Type_Action_Groupbox.Visible = true;
                Mission_Type_Select_Groupbox.Visible = false;
            }
            else
            {

                Register_New_Mission_Type_Groupbox.Visible = true;
                Register_New_Mission_Type_Action_Groupbox.Visible = false;
                LOANIT loanit = LOANIT.getinstance();
                Controller_Sign sign = Controller_Sign.getInstance();
                Data_Set_Employee employee_data = sign.get_employee_information("P4444");
                loanit.mission_type_data_all =  sign.get_mission_type_information(employee_data);

                foreach (Data_Set_Mission_Type mission_data_data in loanit.mission_type_data_all)
                {
                    Mission_Type_Select_Groupbox.Visible = true;
                    Mission_Type_Select_Combobox.Items.Add(mission_data_data.mission_type_id);
                }
            }
        }

        private void Register_New_Mission_Type_Button_Click(object sender, EventArgs e)
        {
            // Example
            LOANIT loanit = LOANIT.getinstance();
            Controller_Sign sign = Controller_Sign.getInstance();
           
            loanit.initial_mission_type();
            loanit.set_mission_type_data(Set_Mission_Type_Option.COMPANY, Company_Label.Text);
            loanit.set_mission_type_data(Set_Mission_Type_Option.DEPARTMENT, Dempartment_Lable.Text);
            loanit.set_mission_type_data(Set_Mission_Type_Option.MISSION_TYPE_ID, Mission_Type_ID_Textbox.Text);
            loanit.set_mission_type_data(Set_Mission_Type_Option.SUMMARY, Mission_Type_Summary_Textbox.Text);

            // show Result
            Data_Set_Excutre_Result result_show = sign.register_new_mission_type(loanit.data_set_mission_type);
            MessageBox.Show(result_show.excute_result.isSuccesed + " " + result_show.excute_result.feedb_back_message);

        }

        private void Mission_Type_Select_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LOANIT loanit = LOANIT.getinstance();

            foreach (Data_Set_Mission_Type mission_type_data in loanit.mission_type_data_all)
            {
                if(mission_type_data.mission_type_id.Equals(Mission_Type_Select_Combobox.SelectedItem.ToString()))
                {
                    Mission_Type_Descrption_Textbox.Text = mission_type_data.summary;
                }
            }


        }

        private void Register_New_Mission_Login_button_Click(object sender, EventArgs e)
        {
            Mission_Type_Select_Combobox.Items.Clear();

            // Example
            Controller_Sign sign = Controller_Sign.getInstance();
            Data_Set_Employee employee_data = sign.get_employee_information(Register_New_Mission_Account_Combobox.SelectedItem.ToString());
            {
                LOANIT loanit = LOANIT.getinstance();
                loanit.mission_type_data_all = sign.get_mission_type_information(employee_data);

                // Show Result
                foreach (Data_Set_Mission_Type mission_data_data in loanit.mission_type_data_all)
                {
                    Register_New_Mission_Type_Combobbox.Items.Add(mission_data_data.mission_type_id);
                }
            }
        }

        private void Login_Button_Click(object sender, EventArgs e)
        {
            Controller_Sign sign = Controller_Sign.getInstance();
            sign.Account = Account_Textbox.Text;
            sign.Password = Password_Textbox.Text;

            Data_Set_Excutre_Result result = sign.login(sign.Account, sign.Password);
            if (result.excute_result.isSuccesed)
            {
                Login_ConsoleLog_Textbox.Text += string.Format("{0} Token過期時間：{1}\r\n", result.excute_result.feedb_back_message, sign.Current_Token.Expired);
                Data_Set_Employee employee_data = sign.get_employee_information(sign.Account);
                sign.get_mission_type_information(employee_data);
            }
            else
            {
                Login_ConsoleLog_Textbox.Text += string.Format("{0} \r\n", result.excute_result.feedb_back_message, sign.Current_Token.Expired);
            }

        }

        private void Requirment_Signature_Mission_Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (Requirment_Signature_Mission_Listbox.SelectedItem.ToString().Contains("需核定"))
            {
                Sign_Mission_Done.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Controller_Sign sign_controller = Controller_Sign.getInstance();
            DemoPluginFunction abstract_PlgunFunction = new DemoPluginFunction();
            abstract_PlgunFunction.SendNextSignMissionMail(sign_controller.get_mission_information("M0117"),2);
        }
    }
}
