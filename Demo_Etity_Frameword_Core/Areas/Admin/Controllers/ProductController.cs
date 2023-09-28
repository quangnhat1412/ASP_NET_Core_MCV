using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo_Etity_Frameword_Core.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Demo_Etity_Frameword_Core.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment ihem;
        public ProductController(ApplicationDbContext _db, IHostingEnvironment _ihem)
        {
            db = _db;
            ihem = _ihem;
        }

        public IActionResult Index(int ?page)
        {
            int pagesize = 5;
            int pageindex; // page hện hành
            if (page == null)
                pageindex = 1;
            else
                pageindex = (int)page;

            // truy van du lieu co phan trang
            var dsproduct = db.products.Include(x => x.Category).ToList();
            // Thống kê tổng số trang có thể có 
            var pagesum = dsproduct.Count() / pagesize + (dsproduct.Count % pagesize > 0 ? 1 : 0);
            // truyền qua view 
            ViewBag.pageindex = pageindex;
            ViewBag.pagesum = pagesum;
            return View(dsproduct.Skip((pageindex-1)*pagesize).Take(pagesize).ToList());
        }
        // Acction Xử lý thêm mới
        public IActionResult Create()
        {
            // truyền danh sách thể loại cho view để sinh ra điểu khiển dropdownlist
            ViewBag.ListCategory = db.categories.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
            return View();
        }
        [HttpPost, ActionName("Create")] // actionname đổi đường dẫn đến "Create" mà không phải là Create_Post
        public IActionResult Create_Post(Product obj, IFormFile fUpload)
        {
            // Tiến hành thêm obj vào table product 
            // 1. Kiểm tra dữ liệu
            if (ModelState.IsValid) // hợp lệ
            {
                if (fUpload != null && fUpload.Length > 0)
                {
                    // Get the file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fUpload.FileName);
                    string path = Path.Combine(ihem.WebRootPath, @"images/products"); // lay duong dan luu tru
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        fUpload.CopyTo(fileStream);
                        // tự động đóng luồng
                    }                    // Save the file name to obj.ImageUrl
                    obj.ImageUrl = fileName;
                }
                db.products.Add(obj);
                db.SaveChanges();
                TempData["Success"] = "Thêm thành công";

                // truy van du lieu co phan trang
                int pagesize = 5;
                var dsproduct = db.products.Include(x => x.Category).ToList();
                // Thống kê tổng số trang có thể có 
                var pagesum = dsproduct.Count() / pagesize + (dsproduct.Count % pagesize > 0 ? 1 : 0);
                // truyền qua view 

                return RedirectToAction("Index", new { page = pagesum});
            }
            return View();
        }
        // Acction xử lý Edit
        public IActionResult Edit(int id, int pageindex)
        {
            // Truy vấn product theo id 
            var obj = db.products.Find(id); // Cách 1 truy vấn theo khóa chính thì nên dùng
            //var obj2 = db.categories.SingleOrDefault(s => s.Id == id); // cách 2
            //var obj3 = db.categories.Where(w => w.Id == id).SingleOrDefault(); // Cách 3

            // trường hợp obj không tồn tại
            if (obj == null) return NotFound();
            // Trả về view Edit để chỉnh sửa
            ViewBag.ListCategory = db.categories.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
            ViewBag.pageindex = pageindex;
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Product obj, IFormFile fUpload, int pageindex)
        {
            // Tiến hành thêm obj vào table product 
            // 1. Kiểm tra dữ liệu
            if (ModelState.IsValid) // hợp lệ
            {
                if (fUpload != null && fUpload.Length > 0)
                {
                    // Get the file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fUpload.FileName);
                    string path = Path.Combine(ihem.WebRootPath, @"images/products"); // lay duong dan luu tru
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        fUpload.CopyTo(fileStream); 
                        // tự động đóng luồng
                    }
                    // fUpload.CopyTo(new FileStream(Path.Combine(path, fileName), FileMode.Create)); // tien hanh sao chep len may chu
                    // đóng luồng lại

                    //Xóa ảnh củ 
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        string imagePath = Path.Combine(ihem.WebRootPath, @"images/products", obj.ImageUrl);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // Save the file name to obj.ImageUrl
                    obj.ImageUrl = fileName;
                    
                }
                db.products.Update(obj);
                //db.Update<product>(obj);
                db.SaveChanges();
                TempData["Success"] = "Cập nhật thành công";


                return RedirectToAction("Index", new { page = pageindex });
            }
            return View();
        }
        //public IActionResult Delete(int id, int pageindex)
        //{
        //    // Truy vấn product theo id 
        //    var obj = db.products.Find(id); // Cách 1 truy vấn theo khóa chính thì nên dùng
        //    //var obj2 = db.categories.SingleOrDefault(s => s.Id == id); // cách 2
        //    //var obj3 = db.categories.Where(w => w.Id == id).SingleOrDefault(); // Cách 3

        //    // trường hợp obj không tồn tại
        //    if (obj == null) return NotFound();
        //    // Trả về view Delete
        //    db.products.Remove(obj);
        //    db.SaveChanges();

        //    if (!string.IsNullOrEmpty(obj.ImageUrl))
        //    {
        //        string imagePath = Path.Combine(ihem.WebRootPath, @"images/products", obj.ImageUrl);
        //        if (System.IO.File.Exists(imagePath))
        //        {
        //            System.IO.File.Delete(imagePath);
        //        }
        //    }
        //    TempData["Success"] = "Xóa thành công";

        //    return RedirectToAction("Index", new { page = pageindex });
        //}

        #region API_CALL
        [HttpGet]
        public IActionResult getAll()
        {
            var lstProduct = db.products.Include(x => x.Category).ToList();
            return Json(new { data = lstProduct });
        }

        [HttpDelete]
        public IActionResult Delete(int id, int pageindex)
        {
            // Truy vấn product theo id 
            var obj = db.products.Find(id); // Cách 1 truy vấn theo khóa chính thì nên dùng
            //var obj2 = db.categories.SingleOrDefault(s => s.Id == id); // cách 2
            //var obj3 = db.categories.Where(w => w.Id == id).SingleOrDefault(); // Cách 3

            // trường hợp obj không tồn tại
            if (obj == null) return NotFound();
            // Trả về view Delete
            db.products.Remove(obj);
            db.SaveChanges();

            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                string imagePath = Path.Combine(ihem.WebRootPath, @"images/products", obj.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            //return RedirectToAction("Index", new { page = pageindex });
            return Json(new { success = true, message = "Xóa thành công" });

        }
        #endregion
    }
}

