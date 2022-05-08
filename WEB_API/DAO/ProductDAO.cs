using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WEB_API.Helpers;

namespace WEB_API.DAO
{
    public class ProductDAO
    {
        public static DataTable getListProduct(int page=1, int page_size=5)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size));
            var dt = new SqlHelper().ExecuteQuery("getListProduct", param);
            return dt;
        }

        public static DataTable getProductByID(int id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id",id));
            var dt = new SqlHelper().ExecuteQuery("getProductByID", param);
            return dt;
        }
        public static DataTable getListProductByCategory(int type,int page=1, int page_size=5)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size));
            param.Add(new SqlParameter("@type", type));
            var dt = new SqlHelper().ExecuteQuery("getListProductByCategory", param);
            return dt;
        }
        public static DataTable getListProductByName(string name,int page=1, int page_size=5)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size));
            param.Add(new SqlParameter("@name", name));
            var dt = new SqlHelper().ExecuteQuery("getListProductByName", param);
            return dt;
        }
        public static DataTable getPriceProductByID(int id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("id", id));
            var dt = new SqlHelper().ExecuteQuery("getPriceProductByID", param);
            return dt;
        }
        public static DataTable getTotalProduct()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var dt = new SqlHelper().ExecuteQuery("getTotalProduct", param);
            return dt;
        }

        public static DataTable getNewestProduct(int quantity)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@QUANTITY", quantity));
            var dt = new SqlHelper().ExecuteQuery("getNewestProduct", param);
            return dt;
        }
    }
}