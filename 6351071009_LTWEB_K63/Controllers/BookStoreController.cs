using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using _6351071009_LTWEB_K63.Models;
using System.Web.UI;

namespace _6351071009_LTWEB_K63.Controllers
{
    public class BookStoreController : Controller
    {

        // GET: BookStore
        QLBansachEntities data = new QLBansachEntities();
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
            //return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ??  1);
            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
            //var book = from bk in data.SACHes select bk;
            //return View(book);
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
        public ActionResult SPTheochude(int id, int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach.ToPagedList(pageNum,pageSize));
        }
        public ActionResult SPTheoNXB(int id, int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
        
    }
}