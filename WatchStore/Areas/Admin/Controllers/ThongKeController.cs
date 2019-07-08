using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();

        // GET: Admin/ThongKe
        public ActionResult Index(DateTime? NgayA, DateTime? NgayB)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            //Tính tổng doanh thu
            TempData["TongDoanhThu"] = db.DONDATHANGs.Where(n => n.TinhTrang == "Đã duyệt" && n.Ngaygiao.ToString() != "").Sum(n => n.TongTien);

            //Đếm đơn hàng chưa duyệt
            TempData["DonHangChuaDuyet"] = db.DONDATHANGs.Where(n => n.TinhTrang == "Chưa duyệt").Count();

            //Đếm đơn hàng chờ giao
            TempData["DonHangChoGiao"] = db.DONDATHANGs.Where(n => n.TinhTrang == "Đã duyệt" && n.Ngaygiao.ToString() ==null).Count();

            //Đếm số khách hàng
            TempData["TongKhachHang"] = db.KHACHHANGs.Count();
            return View(db.DONDATHANGs.Where(n => n.Ngaydat >= NgayA && n.Ngaydat < NgayB && n.TinhTrang == "Đã duyệt").ToList());
        }
        public Object TKDoanhThuTheoNgay(DateTime? NgayA, DateTime? NgayB)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            TempData["NgayA"] = NgayA;
            TempData["NgayB"] = NgayB;
            if (NgayA == null || NgayB == null)
            {
                TempData["DoanhThuTheoNgay"] = "0";
                return RedirectToAction("Index", "ThongKe");
            }
            TempData["DoanhThuTheoNgay"] = db.DONDATHANGs.Where(n => n.Ngaygiao > NgayA && n.Ngaygiao <= NgayB && n.TinhTrang == "Đã duyệt" && n.Dathanhtoan == "Đã thanh toán").Sum(n => n.TongTien);
            return RedirectToAction("Index", "ThongKe");
        }

    }
}