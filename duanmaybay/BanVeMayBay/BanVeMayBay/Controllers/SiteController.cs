using BanVeMayBay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class SiteController : Controller
    {
        BanVeMayBayDbContext db = new BanVeMayBayDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult flightSearch(FormCollection fc, int? page)
        {
            string typeTicket = fc["typeticket"];


            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            int songuoi = int.Parse(fc["songuoi"]);
            ViewBag.songuoi = songuoi;
            string noiBay = fc["departure_address"];
            string noiVe = fc["arrival_address"];
            string ngaybay = fc["departure_date"];

            ViewBag.url = "chuyen-bay";
            //convert sang mm/dd/yy cho may hieu 
            DateTime ngaybay1 = DateTime.ParseExact(ngaybay, "d/M/yyyy", CultureInfo.InvariantCulture);
                //sang mm/dd/yy
            string ngaybay2 = ngaybay1.ToString("MM-dd-yyyy");
            DateTime ngaybay3 = DateTime.Parse(ngaybay2);

            ViewBag.noiBay = noiBay;
            ViewBag.noiVe = noiVe;
            ViewBag.ngaybay = ngaybay;
           
            // neu la ve 2 chieu
            if (typeTicket.Equals("enable"))
            {
                string ngayve = fc["arrival_date"];
                DateTime ngayden1 = DateTime.ParseExact(ngayve, "d/M/yyyy", CultureInfo.InvariantCulture);
                string ngayden2 = ngayden1.ToString("MM-dd-yyyy");
                DateTime ngayden3 = DateTime.Parse(ngayden2);
                ViewBag.ngayden = ngayve;
                ViewBag.date = ngayden3;
                var list = db.tickets.Where(m => m.departure_address.Contains(noiBay) && m.arrival_address.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m=>m.status==1).ToList();
                return View("flightSearchReturn", list.ToPagedList(pageNumber, pageSize));
            }
            else
            {

                //ve 1 chieu
                var list = db.tickets.Where(m => m.departure_address.Contains(noiBay) && m.arrival_address.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m=>m.status==1).ToList();
                return View("flightSearchOnway", list.ToPagedList(pageNumber, pageSize));
            }

        }

        public ActionResult return_ticket(string date,string noibay, string noiden)
        {
            DateTime ngaybay3 = DateTime.Parse(date);
            var list = db.tickets.Where(m => m.departure_address.Contains(noiden) && m.arrival_address.Contains(noibay)).
               Where(m => m.departure_date == ngaybay3).Where(m => m.status == 1).ToList();
            return View("_returnTicket", list);
        }
        public ActionResult AllChuyenBay(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            ViewBag.url = "chuyen-bay";
            int pageNumber = (page ?? 1);
            ViewBag.breadcrumb = "Tất cả chuyến bay";
            var list_flight = db.tickets.Where(m => m.status == 1).ToList();
            return View("allflight", list_flight.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult postOftoPic(int? page, string slug)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            var singleC = db.Topics.Where(m => m.status == 1 && m.slug == slug).Where(m => m.status == 1).First();
            ViewBag.nameTopic = slug;
            ViewBag.url = "tin-tuc/" + slug + "";
            int pageNumber = (page ?? 1);
            var listPost = db.Posts.Where(m => m.status == 1 && m.topid == singleC.ID).Where(m => m.status == 1).ToList();
            return View("postOftoPic", listPost.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult topic()
        {

            var list = db.Topics.Where(m => m.status == 1).Where(m => m.status == 1).ToList();
            return View("_topic", list);
        }

        public ActionResult postSearch(string keyw, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.url = "tim-kiem-bai-viet?keyw=" + keyw + "";
            @ViewBag.nameTopic = "Tim kiếm từ khóa: " + keyw;
            var list = db.Posts.Where(m => m.title.Contains(keyw) || m.detail.Contains(keyw)).Where(m => m.status == 1).OrderBy(m => m.ID);
            return View("postOftoPic", list.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PostDetal(string slug)
        {

            var single = db.Posts.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.Bra = single.title;
            return View("PostDetal", single);
        }
        
            public ActionResult flightDetail(int id)
        {
            var single = db.tickets.Where(m => m.status == 1 && m.id == id).First();
            return View("flightDetail", single);
        }
        public ActionResult lienHe()
        {
            var single = db.Posts.Where(m => m.status == 1 && m.slug == "lien-he").First();
            return View("PostDetal", single);
        }

    }
}