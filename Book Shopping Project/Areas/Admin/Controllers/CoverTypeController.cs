using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
                return View(coverType);
            var param = new DynamicParameters();
            param.Add("@id", id);
            var coverTypeInDb = _unitOfWork.SP_CALLS.OneRecord<CoverType>(SD.Proc_GetCoverType, param);
            // var coverTypeInDb = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverTypeInDb == null)
                return NotFound();
            return View(coverTypeInDb);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(coverType);
            var param = new DynamicParameters();
            param.Add("@name", coverType.Name);
            if (coverType.Id == 0)
                _unitOfWork.SP_CALLS.Execute(SD.Proc_CreateCoverType, param);
            //_unitOfWork.CoverType.Add(coverType);
            else
            {
                param.Add("@id", coverType.Id);
                _unitOfWork.SP_CALLS.Execute(SD.Proc_UpdateCoverType, param);
                // _unitOfWork.CoverType.Update(coverType);
            }

            // _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region CALL APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var coverTypeList = _unitOfWork.SP_CALLS.List<CoverType>(SD.Proc_GetCoverTypes);
            //var coverTypeList = _unitOfWork.CoverType.GetAll();
            return Json(new { data = coverTypeList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();
            var param = new DynamicParameters();
            param.Add("@id", id);
            var coverTypeInDb = _unitOfWork.SP_CALLS.OneRecord<CoverType>(SD.Proc_GetCoverType, param);
            //var coverTypeInDb = _unitOfWork.CoverType.Get(id);
            if (coverTypeInDb == null)
                return Json(new { success = false, message = "Error While Deleting Data" });
            _unitOfWork.CoverType.Remove(coverTypeInDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Data Successfully Deleted" });
        }
        #endregion
    }
}
