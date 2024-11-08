using _6351071009_LTWEB_K63.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _6351071009_LTWEB_K63.Controllers
{
    public class GioHangController : Controller
    {
        QLBansachEntities data = new QLBansachEntities();

        // Lấy giỏ hàng từ Session
        private List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpGet]
        public ActionResult ThemGioHang(int iMasach, string strUrl)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);

            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach, data); // Truyền DbContext vào constructor
                lstGiohang.Add(sanpham);
            }
            else
            {
                sanpham.iSoluong++;
            }

            return Redirect(strUrl);
        }

        // Tính tổng số lượng trong giỏ hàng
        private int TongSoLuong()
        {
            List<Giohang> lstGiohang = Laygiohang();
            return lstGiohang.Sum(n => n.iSoluong);
        }

        // Tính tổng tiền trong giỏ hàng
        private double TongTien()
        {
            List<Giohang> lstGiohang = Laygiohang();
            return lstGiohang.Sum(n => n.dThanhtien);
        }

        // Hiển thị giỏ hàng
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }

        // Giỏ hàng hiển thị trên mọi trang (PartialView)
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        // Xóa sản phẩm trong giỏ hàng
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.Remove(sanpham);
            }
            return RedirectToAction("GioHang");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                int soLuongMoi = int.Parse(f["txtSoLuong"]);
                sanpham.iSoluong = soLuongMoi;
            }
            return RedirectToAction("GioHang");
        }

        // Xóa toàn bộ giỏ hàng
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }

        // Trang đặt hàng - Kiểm tra tài khoản và giỏ hàng
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Dangnhap", "User");
            }

            if (Session["Giohang"] == null || ((List<Giohang>)Session["Giohang"]).Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }

            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        // Xử lý đặt hàng (lưu vào cơ sở dữ liệu)
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();

            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            ddh.Ngaygiao = DateTime.Parse(collection["Ngaygiao"]);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;

            // Thêm đơn hàng vào DbSet và lưu vào cơ sở dữ liệu
            data.DONDATHANGs.Add(ddh);  // Thêm đơn hàng vào DbSet
            data.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu

            // Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                CHITIETDONDATHANG cthd = new CHITIETDONDATHANG
                {
                    MaDonHang = ddh.MaDonHang,
                    Masach = item.iMasach,
                    Soluong = item.iSoluong,
                    Dongia = (decimal)item.dDongia
                };

                // Thêm chi tiết đơn hàng vào DbSet và lưu vào cơ sở dữ liệu
                data.CHITIETDONDATHANGs.Add(cthd);  // Thêm chi tiết đơn hàng
            }

            // Lưu tất cả thay đổi vào cơ sở dữ liệu
            data.SaveChanges();

            // Xóa giỏ hàng trong session
            Session["Giohang"] = null;

            // Chuyển đến trang xác nhận đơn hàng
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        // Xác nhận đơn hàng
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}
