using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;


namespace WatchStore.Areas.Admin.Controllers
{
    public class QlySanPhamController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();
        // GET: Admin/QlySanPham
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLySanPham(string timkiem, int? page)
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
                List<DONGHO> listKQ = db.DONGHOs.Where(n => n.TenDH.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["tb"] = "Không tìm thấy sản phẩm nào phù hợp.";
                    return View(db.DONGHOs.OrderBy(n => n.TenDH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            }
            return View(db.DONGHOs.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            ViewBag.TenNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.TenSP = new SelectList(db.DONGHOs.ToList().OrderBy(n => n.TenDH), "MaSP", "TenSP");
            ViewBag.TenChuDe = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(DONGHO sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3)
        {
            int mansx = int.Parse(Request.Form["TenNSX"]);
            int macd = int.Parse(Request.Form["TenChuDe"]);
            ViewBag.TenChuDe = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.TenNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                var fileName2 = Path.GetFileName(fileUpload2.FileName);
                var fileName3 = Path.GetFileName(fileUpload3.FileName);
                //Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                var path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
                var path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path2))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload2.SaveAs(path2);
                }
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path3))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload3.SaveAs(path3);
                }
                sp.Anhbia = fileUpload.FileName;
                sp.Anh1 = fileUpload2.FileName;
                sp.Anh2 = fileUpload3.FileName;
             
                //sp.NgayCapNhat = DateTime.Now;
                sp.MaCD = macd;
                sp.MaNSX = mansx;
                sp.Ngaycapnhat = DateTime.Now;
                db.DONGHOs.InsertOnSubmit(sp);
                db.SubmitChanges();
                TempData["thanhcong"] = "Thêm mới sản phẩm thành công!";
            }
            else
                TempData["kthanhcong"] = "Thêm sản phẩm thất bại";
            return View();
        }
        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult ChinhSua(int masp)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            //Lấy ra đối tượng sp theo mã
            DONGHO sp = db.DONGHOs.SingleOrDefault(n => n.MaSP == masp);
         if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.TenNSX = new SelectList(db.NHASANXUATs.ToList(), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.TenCD = new SelectList(db.CHUDEs.ToList(), "MaCD", "TenChuDe", sp.MaCD);           
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(DONGHO sp,HttpPostedFileBase mt, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4, HttpPostedFileBase fileUpload5)
        {
            int a = sp.MaSP;
            int mansx = int.Parse(Request.Form["TenNSX"]);
            int soluong = int.Parse(Request.Form["Soluongton"]);
            int giaban = int.Parse(Request.Form["Giaban"]);
            //Đưa dữ liệu vào dropdownlist
            ViewBag.TenNSX = new SelectList(db.NHASANXUATs.ToList(), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.TenCD = new SelectList(db.CHUDEs.ToList(), "MaCD", "TenChuDe", sp.MaCD);
            if (fileUpload3 ==null && fileUpload4 == null && fileUpload5 == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh !!!";
                return View(sp);
            }
            //Thêm vào csdl
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName3 = Path.GetFileName(fileUpload3.FileName);

                    var fileName4 = Path.GetFileName(fileUpload4.FileName);

                    var fileName5 = Path.GetFileName(fileUpload5.FileName);
                    //Lưu đường dẫn của file
                    var path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);

                    var path4 = Path.Combine(Server.MapPath("~/Content/images"), fileName4);

                    var path5 = Path.Combine(Server.MapPath("~/Content/images"), fileName5);
                    //Kiểm tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path3))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload3.SaveAs(path3);
                    }
                    //Kiểm tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path4))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload4.SaveAs(path4);
                    }
                    //Kiểm tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path5))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload5.SaveAs(path5);
                    }
                    DONGHO sp2 = db.DONGHOs.Where(n => n.MaSP ==a).SingleOrDefault();
                    string anh1 = fileName3;
                    string anh2 = fileName4;
                    string anh3 = fileName5;
                    //Thực hiện cập nhật trong model
                    sp2.Anhbia =anh1;
                    sp2.Anh1 = anh2;
                    sp2.Anh2 = anh3; 
                    sp2.Ngaycapnhat = DateTime.Now;
                    sp2.MaDH = Request.Form["MaDH"];
                    sp2.TenDH= Request.Form["TenDH"];
                    sp2.Chatlieu = Request.Form["Chatlieu"];
                    sp2.Giaban = giaban;
                    sp2.Soluongton = soluong;
                    //sp2.Mota = Request.Form["MoTa"];

                    //Lưu vào csdl
                    UpdateModel(sp2);
                    db.SubmitChanges();
                    TempData["thanhcong"] = "Chỉnh sửa sản phẩm thành công!";
                    return RedirectToAction("QuanLySanPham");
                }
                else
                {
                    TempData["kthanhcong"] = "Chỉnh sửa thất bại!";
                }   
            }
            return View();
        }
        public ActionResult HienThi(int masp)
        {
            //Lấy ra đối tượng sp theo mã
            DONGHO sp = db.DONGHOs.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        public ActionResult ChiTiet(int masp, int? page)
        {
            TempData["Masp"] = masp;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //Lấy ra đối tượng sp theo mã
            var listdongho = db.CHITIETSPs.Where(n => n.MaSP == masp).OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize);
            if (listdongho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(listdongho);
        }
        //Xóa
        [HttpGet]
        public ActionResult Xoa(int masp)
        {
            //Lấy ra đối tượng sp theo mã
           DONGHO sp = db.DONGHOs.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int masp)
        {
            DONGHO sp = db.DONGHOs.SingleOrDefault(n => n.MaSP == masp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
         
            db.DONGHOs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("QuanLySanPham");
        }
    }
}