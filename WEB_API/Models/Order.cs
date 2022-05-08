using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_API.Models
{
    public class Order
    {
        [AutoIncrement]
        public int id { get; set; }
        public int user_id { get; set; }
        public string deliveryAddress { get; set; }
        public DateTime? buyDate { get; set; }
        public DateTime? deliveryCancelDay { get; set; }
        public string recipientName{ get; set; }
        public int status { get; set; }
        public string phone { get; set; }
    }
    public class OrderItem
    {
        [AutoIncrement]
        public int id { get; set; }
        public int order_id { get; set; }
        public int quantity { get; set; }
        public int prod_id { get; set; }
        public int price { get; set; }
    }
    public class OrderItemViewModel
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int quantity { get; set; }
        public int prod_id { get; set; }
        public int price { get; set; }
        public string image { get; set; }
        public string name { get; set; }
    }
    public class OrderItemAddModel
    {
        public int quantity { get; set; }
        public int prod_id { get; set; }
        public int price { get; set; }
        public int order_id { get; set; }
    }
    public class OrderAddModel
    {
        public int user_id { get; set; }
        public string deliveryAddress { get; set; }
        public string phone { get; set; }
        public bool status { get; set; }
        public string recipientName { get; set; }
    }
    public class OrderViewModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string deliveryAddress { get; set; }
        public string buyDate { get; set; }
        public string deliveryCancelDay { get; set; }
        public string phone { get; set; }
        public int status { get; set; }
        public string recipientName { get; set; }
        public List<OrderItemViewModel> listOrderItem { get; set; }
    }

}