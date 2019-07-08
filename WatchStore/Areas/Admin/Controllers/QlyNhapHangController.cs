using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using PagedList;
using PagedList.Mvc;
namespace WatchStore.Areas.Admin.Controllers
{
    public class QlyNhapHangController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();

        // GET: Admin/QlyNhapHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSachPhieuNhap(string timkiem, int? page)
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
                List<PHIEUNHAP> listKQ = db.PHIEUNHAPs.Where(n => n.MaPN.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thanhcong"] = "Không tìm thấy phiếu nhập nào phù hợp.";
                    return View(db.PHIEUNHAPs.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
            }
            return View(db.PHIEUNHAPs.OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietPN(int mapn, string timkiem, int? page)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.TuKhoa = timkiem;
            TempData["MaPN"] = mapn;
            if (timkiem != null)
            {
                List<CHITIETPN> listKQ = db.CHITIETPNs.Where(n => n.MaPN.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(db.CHITIETPNs.Where(n => n.MaPN == mapn).OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderByDescending(n => n.MaPN == mapn).ToPagedList(pageNumber, pageSize));
            }
            return View(db.CHITIETPNs.Where(n => n.MaPN == mapn).OrderByDescending(n => n.MaPN).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoiPN()
        {
            ViewBag.TenNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiPN(PHIEUNHAP pn)
        {
            int mancc = int.Parse(Request.Form["TenNCC"]);
            ViewBag.TenNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            
            if (ModelState.IsValid)
            {
                //chèn dữ liệu
                pn.MaNCC = mancc;
                db.PHIEUNHAPs.InsertOnSubmit(pn);

                db.SubmitChanges();
                //Lưu vào CSDL
              
                TempData["thanhcong"] = "Thêm phiếu nhập thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm phiếu nhập thất bại";
            return View();
        }
        [HttpGet]
        public ActionResult ThemMoiCTPN(int mapn)
        {
            ViewBag.TenSP = new SelectList(db.DONGHOs.ToList().OrderBy(n => n.TenDH), "MaSP", "TenDH");
           
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiCTPN(CHITIETPN ctpn)
        {
            TempData["MaPN"] = ctpn;
            int masp = int.Parse(Request.Form["TenSP"]);
          
            int soluong = int.Parse(Request.Form["SoLuong"]);
            int gia = int.Parse(Request.Form["Gia"]);
            ViewBag.TenSP = new SelectList(db.DONGHOs.ToList().OrderBy(n => n.TenDH), "MaSP", "TenDH");
           
            //var ctpnkt = db.CHITIETPNs.Where(n => n.MaSP == masp && n.MaSize == masize && n.MaMau == mamau).Count();
            CHITIETSP ctsp = db.CHITIETSPs.SingleOrDefault(n => n.MaSP == masp);
            CHITIETSP ctspn = new CHITIETSP();
            //if (ctpnkt > 0)
            //{
            //    TempData["loi"] = "Sản phẩm trong phiếu nhập tồn tại";
            //    ModelState.AddModelError("loi", " ");
            //    return RedirectToAction("ThemMoiCTPN");
            //}
            if (ModelState.IsValid)
            {
                if (ctsp == null)
                {
                    ctspn.MaSP = masp;
                    
                    ctspn.SoLuong = soluong;
                    db.CHITIETSPs.InsertOnSubmit(ctspn);

                    db.SubmitChanges();

                   
                }
                //chèn dữ liệu
                ctpn.MaSP = masp;
                
                ctpn.ThanhTien = soluong * gia;
                db.CHITIETPNs.InsertOnSubmit(ctpn);

                //Lưu vào CSDL

                db.SubmitChanges();
                TempData["thanhtien"] = ctpn.ThanhTien;
                TempData["thanhcong"] = "Thêm thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm thất bại";
            return View();
        }

        public ActionResult HuyPhieuNhap(int mapn)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            List<CHITIETPN> ctpn = db.CHITIETPNs.Where(n => n.MaPN == mapn).ToList();
            PHIEUNHAP pn = db.PHIEUNHAPs.Where(n => n.MaPN == mapn).SingleOrDefault();
           
            pn.TinhTrang = "Đã hủy";
            foreach (var item in ctpn)
            {
                db.CHITIETPNs.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            db.SubmitChanges();
            return RedirectToAction("DanhSachPhieuNhap", "QlyNhapHang");
        }

    }
}