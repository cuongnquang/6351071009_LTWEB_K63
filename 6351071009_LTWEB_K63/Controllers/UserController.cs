using _6351071009_LTWEB_K63.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _6351071009_LTWEB_K63.Controllers
{
    public class UserController : Controller
    {
        QLBansachEntities data = new QLBansachEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dangky
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        // POST: Dangky
        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            var hoten = collection["HotrnKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);

            // Validate the inputs
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (matkhau != nhaplaimatkhau)
            {
                ViewData["Loi4"] = "Mật khẩu không trùng khớp";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Điện thoại không được bỏ trống";
            }
            else
            {
                // Create a new KHACHHANG object and add it to the database
                KHACHHANG kh = new KHACHHANG
                {
                    HoTen = hoten,
                    Taikhoan = tendn,
                    Matkhau = matkhau,
                    Email = email,
                    DiachiKH = diachi,
                    DienthoaiKH = dienthoai,
                    Ngaysinh = DateTime.Parse(ngaysinh)
                };

                // Check if the username already exists
                var existingUser = data.KHACHHANGs.FirstOrDefault(u => u.Taikhoan == tendn);
                if (existingUser != null)
                {
                    ViewData["Loi8"] = "Tên đăng nhập đã tồn tại!";
                }
                else
                {
                    // Add the customer to the database
                    data.KHACHHANGs.Add(kh);
                    data.SaveChanges();  // Save changes to the database

                    return RedirectToAction("Dangnhap"); // Redirect to login page after successful registration
                }
            }
            return this.Dangky(); // Return to registration page in case of error
        }

        // GET: Dangnhap
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        // POST: Dangnhap
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            // Validate the inputs
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Search for the user in the database using Entity Framework
                KHACHHANG kh = data.KHACHHANGs.FirstOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);

                if (kh != null) // Login successful
                {
                    // Save user info in session
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");  // Redirect to home page after successful login
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View(); // Return to login page if validation fails
        }
        public ActionResult Logout()
        {
            Session["Taikhoan"] = null; // Clear the session
            return RedirectToAction("Index", "BookStore"); // Redirect to the home page
        }
    }
}
