using Newtonsoft.Json;
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
    public abstract class Mission : I_Mission
    {
        public Data_Set_Mission_Status data_set_mission_status { get; set; }
        public Data_Set_Mission data_set_mission { get; set; }
        public Data_Set_Mission_Type data_set_mission_type { get; set; }
        public List<Data_Set_Mission_Type> mission_type_data_all { get; set; }
        public List<Data_Set_Mission_Employee_Level> data_set_mission_employee_level { get; set; }

        public abstract string get_mission_require_signature(int risk_value, SubSysNo MissionTypes);

        public void initail_mission_object()
        {
            data_set_mission = new Data_Set_Mission();
            /// 初始化
            data_set_mission = initiail_collect_signed(data_set_mission);
            data_set_mission = initiail_extends(data_set_mission);
            data_set_mission = initiail_status_id(data_set_mission);
            data_set_mission = initiail_mid(data_set_mission);
        }
        public Data_Set_Mission set_mission_object_data(Set_Mission_Data_Option set_option, string value)
        {
            switch (set_option)
            {
                case Set_Mission_Data_Option.NAME:
                    data_set_mission.name = value;
                    break;
                case Set_Mission_Data_Option.COMPANY:
                    data_set_mission.company = value;
                    break;
                case Set_Mission_Data_Option.BINDING_PROJECT_ID:
                    data_set_mission.binding_project_id = value;
                    break;
                case Set_Mission_Data_Option.require_SIGNATURE:
                    data_set_mission.require_sign_json = value;
                    break;
                case Set_Mission_Data_Option.TYPE:
                    data_set_mission.mission_type = value;
                    break;
                case Set_Mission_Data_Option.RISK_VALUE:
                    //data_set_mission.risk_value = value; /// 暫且先HardCode之後用 請居政改掉他的Rigester Event就可以弄回來了
                    data_set_mission.risk_value = 1.ToString();
                    break;
                default:
                    break;
            }

            return data_set_mission;
        }
        public Data_Set_Mission initiail_collect_signed(Data_Set_Mission data_set_mission)
        {
            List<Data_Set_Mission_Collect_Signed> collect_signed_initial = new List<Data_Set_Mission_Collect_Signed>();
            collect_signed_initial.Add(new Data_Set_Mission_Collect_Signed());
            collect_signed_initial[0].extend_data = new Data_Set_Extends_LoanIt();
            data_set_mission.collect_signed_json = JsonConvert.SerializeObject(collect_signed_initial).Replace("null", "\"\"");
            return data_set_mission;
        }
        public Data_Set_Mission initiail_extends(Data_Set_Mission data_set_mission)
        {
            data_set_mission.extends = string.Format("{0}{1}", "{", "}");
            return data_set_mission;
        }

        public Data_Set_Mission initiail_status_id(Data_Set_Mission data_set_mission)
        {
            data_set_mission.status_id = 0.ToString();
            return data_set_mission;
        }
        public Data_Set_Mission initiail_mid(Data_Set_Mission data_set_mission)
        {
            Controller_Sign sign = Controller_Sign.getInstance();
            data_set_mission.m_id = sign.get_total_mission_count();

            return data_set_mission;
        }

        public Data_Set_Mission_Type initial_mission_type()
        {
            data_set_mission_type = new Data_Set_Mission_Type();
            return data_set_mission_type;
        }




        public List<Data_Set_Mission_Details> get_mission_information_all()
        {
            throw new NotImplementedException();
        }

        public string register_mission_status_summary(Data_Set_Mission_Status data_set_mission_status)
        {
            throw new NotImplementedException();
        }

        public string register_new_mission(Mission mission)
        {
            throw new NotImplementedException();
        }

        public string update_mission_data(string m_id)
        {
            throw new NotImplementedException();
        }


        public string get_total_mission_count()
        {
            throw new NotImplementedException();
        }

        public Data_Set_Mission_Details get_mission_information(string m_id)
        {
            throw new NotImplementedException();
        }

        public List<Data_Set_Mission_Type> get_mission_type_information(Data_Set_Employee data_set_employee)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Excutre_Result register_new_mission_type(Data_Set_Mission_Type data_set_mission_type)
        {
            throw new NotImplementedException();
        }

        public Data_Set_Mission_Type set_mission_type_data(Set_Mission_Type_Option Set_Mission_Type_Option, string value)
        {
            switch (Set_Mission_Type_Option)
            {
                case Set_Mission_Type_Option.COMPANY:
                    data_set_mission_type.company = value;
                    break;
                case Set_Mission_Type_Option.DEPARTMENT:
                    data_set_mission_type.department = value;
                    break;
                case Set_Mission_Type_Option.MISSION_TYPE_ID:
                    data_set_mission_type.mission_type_id = value;
                    break;
                case Set_Mission_Type_Option.SUMMARY:
                    data_set_mission_type.summary = value;
                    break;
                default:
                    break;
            }
            return data_set_mission_type;
        }

        public string filter_Mission_Done_RequireAmount(Data_Set_Mission_Details mission_data)
        {
            throw new NotImplementedException();
        }

        public List<Data_Set_Mission_Details> filter_Mission_Done_RequireAmount(List<Data_Set_Mission_Details> list_mission_data)
        {
            throw new NotImplementedException();
        }
    }
}
