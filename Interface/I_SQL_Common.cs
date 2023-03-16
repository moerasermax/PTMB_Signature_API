using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using PTMB_Signature_API.Data_Set;
using System.Data;

namespace PTMB_Signature_API.Interface
{
    public interface I_SQL_Common
    {
        SqlCommand get_Sql_Command(SqlConnection conn, string cmd_str);
        SqlConnection get_Sql_Load_Connection(string source);
        SqlCommand set_Sql_Parametert(Sql_Action_Option action_option,SqlCommand cmd, string value);
        DataTable excute_Sql_cmd(Sql_Action_Category_Option category_option, SqlConnection conn, SqlCommand cmd);

    }
}
