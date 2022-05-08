using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEB_API.DAO;
using WEB_API.Helpers;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult getListUser(int page = 1, int page_size = 5)
        {
            var data = UserDAO.getListUser(page, page_size);
            List<UserViewModel> list = new List<UserViewModel>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                UserViewModel u = new UserViewModel();
                u.id = Int32.Parse(data.Rows[i]["id"].ToString());
                u.email = data.Rows[i]["email"].ToString();
                DateTime birthday = DateTime.Parse(data.Rows[i]["birthday"].ToString());
                int day = birthday.Day;
                int month = birthday.Month;
                int year = birthday.Year;
                u.birthday = day + "/" + month + "/" + year;
                u.address = data.Rows[i]["address"].ToString();
                u.firstname = data.Rows[i]["firstname"].ToString();
                u.lastname = data.Rows[i]["lastname"].ToString();
                u.phone = data.Rows[0]["phone"].ToString();
                u.sex = Int32.Parse(data.Rows[i]["sex"].ToString());
                u.status = bool.Parse(data.Rows[i]["status"].ToString());
                u.role_id = Int32.Parse(data.Rows[i]["role_id"].ToString());
                u.isVerify = Int32.Parse(data.Rows[i]["isVerify"].ToString());
                list.Add(u);
            }
            return Ok(new { result = true, data = list });
        }
        [Route("id")]
        [HttpGet]
        public IHttpActionResult getUserById(int id)
        {
            var data = UserDAO.getUserById(id);
            if (data.Rows.Count == 0)
            {
                return Ok(new { result = false, data = "Không tìm thấy người dùng" });
            }
            UserViewModel u = new UserViewModel();
            u.id = Int32.Parse(data.Rows[0]["id"].ToString());
            u.email = data.Rows[0]["email"].ToString();

            DateTime birthday = DateTime.Parse(data.Rows[0]["birthday"].ToString());
            int day = birthday.Day;
            int month = birthday.Month;
            int year = birthday.Year;
            u.birthday = day + "/" + month + "/" + year;
            u.address = data.Rows[0]["address"].ToString();
            u.firstname = data.Rows[0]["firstname"].ToString();
            u.lastname = data.Rows[0]["lastname"].ToString();
            u.phone = data.Rows[0]["phone"].ToString();
            u.sex = Int32.Parse(data.Rows[0]["sex"].ToString());
            u.status = bool.Parse(data.Rows[0]["status"].ToString());
            u.role_id = Int32.Parse(data.Rows[0]["role_id"].ToString());
            u.isVerify = Int32.Parse(data.Rows[0]["isVerify"].ToString());
            return Ok(new { result = true, data = u });
        }
        [Route("")]
        [HttpPost]
        public IHttpActionResult addUser(UserAddModel model)
        {
            User data = new User();
            UserViewModel u = new UserViewModel();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(model.email) || String.IsNullOrEmpty(model.password))
                    {
                        return Ok(new { result = false, data = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where email='" + model.email + "'");
                    if (exist.Rows.Count > 0)
                    {
                        return Ok(new { result = false, data = "Email này đã có tài khoản, mời bạn nhập email khác" });
                    }
                    data.address = model.address;
                    data.email = model.email;
                    data.birthday = model.birthday.Date;
                    data.firstname = model.firstname;
                    data.lastname = model.lastname;
                    data.password = PasswordHelper.Encrypt(model.password);
                    data.sex = model.sex;
                    data.role_id = 0;//nguoi mua
                    data.status = true;
                    data.isVerify = 1;
                    data.phone = model.phone;


                    u.email = model.email;
                    DateTime birthday = model.birthday.Date;
                    int day = birthday.Day;
                    int month = birthday.Month;
                    int year = birthday.Year;
                    u.birthday = day + "/" + month + "/" + year;
                    u.address = model.address;
                    u.firstname = model.firstname;
                    u.lastname = model.lastname;
                    u.phone = model.phone;
                    u.sex = model.sex;
                    u.status = true;
                    u.role_id = 0;
                    u.isVerify = 1;
                    dbConn.Insert(data);
                    u.id = SqlHelper.getLastInsertId("User");
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = u });
        }
        [Route("")]
        [HttpPut]
        public IHttpActionResult editUser(UserEditModel model)
        {
            User data = new User();
            UserViewModel u = new UserViewModel();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(model.email))
                    {
                        return Ok(new { result = false, data = "Vui lòng nhập đầy đủ thông tin!" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where id='" + model.id + "'");

                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không có tài khoản này" });
                    }
                    //những thông tin không được sửa
                    data.password = exist.Rows[0]["password"].ToString();
                    data.role_id = 0;
                    var existEmail = new SqlHelper().ExecuteString("Select * from [User] where email='" + model.email + "'and id!=" + model.id + "and status!=0");
                    if (existEmail.Rows.Count > 0)
                    {
                        return Ok(new { result = false, data = "Đã tồn tại email" });
                    }
                    data.email = model.email;
                    //những thông tin lấy xuống từ model                    
                    data.address = model.address;
                    data.birthday = model.birthday;
                    data.firstname = model.firstname;
                    data.lastname = model.lastname;
                    data.status = true;
                    data.sex = model.sex;
                    data.phone = model.phone;
                    dbConn.Update<User>(data, s => s.id == model.id);

                    var dataView = UserDAO.getUserById(model.id);
                    if (dataView.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không tìm thấy người dùng" });
                    }
                  
                    u.id = Int32.Parse(dataView.Rows[0]["id"].ToString());
                    u.email = dataView.Rows[0]["email"].ToString();
                    DateTime birthday = DateTime.Parse(dataView.Rows[0]["birthday"].ToString());
                    int day = birthday.Day;
                    int month = birthday.Month;
                    int year = birthday.Year;
                    u.birthday = day + "/" + month + "/" + year;
                    u.address = dataView.Rows[0]["address"].ToString();
                    u.firstname = dataView.Rows[0]["firstname"].ToString();
                    u.lastname = dataView.Rows[0]["lastname"].ToString();
                    u.phone = dataView.Rows[0]["phone"].ToString();
                    u.sex = Int32.Parse(dataView.Rows[0]["sex"].ToString());
                    u.status = bool.Parse(dataView.Rows[0]["status"].ToString());
                    u.role_id = Int32.Parse(dataView.Rows[0]["role_id"].ToString());
                    u.isVerify = Int32.Parse(dataView.Rows[0]["isVerify"].ToString());
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = u });
        }
        [Route("")]
        [HttpDelete]
        public IHttpActionResult deleteUser(int id)
        {
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (id == 0)
                    {
                        return Ok(new { result = false, message = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where id='" + id + "'and status <>0");

                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không có tài khoản này" });
                    }
                    dbConn.ExecuteNonQuery("update [User] set status=0 where id=" + id + "");

                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = id });
        }
        [Route("login")]
        [HttpPost]
        public IHttpActionResult login(string email, string password)
        {
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                    {
                        return Ok(new { result = false, message = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where email='" + email + "'and status <>0");

                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không có tài khoản này" });
                    }
                    var passwordDB = exist.Rows[0]["password"].ToString();
                    var desPass = PasswordHelper.Decrypt(passwordDB);
                    if (password == desPass)
                    {
                        UserViewModel u = new UserViewModel();
                        u.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                        u.email = exist.Rows[0]["email"].ToString();
                        DateTime birthday = DateTime.Parse(exist.Rows[0]["birthday"].ToString());
                        int day = birthday.Day;
                        int month = birthday.Month;
                        int year = birthday.Year;
                        u.birthday = day + "/" + month + "/" + year;

                        u.address = exist.Rows[0]["address"].ToString();
                        u.firstname = exist.Rows[0]["firstname"].ToString();
                        u.lastname = exist.Rows[0]["lastname"].ToString();
                        u.phone = exist.Rows[0]["phone"].ToString();
                        u.sex = Int32.Parse(exist.Rows[0]["sex"].ToString());
                        u.status = bool.Parse(exist.Rows[0]["status"].ToString());
                        u.role_id = Int32.Parse(exist.Rows[0]["role_id"].ToString());
                        u.isVerify = Int32.Parse(exist.Rows[0]["isVerify"].ToString());

                        return Ok(new { result = true, data = u });
                    }
                    else
                    {
                        return Ok(new { result = false, data = "Email hoặc mật khẩu không đúng!" });
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
        }
        [Route("change-password")]
        [HttpPost]
        public IHttpActionResult changePass(string email, string oldPassword, string newPassword)
        {
            User data = new User();
            UserViewModel view = new UserViewModel();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(newPassword))
                    {
                        return Ok(new { result = false, message = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where email='" + email + "'and status <>0");
                    //    var data = dbConn.Exists<User>(new { email = email });
                    var passwordDB = exist.Rows[0]["password"].ToString();
                    var desPass = PasswordHelper.Decrypt(passwordDB);
                    if (oldPassword == desPass)
                    {   
                        view.id = data.sex = Int32.Parse(exist.Rows[0]["id"].ToString());
                        string enPass = PasswordHelper.Encrypt(newPassword);
                        data.email = email;
                        view.email = email;
                        data.password = enPass;
                        //những thông tin lấy xuống từ model                    
                        data.address = exist.Rows[0]["address"].ToString();
                        view.address = data.address;
                        data.birthday = DateTime.Parse(exist.Rows[0]["birthday"].ToString()).Date;
                        DateTime birthday = data.birthday;
                        int day = birthday.Day;
                        int month = birthday.Month;
                        int year = birthday.Year;
                        view.birthday = day + "/" + month + "/" + year;
                        data.firstname = exist.Rows[0]["firstname"].ToString();
                        view.firstname = data.firstname;
                        data.lastname = exist.Rows[0]["lastname"].ToString();
                        view.lastname = data.lastname;
                        data.status = true;
                        view.status = data.status;
                        data.sex = Int32.Parse(exist.Rows[0]["sex"].ToString());
                        view.sex = data.sex;
                        data.phone = exist.Rows[0]["phone"].ToString();
                        view.phone = data.phone;
                        dbConn.Update<User>(data, s => s.id == Int32.Parse(exist.Rows[0]["id"].ToString()));
                        return Ok(new { result = true, data = view });
                    }
                    else
                    {
                        return Ok(new { result = false, data = "Sai mật khẩu!" });
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
        }
        [Route("forgot-password")]
        [HttpPost]
        public IHttpActionResult forgotPassword(string email)
        {
            User data = new User();
            UserViewModel view = new UserViewModel();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(email))
                    {
                        return Ok(new { result = false, message = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where email='" + email + "'and status <>0");
                    //    var data = dbConn.Exists<User>(new { email = email });
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, message = "Không có tài khoản này" });
                    }
                    var passwordDB = exist.Rows[0]["password"].ToString();
                    var desPass = PasswordHelper.Decrypt(passwordDB);
                    
                        view.id = data.sex = Int32.Parse(exist.Rows[0]["id"].ToString());
                        data.email = email;
                        view.email = email;
                        string newPass = Helpers.PasswordHelper.CreatePassword(8);
                        string enPass = Helpers.PasswordHelper.Encrypt(newPass);
                        data.password = enPass;
                        //những thông tin lấy xuống từ model                    
                        data.address = exist.Rows[0]["address"].ToString();
                        view.address = data.address;
                        data.birthday = DateTime.Parse(exist.Rows[0]["birthday"].ToString()).Date;
                        DateTime birthday = data.birthday;
                        int day = birthday.Day;
                        int month = birthday.Month;
                        int year = birthday.Year;
                        view.birthday = day + "/" + month + "/" + year;
                        data.firstname = exist.Rows[0]["firstname"].ToString();
                        view.firstname = data.firstname;
                        data.lastname = exist.Rows[0]["lastname"].ToString();
                        view.lastname = data.lastname;
                        data.status = true;
                        view.status = data.status;
                        data.sex = Int32.Parse(exist.Rows[0]["sex"].ToString());
                        view.sex = data.sex;
                        data.phone = exist.Rows[0]["phone"].ToString();
                        view.phone = data.phone;
                    string smtpUserName = "minhtan0802@gmail.com";
                    string smtpPassword = "wvtpyuhdugdcxjat";
                    string smtpHost = "smtp.gmail.com";
                    int smtpPort = 587;

                    string emailTo = email;
                    string subject = "Lấy lại mật khẩu";
                    string body = string.Format("Mật khẩu mới của bạn là: <b></b><br/><br/>{0} </br>",newPass);
                    EmailService service = new EmailService();
                    bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
                    dbConn.Update<User>(data, s => s.id == Int32.Parse(exist.Rows[0]["id"].ToString()));
                        return Ok(new { result = true, data = "" });
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
        }

        [Route("change-password-by-phone")]
        [HttpPost]
        public IHttpActionResult changePassByPhone(string phone, string newPassword)
        {
            User data = new User();
            UserViewModel view = new UserViewModel();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(newPassword))
                    {
                        return Ok(new { result = false, message = "Vui lòng nhập đầy đủ thông tin" });
                    }
                    var exist = new SqlHelper().ExecuteString("Select * from [User] where phone='" + phone + "'and status <>0");
                    //    var data = dbConn.Exists<User>(new { email = email });
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, message = "Số điện thoại không khớp với tài khoản nào" });
                    }
                        view.id = data.sex = Int32.Parse(exist.Rows[0]["id"].ToString());
                        string enPass = PasswordHelper.Encrypt(newPassword);
                        data.email = exist.Rows[0]["email"].ToString();
                        view.email = exist.Rows[0]["email"].ToString(); ;
                        data.password = enPass;
                        //những thông tin lấy xuống từ model                    
                        data.address = exist.Rows[0]["address"].ToString();
                        view.address = data.address;
                        data.birthday = DateTime.Parse(exist.Rows[0]["birthday"].ToString()).Date;
                        DateTime birthday = data.birthday;
                        int day = birthday.Day;
                        int month = birthday.Month;
                        int year = birthday.Year;
                        view.birthday = day + "/" + month + "/" + year;
                        data.firstname = exist.Rows[0]["firstname"].ToString();
                        view.firstname = data.firstname;
                        data.lastname = exist.Rows[0]["lastname"].ToString();
                        view.lastname = data.lastname;
                        data.status = true;
                        view.status = data.status;
                        data.sex = Int32.Parse(exist.Rows[0]["sex"].ToString());
                        view.sex = data.sex;
                        data.phone = phone;
                        view.phone = data.phone;
                        dbConn.Update<User>(data, s => s.id == Int32.Parse(exist.Rows[0]["id"].ToString()));
                        return Ok(new { result = true, data = view });
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
        }

        [Route("total")]
        [HttpGet]
        public IHttpActionResult getTotalUser()
        {
            var data = UserDAO.getTotalUser();
            var result = data.Rows[0][0];
            return Ok(new { result = true, data = result });
        }
    }
}
