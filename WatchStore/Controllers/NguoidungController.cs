using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WatchStore.Models;
using System.Data.SqlClient;
using Facebook;
using System.Configuration;
using System.Text.RegularExpressions;




namespace WatchStore.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBandhDataContext data = new dbQLBandhDataContext();

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: Nguoidung

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:yyyy/MM/dd}", collection["Ngaysinh"]);
            if (matkhau != matkhaunhaplai)
            {
                ViewData["Loi1"] = "không trùng mật khẩu!";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                ViewBag.Thongbao = "Đăng ký thành công";

                return RedirectToAction("Dangnhap");
            }

            return this.Dangky();

        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"].ToString();
            var matkhau = collection["Matkhau"].ToString();
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
            ViewBag.MaKH = kh.MaKH;

            if (kh != null)
            {
                Session["Taikhoan"] = kh;
                Session["TenDN"] = kh.HoTen;
                return RedirectToAction("Index", "WatchStore");
            }
            ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Dangnhap");
        }

        public ActionResult HienThiKH(int makh)
        {

            //Lấy ra đối tượng kh theo mã
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == makh);
            Session["Taikhoan"] = kh;
            Session["MaKH"] = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
  
    }
}
