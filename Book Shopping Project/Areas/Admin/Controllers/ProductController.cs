using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Model.ViewModels;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product=new Product(),

                CategoryList=_unitOfWork.Category.GetAll().Select(cl=>new SelectListItem()
                {
                 Text=cl.Name,
                 Value=cl.Id.ToString()
                }),

                CoverTypeList=_unitOfWork.CoverType.GetAll().Select(cv=>new SelectListItem()
                {
                    Text=cv.Name,
                    Value=cv.Id.ToString()
                })
            };
            if (id == null)
                return View(productVM);
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
                return NotFound();
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var webrootpath = _webHostEnvironment.WebRootPath;

                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootpath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.Id != 0)
                    {
                        var imageExists = _unitOfWork.Product.Get(productVM.Product.Id).ImageURL;

                        productVM.Product.ImageURL = imageExists;
                    }
                    if(productVM.Product.ImageURL !=null)
                    {
                        var imagePath = Path.Combine(webrootpath, productVM.Product.ImageURL.TrimStart('\\'));
                        //var imagePath = _unitOfWork.Product.Get(productVM.Product.Id).ImageURL;
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))//filestram used to create image in folder
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageURL = @"\images\products\" + filename + extension;//to store image in database 
                }
                else
                {
                    if(productVM.Product.Id !=0)
                    {
                        var imageExists = _unitOfWork.Product.Get(productVM.Product.Id).ImageURL;
                        productVM.Product.ImageURL = imageExists;
                    }
                }
                if (productVM.Product.Id == 0)
                    _unitOfWork.Product.Add(productVM.Product);
                else
                    _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM = new ProductVM()
                {
                    CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text=cl.Name,
                        Value=cl.Id.ToString()
                    }),
                    CoverTypeList=_unitOfWork.CoverType.GetAll().Select(ct=>new SelectListItem()
                    {
                        Text=ct.Name,
                        Value=ct.Id.ToString()
                    })
                };
                if(productVM.Product.Id !=0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
                return View(productVM);
            }
        }
        #region APIs

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productInDb = _unitOfWork.Product.Get(id);
            if (productInDb == null)
                return Json(new { success = false, message = "Error While Deleting Data" });
            if(productInDb.ImageURL!="")
            {
                var webrootpath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webrootpath, productInDb.ImageURL.TrimStart('\\'));
                if(System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "$Data Deleted Successfully$" });
        }
        #endregion
    }
}
