using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_API.Models
{
    public class CartItem
    {
        [AutoIncrement]
        public int id { get; set; }
        public int quantity { get; set; }
        public int prod_id { get; set; }
        public int user_id { get; set; }
    }
    public class CartItemAddModel
    {
        public int quantity { get; set; }
        public int prod_id { get; set; }
        public int user_id { get; set; }
    }
    public class CartItemEditModel
    {
        public int quantity { get; set; }
        public int prod_id { get; set; }
    }
}