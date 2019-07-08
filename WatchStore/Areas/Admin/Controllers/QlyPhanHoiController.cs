using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class QlyPhanHoiController : Controller
    {
        dbQLBandhDataContext db = new dbQLBandhDataContext();
        // GET: Admin/QlyPhanHoi
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSachPhanHoi(string timkiem, int? page)
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

                List<LIENHE> listKQ = db.LIENHEs.Where(n => n.HoTen.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["tb"] = "Không tìm thấy phản hồi nào phù hợp.";
                    return View(db.LIENHEs.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.MaLH).ToPagedList(pageNumber, pageSize));
            }
            return View(db.LIENHEs.OrderBy(n => n.MaLH).ToPagedList(pageNumber, pageSize));
        }
        //Chi tiết
      
        public ActionResult HienThiLH(int malh)
        {
            if (Session["Admin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Authh");
            }
            //Lấy ra đối tượng sp theo mã
            LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLH == malh);
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lh);
        }
        public ActionResult ChiTiet(int malh, int? page)
        {
            TempData["Malh"] = malh;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //Lấy ra đối tượng sp theo mã
            var listlienhe = db.LIENHEs.Where(n => n.MaLH == malh).OrderBy(n => n.MaLH).ToPagedList(pageNumber, pageSize);
            if (listlienhe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(listlienhe);
        }
    }
}