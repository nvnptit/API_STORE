using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WEB_API.Helpers;
using WEB_API.Models;

namespace WEB_API.DAO
{
    public class CategoryDAO
    {
        public static DataTable GetListCategory(int page, int page_size)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size));
            var dt = new SqlHelper().ExecuteQuery("getListCategory", param);
            return dt;
        }
        public static DataTable getTotalCategory()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var dt = new SqlHelper().ExecuteQuery("getTotalCategory", param);
            return dt;
        }

        public static DataTable getCategoryByID(int id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id", id));
            var dt = new SqlHelper().ExecuteQuery("getCategoryByID", param);
            return dt;
        }
    }
}