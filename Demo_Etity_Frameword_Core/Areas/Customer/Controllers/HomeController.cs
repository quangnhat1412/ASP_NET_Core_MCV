using Demo_Etity_Frameword_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Etity_Frameword_Core.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Cus)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = _db;
        }

        public IActionResult Index(int ?page)
        {
            int pagesize = 6;
            int pageindex; // page hện hành
            if (page == null)
                pageindex = 1;
            else
                pageindex = (int)page;

            // truy van du lieu co phan trang
            var dsproduct = db.products.ToList();
            // Thống kê tổng số trang có thể có 
            var pagesum = dsproduct.Count() / pagesize + (dsproduct.Count % pagesize > 0 ? 1 : 0);
            // truyền qua view 
            ViewBag.pageindex = pageindex;
            ViewBag.pagesum = pagesum;
            return View(dsproduct.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList());
        }

        public IActionResult Detail(int id)
        {
            var obj = db.products.Find(id);
            if (obj == null) return NotFound();
            var listProduct = db.products.Where(w => w.CategoryID == obj.CategoryID && w.Id != obj.Id);
            var model = new Tuple<Product, IEnumerable<Product>>(obj, listProduct);
            return View(model);
        }

    }
}
