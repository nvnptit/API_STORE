using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WEB_API.Models
{
    public class User
    {
        [AutoIncrement]
        public int id { get; set; }
        public string email { get;set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
        public DateTime birthday { get; set; }
        public int role_id { get; set; }
        public bool status { get; set; }
        public int isVerify { get; set; }
    }
    public class UserAddModel
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
        public DateTime birthday { get; set; }
    }
    public class UserEditModel
    {
        public int id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
        public DateTime birthday { get; set; }
    }
    public class UserViewModel
    {
        public int id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
        public string birthday { get; set; }
        public int role_id { get; set; }
        public bool status { get; set; }
        public int isVerify { get; set; }
    }
}