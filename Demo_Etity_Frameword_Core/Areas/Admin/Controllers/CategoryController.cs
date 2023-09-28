using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo_Etity_Frameword_Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Demo_Etity_Frameword_Core.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }
        // list category
        public IActionResult Index()
        {
            var dsCategory = db.categories.ToList();
            return View(dsCategory);
        }
        // Acction Xử lý thêm mới
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Create")] // actionname đổi đường dẫn đến "Create" mà không phải là Create_Post
        public IActionResult Create_Post(Category obj)
        {
            // Tiến hành thêm obj vào table Category 
            // 1. Kiểm tra dữ liệu
            if (ModelState.IsValid) // hợp lệ
            {
                db.categories.Add(obj);
                db.SaveChanges();
                TempData["Success"] = "Thêm thành công";
                return RedirectToAction("Index");
            }
            return View();
        }
        // Acction xử lý Edit
        public IActionResult Edit(int id)
        {
            // Truy vấn Category theo id 
            var obj = db.categories.Find(id); // Cách 1 truy vấn theo khóa chính thì nên dùng
            //var obj2 = db.categories.SingleOrDefault(s => s.Id == id); // cách 2
            //var obj3 = db.categories.Where(w => w.Id == id).SingleOrDefault(); // Cách 3

            // trường hợp obj không tồn tại
            if (obj == null) return NotFound();
            // Trả về view Edit để chỉnh sửa
            
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            // Tiến hành thêm obj vào table Category 
            // 1. Kiểm tra dữ liệu
            if (ModelState.IsValid) // hợp lệ
            {
                db.categories.Update(obj);
                //db.Update<Category>(obj);
                db.SaveChanges();
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View();
        } 
        public IActionResult Delete(int id)
        {
            // Truy vấn Category theo id 
            var obj = db.categories.Find(id); // Cách 1 truy vấn theo khóa chính thì nên dùng
            //var obj2 = db.categories.SingleOrDefault(s => s.Id == id); // cách 2
            //var obj3 = db.categories.Where(w => w.Id == id).SingleOrDefault(); // Cách 3

            // trường hợp obj không tồn tại
            if (obj == null) return NotFound();
            // Trả về view Delete
            db.categories.Remove(obj);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công";
            return RedirectToAction("Index");
        }
        
    }
}
