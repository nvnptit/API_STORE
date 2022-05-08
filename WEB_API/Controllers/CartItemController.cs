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
    [RoutePrefix("api/cart-item")]
    public class CartItemController : ApiController
    {
        [Route("")]
        public IHttpActionResult getListCartItem(int user_id)
        {
            var data = CartItemDAO.getListCartItem(user_id);
            if (data.Rows.Count == 0)
            {
                return Ok(new { result = true, data =data});
            }
            return Ok(new { result = true, data = data });
        }
        [HttpPost]
        [Route("")]
        public IHttpActionResult addCartItem(CartItemAddModel model)
        {
            var data = new CartItem();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    if (model.prod_id==0 || model.quantity==0)
                    {
                        return Json("Vui lòng nhập đầy đủ thông tin");
                    }
                    data.prod_id = model.prod_id;
                    data.quantity = model.quantity;
                    data.user_id = model.user_id;
                    dbConn.Insert(data);
                    var id = SqlHelper.getLastInsertId("CartItem");
                    data.id = (int)id;
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
        public IHttpActionResult editCartItem(int cartItem_id, int quantity)
        {
            CartItem cartItem = new CartItem();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("SELECT * FROM CartItem WHERE id=" + cartItem_id);
                    cartItem.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                    cartItem.prod_id= Int32.Parse(exist.Rows[0]["prod_id"].ToString());
                    cartItem.quantity = quantity;
                    cartItem.user_id= Int32.Parse(exist.Rows[0]["user_id"].ToString());
                    dbConn.Update<CartItem>(cartItem,s=> s.id == cartItem_id);
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false,data=e.Message }); 
                 
            }
            return Ok(new {result=true, data=cartItem });
        }
        [HttpDelete]
        [Route("")]
        public IHttpActionResult deleteCartItem(int id)
        {
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from CartItem where id=" + id);
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không tìm thấy sản phẩm cần xóa" });
                    }
                    else
                    {
                        dbConn.ExecuteNonQuery("DELETE FROM CartItem WHERE id=" + id);
                        return Ok(new { result = true, data = id });
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message }); 
                    
            }
        }
        [HttpDelete]
        [Route("all-cart-item")]
        public IHttpActionResult deleteAllCartItem(int user_id)
        {
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from CartItem where user_id=" + user_id);
                    if (exist.Rows.Count == 0)
                    {
                        return Ok(new { result = false, data = "Không có danh sách cart item" });
                    }
                    else
                    {
                        dbConn.ExecuteNonQuery("DELETE FROM CartItem WHERE user_id=" + user_id);
                        return Ok(new { result = true, data = user_id });
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
