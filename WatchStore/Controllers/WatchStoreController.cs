using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using PagedList;
using PagedList.Mvc;
namespace WatchStore.Controllers
{
    public class WatchStoreController : Controller
    {
        dbQLBandhDataContext data = new dbQLBandhDataContext();
        private List<DONGHO> Laydonghomoi(int count) => data.DONGHOs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        // GET: WatchStore
        public ActionResult Index(int ? page)
        {
            int pageSize=4;
            int pageNum = (page ?? 1);

            var donghomoi = Laydonghomoi(12);
            return View(donghomoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult SPtheochude(int id)
        {
            var dongho = from dh in data.DONGHOs where dh.MaCD == id select dh;
            return View(dongho);
        }
        public ActionResult Details(int id)
        {
            var dongho = from dh in data.DONGHOs where dh.MaSP == id select dh;
            return View(dongho.Single());
        }
        public ActionResult AboutUS()
        {
            return View();
        }
        public ActionResult Shop(string timkiem,int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            if (timkiem != null)
            {
                List<DONGHO> listKQ = data.DONGHOs.Where(n => n.TenDH.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["tb"] = "Không tìm thấy sản phẩm nào phù hợp.";
                    return View(data.DONGHOs.OrderBy(n => n.TenDH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            }
            return View(data.DONGHOs.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));

           
        }

    }
}
