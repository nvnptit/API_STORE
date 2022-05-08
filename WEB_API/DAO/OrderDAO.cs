using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WEB_API.Helpers;

namespace WEB_API.DAO
{
    public class OrderDAO
    {
        public static DataTable getListOrderByUser(int page, int page_size,int user_id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size));
            param.Add(new SqlParameter("@user_id", user_id));
            var dt = new SqlHelper().ExecuteQuery("getListOrderByUser", param);
            return dt;
        }
        public static DataTable getListOrderChuaXuLy(int page, int page_size)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page", page));
            param.Add(new SqlParameter("@page_size", page_size)) ;
            var dt = new SqlHelper().ExecuteQuery("getListOrderChuaXuLy", param);
            return dt;
        }
        public static DataTable getListOrderDaXuLy(int page, int page_size )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@page",page));
            param.Add(new SqlParameter("@page_size", page_size));
            var dt = new SqlHelper().ExecuteQuery("getListOrderDaXuLy", param);
            return dt;
        }
        public static DataTable getDetailOrder(int id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@order_id", id));
            var dt = new SqlHelper().ExecuteQuery("getDetailOrder", param);
            return dt;
        }

        public static DataTable getTotalOrderChuaXuLy()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var dt = new SqlHelper().ExecuteQuery("getTotalOrderChuaXuLy", param);
            return dt;
        }
        public static DataTable getTotalOrderDaXuLy()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var dt = new SqlHelper().ExecuteQuery("getTotalOrderDaXuLy", param);
            return dt;
        }
        public static void updateSoLuongTon(int prod_id, int soLuongGiam)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@prod_id", prod_id));
            param.Add(new SqlParameter("@soLuongGiam",soLuongGiam));
            new SqlHelper().ExecuteQuery("updateQuantityProduct", param);
        }
    }
}