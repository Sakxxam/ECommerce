using BookShoppingProject.DataAccess.Repository.IRepository;
//using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
                return View(category);
            var categoryInDb = _unitOfWork.Category.Get(id.GetValueOrDefault());
            return View(categoryInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(category);
            if (category.Id == 0)
                _unitOfWork.Category.Add(category);
            else
                _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryList = _unitOfWork.Category.GetAll();
            return Json(new { data = categoryList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();
            var categoryFromDb = _unitOfWork.Category.Get(id);
            if (categoryFromDb == null)
                return Json(new { success = false, message = "Error while delete data " });
            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data successfully deleted" });
        }
        #endregion
    }
}