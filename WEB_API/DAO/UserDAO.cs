using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WEB_API.Helpers;

namespace WEB_API.DAO
{
    public class UserDAO
    {
        public static DataTable getListUser(int page,int page_size)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page_size", page_size));
            param.Add(new SqlParameter("@page", page));
            var dt = new SqlHelper().ExecuteQuery("getListUser", param);
            return dt;
        }

        public static DataTable getUserById(int id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id", id));   
            var dt = new SqlHelper().ExecuteQuery("getUserById", param);
            return dt;
        }
        public static DataTable getTotalUser()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var dt = new SqlHelper().ExecuteQuery("getTotalUser", param);
            return dt;
        }
    }
}