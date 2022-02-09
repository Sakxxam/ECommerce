using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + ","+SD.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
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
            Company company = new Company();
            if (id == null)
                return View(company);
            else
                company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
                return NotFound();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            //if (company == null)
            //    return NotFound();
            //if (!ModelState.IsValid)
            //    return View(company);
            //if (company.Id == 0)
            //    _unitOfWork.Company.Add(company);
            //else
            //    _unitOfWork.Company.Update(company);
            //_unitOfWork.Save();
            //return RedirectToAction(nameof(Index));

            if (ModelState.IsValid)
            {
                var webrootpath = _webHostEnvironment.WebRootPath;

                 var files = HttpContext.Request.Form.Files; //To Access File
                 
                //IFormFileCollection files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootpath, @"images\companies");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (company.Id != 0)
                    {
                        var imageExists = _unitOfWork.Company.Get(company.Id).ImageURL;

                        company.ImageURL = imageExists;
                    }
                    if (company.ImageURL != null)
                    {
                        var imagePath = Path.Combine(webrootpath, company.ImageURL.TrimStart('\\'));
                        //var imagePath = _unitOfWork.Product.Get(productVM.Product.Id).ImageURL;
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))//filestram used to create image in folder
                    {
                        files[0].CopyTo(fileStream);
                    }
                    company.ImageURL = @"\images\companies\" + filename + extension;//to store image in database 
                }
                else
                {
                    if (company.Id != 0)
                    {
                       // company = new Company();
                        var imageExists = _unitOfWork.Product.Get(company.Id).ImageURL;
                        company.ImageURL = imageExists;
                    }
                }
                if (company.Id == 0)
                    _unitOfWork.Company.Add(company);
                else
                    _unitOfWork.Company.Update(company);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                company = new Company();
                if (company.Id != 0)
                {
                    company = _unitOfWork.Company.Get(company.Id);
                }
                return View(company);
            }
        }

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var companyInDb = _unitOfWork.Company.Get(id);
            if (companyInDb == null)
                return Json(new { success = false, message = "Error While Deleting Data!!!" });
            else
                _unitOfWork.Company.Remove(companyInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion
    }
}
