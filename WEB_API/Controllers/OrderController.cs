using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WEB_API.DAO;
using WEB_API.Helpers;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        [Route("chua-xu-ly")]
        public IHttpActionResult getListOrderChuaXuLy(int page = 1, int page_size = 5)
        {
            var data = OrderDAO.getListOrderChuaXuLy(page, page_size);
            return Ok(new { result = true, data = data });
        }
        [Route("da-xu-ly")]
        public IHttpActionResult getListOrderDaXuLy(int page = 1, int page_size = 5)
        {
            var data = OrderDAO.getListOrderDaXuLy(page, page_size);
            return Ok(new { result = true, data = data }); 
        }
        [Route("")]
        [HttpGet]
        public IHttpActionResult getListOrderByUser(int user_id,int page = 1, int page_size = 5)
        {
            var data = OrderDAO.getListOrderByUser(page, page_size,user_id);
            List<OrderItemViewModel> listOrderItem = new List<OrderItemViewModel>();
            List<OrderViewModel> listOrder = new List<OrderViewModel>();
            if(data.Rows.Count==0)
            {
                return Ok(new { result =true, data = data });
            }   
            for(int i=0;i<data.Rows.Count;i++)
            {
                var resultItem = new OrderViewModel();
                var dataTableDetailOrder = OrderDAO.getDetailOrder(Int32.Parse(data.Rows[i]["id"].ToString()));
                
                resultItem.id = Int32.Parse(data.Rows[i]["id"].ToString());
                resultItem.user_id= Int32.Parse(data.Rows[i]["user_id"].ToString());
                resultItem.buyDate = DateTime.Parse(data.Rows[i]["buyDate"].ToString()).ToShortDateString();
                resultItem.phone = data.Rows[i]["phone"].ToString();
                DateTime date;
                if (DateTime.TryParse(data.Rows[i]["deliveryCancelDay"].ToString(), out date))
                {
                    int day = date.Day;
                    int month = date.Month;
                    int year = date.Year;
                    resultItem.deliveryCancelDay = day + "/" + month + "/" + year;
                }
                else
                {
                    resultItem.deliveryCancelDay = null;
                }
                if (DateTime.TryParse(data.Rows[i]["buyDate"].ToString(), out date))
                {
                    int day = date.Day;
                    int month = date.Month;
                    int year = date.Year;
                    resultItem.buyDate = day + "/" + month + "/" + year;
                }
                else
                {
                    resultItem.buyDate = null;
                }
                resultItem.deliveryAddress = data.Rows[i]["deliveryAddress"].ToString();
                resultItem.status = Int32.Parse(data.Rows[i]["status"].ToString());
                resultItem.recipientName = data.Rows[i]["recipientName"].ToString();
                for (int j=0;j<dataTableDetailOrder.Rows.Count;j++)
                {
                    var orderItem = new OrderItemViewModel();
                    orderItem.id = Int32.Parse(dataTableDetailOrder.Rows[j]["id"].ToString());
                    orderItem.price = Int32.Parse(dataTableDetailOrder.Rows[j]["price"].ToString());
                    orderItem.prod_id=Int32.Parse(dataTableDetailOrder.Rows[j]["prod_id"].ToString());
                    orderItem.quantity= Int32.Parse(dataTableDetailOrder.Rows[j]["quantity"].ToString());
                    orderItem.order_id = Int32.Parse(dataTableDetailOrder.Rows[j]["order_id"].ToString());
                    orderItem.image = dataTableDetailOrder.Rows[j]["image"].ToString();
                    orderItem.name= dataTableDetailOrder.Rows[j]["name"].ToString();
                    listOrderItem.Add(orderItem);
                }
                resultItem.listOrderItem = listOrderItem;
                listOrder.Add(resultItem);
              }
            return Ok(new { result = true, data = listOrder }); 
        }
        [Route("detail")]
        [HttpGet]
        public IHttpActionResult getDetailOrder(int id)
        {
            var data = OrderDAO.getDetailOrder(id);
            return Ok(new { result = true, data = data });
        }
        [Route("")]
        [HttpPost]
        public IHttpActionResult addOrder(OrderAddModel model)
        {
            var data = new Order();
            try
            {
                using (var dbConn=Helpers.OrmliteConnection.openConn())
                {
                    data.buyDate = DateTime.Now.Date;
                    data.status =0;
                    data.user_id =model.user_id;
                    data.phone = model.phone;
                    data.deliveryAddress = model.deliveryAddress;
                    data.recipientName = model.recipientName;
                    dbConn.Insert(data);
                    var id = SqlHelper.getLastInsertId("Order");
                    data.id = id;
                }    
            }
            catch(Exception e)
            {
                return Ok(new { result = false, data = e.Message }); 
                
            }
            return Ok(new { result = true, data = data});         
        }
        [Route("order-item")]
        [HttpPost]
        public IHttpActionResult addOrderItem(OrderItemAddModel model)
        {
            var data = new OrderItem();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    data.price = model.price;
                    data.prod_id = model.prod_id;
                    data.quantity = model.quantity;
                    data.order_id = model.order_id;
                    dbConn.Insert(data);
                    int id = SqlHelper.getLastInsertId("OrderItem");
                    SqlHelper sqlHelp = new SqlHelper();
                    data.id = id;
                    OrderDAO.updateSoLuongTon(model.prod_id, (-1)*model.quantity);
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });

            }
            return Ok(new { result = true, data = data });
        }
        [Route("total-chua-xu-ly")]
        [HttpGet]
        public IHttpActionResult getTotalOrderChuaXuLy()
        {
            var data = OrderDAO.getTotalOrderChuaXuLy();
            var result = data.Rows[0][0];
            return Ok(new { result = true, data = result });
        }
        [Route("total-da-xu-ly")]
        [HttpGet]
        public IHttpActionResult getTotalOrderDaXuLy()
        {
            var data = OrderDAO.getTotalOrderDaXuLy();
            var result = data.Rows[0][0];
            return Ok(new { result = true, data = result });
        }
        [HttpPut]
        [Route("giao-hang")]
        public IHttpActionResult editOrder(int id)
        {
            Order data = new Order();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from [Order] where id=" +id);
                    if (exist.Rows.Count == 0)
                    {
                        return Json("Không tìm thấy đơn hàng cần cập nhật");
                    }
                    data.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                    data.buyDate= DateTime.Parse(exist.Rows[0]["buyDate"].ToString());
                    data.recipientName = exist.Rows[0]["recipientName"].ToString();
                    data.phone= exist.Rows[0]["phone"].ToString();
                    data.status = 1;
                    data.deliveryCancelDay = DateTime.Now.Date;
                    data.user_id= Int32.Parse(exist.Rows[0]["user_id"].ToString());
                    data.deliveryAddress = exist.Rows[0]["deliveryAddress"].ToString();
              
                    dbConn.Update<Order>(data,s=>s.id==id);

                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = data });
        }
        [HttpPut]
        [Route("huy-hang")]
        public IHttpActionResult huyHang(int id)
        {
            Order data = new Order();
            try
            {
                using (var dbConn = Helpers.OrmliteConnection.openConn())
                {
                    var exist = new SqlHelper().ExecuteString("select * from [Order] where id=" + id);
                    if (exist.Rows.Count == 0)
                    {
                        return Json("Không tìm thấy đơn hàng cần cập nhật");
                    }
                    data.id = Int32.Parse(exist.Rows[0]["id"].ToString());
                    data.buyDate = DateTime.Parse(exist.Rows[0]["buyDate"].ToString());
                    data.recipientName = exist.Rows[0]["recipientName"].ToString();
                    data.phone = exist.Rows[0]["phone"].ToString();
                    data.status = -1;
                    data.user_id = Int32.Parse(exist.Rows[0]["user_id"].ToString());
                    data.deliveryAddress = exist.Rows[0]["deliveryAddress"].ToString();
                    data.deliveryCancelDay = DateTime.Now.Date;
                    dbConn.Update<Order>(data, s => s.id == id);

                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }
            return Ok(new { result = true, data = data });
        }
    }
}
