using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class QlyDonHangController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();

        // GET: Admin/QlyDonHang
        public ActionResult Index()
        {
            

            return View();
        }
        public ActionResult DanhSachDonHang(string timkiem, int? page)
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
                List<DONDATHANG> listKQ = db.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) || n.KHACHHANG.HoTen.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.DONDATHANGs.Where(n => n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.TinhTrang != "Chưa duyệt" ).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONDATHANGs.Where(n => n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachChuaDuyet(string timkiem, int? page)
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
                List<DONDATHANG> listKQ = db.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) || n.KHACHHANG.HoTen.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.DONDATHANGs.Where(n => n.Ngaygiao.ToString() == null && n.TinhTrang != "Đã duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.Ngaygiao.ToString() == null && n.TinhTrang != "Đã duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONDATHANGs.Where(n => n.Ngaygiao.ToString() == null &&  n.TinhTrang != "Đã duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DonHangChoGiao(string timkiem, int? page)
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
                List<DONDATHANG> listKQ = db.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) ||  n.KHACHHANG.HoTen.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.DONDATHANGs.Where(n => n.Tinhtranggiaohang == null && n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.Tinhtranggiaohang == null && n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONDATHANGs.Where(n => n.Tinhtranggiaohang == null && n.TinhTrang != "Chưa duyệt").OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DuyetDonHang(int madh)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            DONDATHANG dh = db.DONDATHANGs.Where(n => n.MaDonHang == madh).SingleOrDefault();
            dh.TinhTrang = "Đã duyệt";
            db.SubmitChanges();
            return RedirectToAction("DanhSachDonHang", "QlyDonHang");
        }
        public ActionResult GiaoHang(int madh)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }         
            DONDATHANG dh = db.DONDATHANGs.Where(n => n.MaDonHang == madh).SingleOrDefault();
            dh.Ngaygiao = DateTime.Now;
            dh.ThanhToan = "Tiền mặt";
            dh.Dathanhtoan = "Đã thanh toán";
            dh.Tinhtranggiaohang = "Đã giao hàng";
            db.SubmitChanges();
            return RedirectToAction("DonHangChoGiao", "QlyDonHang");
        }
        public ActionResult HuyDonHang(int madh)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            List<CHITIETDONTHANG> cthd = db.CHITIETDONTHANGs.Where(n => n.MaDonHang == madh).ToList();
            DONDATHANG dh = db.DONDATHANGs.Where(n => n.MaDonHang == madh).SingleOrDefault();

            dh.TinhTrang = "Đã hủy";
            foreach (var item in cthd)
            {
                db.CHITIETDONTHANGs.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            db.DONDATHANGs.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("DanhSachDonHang", "QlyDonHang");
        }

        public ActionResult ChiTietDH(int madh, string timkiem, int? page)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.TuKhoa = timkiem;
            if (timkiem != null)
            {
                List<CHITIETDONTHANG> listKQ = db.CHITIETDONTHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.CHITIETDONTHANGs.Where(n => n.MaDonHang == madh).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaDonHang == madh).ToPagedList(pageNumber, pageSize));
            }
            return View(db.CHITIETDONTHANGs.Where(n => n.MaDonHang == madh).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
    }
}