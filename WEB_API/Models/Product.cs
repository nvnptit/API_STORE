using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_API.Models
{
    public class Product
    {
        [AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
        public int cate_id { get; set; }
        public string image { get; set; }
        public string unit { get; set; }
        public int quantity { get; set; }
        public string detail { get; set; }
        public bool status { get; set; }   
    }
    public class ProductAddModel
    {
        public string name { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
        public int cate_id { get; set; }
        public string image { get; set; }
        public string unit { get; set; }
        public int quantity { get; set; }
        public string detail { get; set; }
    }
    public class ProductEditModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
        public int cate_id { get; set; }
        public string image { get; set; }
        public string unit { get; set; }
        public int quantity { get; set; }
        public string detail { get; set; }
    }


}