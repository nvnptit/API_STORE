using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEB_API.DAO;
using WEB_API.Helpers;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [ServiceStack.EnableCors(allowedOrigins: "https://vnn.digital/api/product", allowedHeaders: "*", allowedMethods: "*")] 
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult getListProduct(int page=1, int page_size=5)
        {
            var data = ProductDAO.getListProduct(page,page_size);
            return Ok(new { result = true, data = data }); 
        }

        [Route("newest")]
        [HttpGet]
        public IHttpActionResult getNewestProduct(int quantity=10)
        {
            var data = ProductDAO.getNewestProduct(quantity);
            return Ok(new { result = true, data = data });
        }

        [Route("category")]
        [HttpGet]
        public IHttpActionResult getListProductByCategory(int type,int page=1, int page_size=5)
        {
            var data = ProductDAO.getListProductByCategory(type, page, page_size);
            return Ok(new { result = true, data = data });
        }
        [Route("name")]
        [HttpGet]
        public IHttpActionResult getListProductByName(string name="", int page=1, int page_size=5)
        {
            var data = ProductDAO.getListProductByName(name, page, page_size);
            return Ok(new { result = true, data = data });
        }

        [Route("id")]
        [HttpGet]
        public IHttpActionResult getProductByID(int id)
        {
            var data = ProductDAO.getProductByID(id);
            if(data.Rows.Count==0)
            {
                return Ok(new { result = false, message="Không tìm thấy sản phẩm"});
            }
            Product p = new Product();
            p.cate_id = Int32.Parse(data.Rows[0]["cate_id"].ToString());
            p.price = Int32.Parse(data.Rows[0]["price"].ToString());
            p.quantity= Int32.Parse(data.Rows[0]["quantity"].ToString());
            p.detail = data.Rows[0]["detail"].ToString();
            p.id = Int32.Parse(data.Rows[0]["id"].ToString());
            p.image= data.Rows[0]["image"].ToString();
            p.name= data.Rows[0]["name"].ToString();
            p.unit=data.Rows[0]["unit"].ToString();
            p.status = bool.Parse(data.Rows[0]["status"].ToString());
            p.discount = float.Parse(data.Rows[0]["discount"].ToString());
            return Ok(new { result = true, data = p});
        }

        [Route("total")]
        [HttpGet]
        public IHttpActionResult getTotalProduct()
        {
            var data = ProductDAO.getTotalProduct();
            var result = data.Rows[0][0];
            return Ok(new { result = true, data =result});
        }
        [Route("price")]
        [HttpGet]
        public IHttpActionResult getPriceProductByID(int id)
        {
            var data = ProductDAO.getPriceProductByID(id);
            return Ok(new { result = true, data = data });
        }
        [HttpPost]
        [Route("")]
        public IHttpActionResult addProduct(ProductAddModel model)
        {
            Product data = new Product();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (String.IsNullOrEmpty(model.name) || String.IsNullOrEmpty(model.image)|| String.IsNullOrEmpty(model.unit)||model.price==0)
                    {
                        return Ok(new {result=false, data= "Vui lòng nhập đầy đủ thông tin" });
                    }
                    data.name = model.name;
                    data.cate_id = model.cate_id;
                    data.discount = model.discount;
                    data.detail = model.detail;
                    data.quantity = model.quantity;
                    data.unit = model.unit;
                    data.status = true;
                    data.price = model.price;
                    data.image = model.image;
                    dbConn.Insert(data);
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = data });
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult editProduct(ProductEditModel model)
        {
            Product data = new Product();
              try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from Product where id=" + model.id+"and status<>0");
                    if (exist.Rows.Count == 0)
                    {
                        return Json("Không tìm thấy sản phẩm cần sửa");
                    }
                    data.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                    data.price = model.price;
                    var existName = new SqlHelper().ExecuteString("select * from Product where name like N'%" + model.name + "%'and status<>0 and id!="+model.id);
                    if(existName.Rows.Count!=0)
                    {
                        return Ok(new {result=false, data= "Đã tồn tại tên sản phẩm, mời nhập tên sản phẩm khác" });
                    }    
                    data.name = model.name;
                    data.detail = model.detail;
                    data.image = model.image;
                    data.quantity = model.quantity;
                    data.status = true;
                    data.unit = model.unit;
                    data.cate_id = model.cate_id;
                    dbConn.Update<Product>(data, s => s.id == model.id);
         
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = model });
        }
        [HttpDelete]
        [Route("")]
        public IHttpActionResult deleteProduct(int id)
        {
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from Product where id=" + id+"and status<>0");
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không tìm thấy sản phẩm cần xóa" });
                       
                    }
                    else
                    {
                        dbConn.ExecuteNonQuery("update Product set status=0 where id=" + id);
                        return Ok(new { result = true, data = id });
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
           
        }
    }
}
