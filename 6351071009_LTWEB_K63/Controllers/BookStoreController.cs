using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _6351071009_LTWEB_K63.Models;

namespace _6351071009_LTWEB_K63.Controllers
{
    public class BookStoreController : Controller
    {

        // GET: BookStore
        QLBansachEntities data = new QLBansachEntities();

        public ActionResult Index()
        {
            var book = from bk in data.SACHes select bk;
            return View(book);
        }
        public ActionResult ChuDe()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult NhaXuatBan()
        {
            var nhaxb = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxb);
        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
        
    }
}