using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class CheckoutController : Controller
    {
        BanVeMayBayDbContext db = new BanVeMayBayDbContext();
        // GET: Checkout
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            var list = new List<ticket>();

            int id = int.Parse(fc["datve"]);
            var list1 = db.tickets.Find(id);
            ViewBag.songuoi = int.Parse(fc["songuoi"]);
            
            list.Add(list1);
            // neu co ve khu hoi
            if (!string.IsNullOrEmpty(fc["datveKH"]))
            {
                int id2 = int.Parse(fc["datveKH"]);
                var list2 = db.tickets.Find(id2);
                ViewBag.ve2 = id2;
                list.Add(list2);
            }
            ViewBag.ve1 = id;
         
            return View("",list.ToList());
        }
        [HttpPost]
        public ActionResult checkOut(order order,FormCollection fc)
        {
            var iddd = 0;
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if(sessionUser !=null)
            {
                iddd = sessionUser.ID;
            }
            float total =  float.Parse(fc["total"]);
            order.created_ate = DateTime.Now;
            order.status = 1;
            order.total = total;
            order.CusId = iddd;
            db.orders.Add(order);
            db.SaveChanges();
            int lastOrderID = order.ID;
            ordersdetail orderDetail = new ordersdetail();
            int id1 = int.Parse(fc["veOnvay"]);
            orderDetail.ticketId = id1;
            orderDetail.quantity = order.guestTotal;
            orderDetail.orderid = lastOrderID;
            db.ordersdetails.Add(orderDetail);
            // tru so luong nghe
            var ticket = db.tickets.Find(id1);
            ticket.sold = ticket.sold + order.guestTotal;
            db.Entry(ticket).State = EntityState.Modified;

            //neu ton tai ve 2 chieu
            if (!string.IsNullOrEmpty(fc["veReturn"]))
            {
                int id2 = int.Parse(fc["veReturn"]);
                ordersdetail orderDetail2 = new ordersdetail();
                orderDetail2.ticketId = id2;
                orderDetail2.orderid = lastOrderID;
                orderDetail2.quantity = order.guestTotal;
                db.ordersdetails.Add(orderDetail2);
                // tru so luong nghe
                var ticket2 = db.tickets.Find(id2);
                ticket2.sold = ticket2.sold + order.guestTotal;
                db.Entry(ticket2).State = EntityState.Modified;
            }
            db.SaveChanges();
            return View("checkOutComfin", order);
        }
        // lay thong tin cac ve da book
        public ActionResult _BookingConnfig(int orderId)
        {
            var list = db.ordersdetails.Where(m => m.orderid == orderId).ToList();
            var list1 = new List<ticket>();
            foreach (var item in list)
            {
                ticket ticket = db.tickets.Find(item.ticketId);
                list1.Add(ticket);
            }

            return View("_BookingConnfig", list1.ToList());
        }
        
    }
}