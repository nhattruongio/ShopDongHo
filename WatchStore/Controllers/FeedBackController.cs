using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    public class FeedBackController : Controller
    {
        dbQLBandhDataContext data = new dbQLBandhDataContext();
        // GET: FeedBack

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Lienhe()
        {
            if (Session["TenDN"] == null || Session["TenDN"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Lienhe(FormCollection collection, KHACHHANG a)
        {
                LIENHE lh = new LIENHE();
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                var phanhoi = collection["Mess"];
                lh.MaKH = kh.MaKH;
                lh.HoTen = kh.HoTen;
                lh.DienthoaiKH = kh.DienthoaiKH;
                lh.Email = kh.Email;
                lh.Mess = phanhoi;
                data.LIENHEs.InsertOnSubmit(lh);
                data.SubmitChanges();   
            return this.Lienhe();
        }
    }
    
}