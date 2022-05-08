using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WEB_API.Helpers;

namespace WEB_API.DAO
{
    public class CartItemDAO
    {
        public static DataTable getListCartItem(int user_id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@user_id", user_id));
            var dt = new SqlHelper().ExecuteQuery("getListCartItem", param);
            return dt;
        }
    }
}