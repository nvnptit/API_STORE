using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_API.Models
{
    public class Category
    {
        [AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public string image { get; set; }

    }
    public class CategoryAddModel
    {
        public string name { get; set; }
        public string image { get; set; }
    }
    public class CategoryEditModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }

    }
}