using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    public class GiohangController : Controller
    {
        dbQLBandhDataContext data = new dbQLBandhDataContext();
        // GET: Giohang

        public ActionResult Index()
        {
            return View();
        }

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }

            return lstGiohang;
        }
        //Them hang vao gio
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            if (Session["TenDN"] == null || Session["TenDN"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }

                List<Giohang> lstGiohang = Laygiohang();
            Giohang sp = lstGiohang.Find(n => n.iMaSP == iMaSP);
            if (sp == null)
            {
                sp = new Giohang(iMaSP);
                lstGiohang.Add(sp);
                return Redirect(strURL);

            }
            else
            {
                sp.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "WatchStore");

            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMa)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sp = lstGiohang.SingleOrDefault(n => n.iMaSP == iMa);
            if (sp != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMa);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "WatchStrore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMa, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sp = lstGiohang.SingleOrDefault(n => n.iMaSP == iMa);
            if (sp != null)
            {
                sp.iSoluong = int.Parse(f["txtSoluong"].ToString());

            }
            return RedirectToAction("Giohang");

        }
        public ActionResult Xoatatcagiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "WatchStore");


        }
        [HttpPost]
         public ActionResult DatHang(FormCollection collection, KHACHHANG a)
         {

             //Kiểm tra đăng nhập
              if (Session["TenDN"] == null || Session["TenDN"].ToString() == "")
              {                   
                  var ten = collection["HoTen"];
                  var diachi = collection["DiachiKH"];
                  var sdt = collection["DienthoaiKH"];
                //Kiểm tra dơn hàng
                if (Session["Giohang"] == null)
                {
                    RedirectToAction("Index", "WatchStore");
                }
                  DONDATHANG dh = new DONDATHANG();
                List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;

                //List<Giohang> lstGiohang = Laygiohang();
                  dh.HoTen = ten;
                  dh.DienthoaiKH = sdt;                
                  dh.Ngaydat = DateTime.Now;
                  dh.TinhTrang = "Chưa duyệt";
                  dh.TongTien = (decimal)TongTien();
                  data.DONDATHANGs.InsertOnSubmit(dh);
                  data.SubmitChanges();
                //thêm chi tiết đơn hàng
                foreach (var item in lstGiohang)
                  {
                    CHITIETDONTHANG ctHD = new CHITIETDONTHANG();
                      ctHD.MaDonHang = dh.MaDonHang;
                      ctHD.MaSP = item.iMaSP;
                      ctHD.Soluong = item.iSoluong;
                      ctHD.Dongia = (decimal)item.dDongia;
                      data.CHITIETDONTHANGs.InsertOnSubmit(ctHD);
                  }
                data.SubmitChanges();
                Session["Giohang"] = null;
              }
              else
              {
                  //Kiểm tra dơn hàng
                  if (Session["Giohang"] == null)
                  {
                      RedirectToAction("Index", "WatchStore");
                  }
                //Thêm đơn đặt hàng
                  DONDATHANG dh = new DONDATHANG();
                  KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                List<Giohang> lstGiohang = Laygiohang();
               // List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;

                  dh.MaKH = kh.MaKH;
                  dh.DiachiGH = kh.DiachiKH;
                  dh.HoTen = kh.HoTen;
                  dh.DienthoaiKH = kh.DienthoaiKH;               
                  dh.Ngaydat = DateTime.Now;
                  dh.TinhTrang = "Chưa duyệt";
                  dh.TongTien = (decimal)TongTien();
                  data.DONDATHANGs.InsertOnSubmit(dh);
                  data.SubmitChanges();
                  foreach (var item in lstGiohang)
                  {
                    CHITIETDONTHANG ctHD = new CHITIETDONTHANG();
                    ctHD.MaDonHang = dh.MaDonHang;
                    ctHD.MaSP = item.iMaSP;
                    ctHD.Soluong = item.iSoluong;
                    ctHD.Dongia = (decimal)item.dDongia;
                    data.CHITIETDONTHANGs.InsertOnSubmit(ctHD);
                  }
                data.SubmitChanges();
                Session["Giohang"] = null;
              }
              return RedirectToAction("Index", "WatchStore");
          }
    }
}