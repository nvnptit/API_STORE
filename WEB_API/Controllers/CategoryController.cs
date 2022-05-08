using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WEB_API.DAO;
using WEB_API.Helpers;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [RoutePrefix("api/category")]
    public class CategoryController :ApiController
    {
        [Route("")]
        public IHttpActionResult getListCategory(int page=1, int page_size=5)
        {
            var data= CategoryDAO.GetListCategory(page, page_size);
            return Ok(new { result = true, data = data });
                
        }
        [Route("")]
        [HttpPost]
        public IHttpActionResult addCategory(CategoryAddModel model)
        {
            Category data = new Category();
            try
            {
                using (var dbConn= Helpers.OrmliteConnection.openConn())
                {
                    if(String.IsNullOrEmpty(model.name)|| String.IsNullOrEmpty(model.image))
                    {
                        return Ok(new { result = false, data = "Vui lòng nhập đầy đủ thông tin!" });
                    }
                    var exist = new SqlHelper().ExecuteString("SELECT * FROM Category WHERE name like N'%"+model.name+"%'");
                    if(exist.Rows.Count>0)
                    {
                        return Ok(new { result = false, data = "Tên sản phẩm đã có" });
                      
                    }    
                    data.name = model.name;
                    data.status = true;
                    data.image = model.image;
                    dbConn.Insert(data);
                    data.id = SqlHelper.getLastInsertId("Category");
                }    
            }
            catch(Exception e)
            {
                return Ok(new { result = false, data = e.Message }); 
                  
            }
            return Ok(new { result = true, data = data});
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult editCategory(CategoryEditModel model)
        {
            var data = new Category();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from Category where id="+model.id+"and status<>0");
                    if(exist.Rows.Count==0)
                    {
                        return Json("Không tìm thấy loại hàng cần sửa");
                    }
                    var existName = new SqlHelper().ExecuteString("select * from Category where name like N'%" + model.name + "%'and status<>0 and id!=" + model.id);
                    if (existName.Rows.Count != 0)
                    {
                        return Ok(new { result = false, data = "Đã tồn tại tên loại sản phẩm, mời nhập tên sản phẩm khác" });
                    }
                    data.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                    data.name = model.name;
                    data.image = model.image;
                    data.status = true;
                    dbConn.Update<Category>(data,s=>s.id==model.id);
                      
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
                return Ok(new { result = true, data = model});
        }
        [HttpDelete]
        [Route("")]
        public IHttpActionResult deleteCatgory(int id)
        {
            try
            {
                using (var dbConn= Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from Category where id=" +id+"and status<>0");
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data= "Không tìm thấy loại hàng cần xóa" });
                    }
                    else
                    {
                        dbConn.ExecuteNonQuery("update Category set status=0 where id=" + id);
                        return Ok(new { result =true, data =id });
                    }    
                }    
            }
            catch(Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
        }
        [Route("total")]
        [HttpGet]
        public IHttpActionResult getTotalCategory()
        {
            var data = CategoryDAO.getTotalCategory();
            var result = data.Rows[0][0];
            return Ok(new { result = true, data = result });
        }

        [Route("id")]
        [HttpGet]
        public IHttpActionResult getCategoryByID(int id)
        {
            var data = CategoryDAO.getCategoryByID(id);
            if (data.Rows.Count == 0)
            {
                return Ok(new { result = false, message = "Không tìm thấy loại sản phẩm" });
            }
            Category p = new Category();
            p.id = Int32.Parse(data.Rows[0]["id"].ToString());
            p.image = data.Rows[0]["image"].ToString();
            p.name = data.Rows[0]["name"].ToString();
            p.status = bool.Parse(data.Rows[0]["status"].ToString());
            return Ok(new { result = true, data = p });
        }
    }

}
