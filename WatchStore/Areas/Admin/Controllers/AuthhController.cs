using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WatchStore.Areas.Admin.Models;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class AuthhController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();

        // GET: Admin/Authh

       
        public ActionResult kCoQuyen()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f, LoginViewModel model)
        {  
            var TK = f["username"];
            var MK = f["password"];
            var ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == TK && n.PassAdmin  == MK);
            if (ad != null)
            {
              
                ViewBag.ThongBao = "Xin chào, Admin:" + ad.Hoten;
                FormsAuthentication.SetAuthCookie(ad.Hoten, false);
                Session["TaiKhoanAdmin"] = ad;
                Session["Admin"] = ad.UserAdmin;
                Session["Pw"] = ad.PassAdmin;
                return RedirectToAction("Index", "ThongKe");
            }
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!!!";
            TempData["thongbao"] = "Không tìm thấy khách hàng nào phù hợp.";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}