using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class QlyKhachHangController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();
        // GET: Admin/QlyKhachHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSachKhachHang(string timkiem, int? page)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<KHACHHANG> listKQ = db.KHACHHANGs.Where(n => n.HoTen.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy khách hàng nào phù hợp.";
                    return View(db.KHACHHANGs.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
            }
            return View(db.KHACHHANGs.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
        }

        //Chi tiết
        public ActionResult HienThiKH(int makh)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            //Lấy ra đối tượng sp theo mã
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == makh);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
         //Xóa
        [HttpGet]
        public ActionResult Xoa(int makh)
        {
            //Lấy ra đối tượng sp theo mã
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == makh);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int makh)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == makh);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("DanhSachKhachHang");
        }
    }
}