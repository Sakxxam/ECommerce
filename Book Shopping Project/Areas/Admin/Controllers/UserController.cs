using BookShoppingProject.DataAccess.Data;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region APIs

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.ApplicationUsers.Include(c => c.Company).ToList();//AspNetUsers Table

            var roles = _context.Roles.ToList();//AspNetRoles Table

            var userRole = _context.UserRoles.ToList();//AspNetUserRole Table

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;

                if(user.Company==null)
                {
                    user.Company = new Company()
                    {
                        Name=""
                    };
                }
            }
            if(!User.IsInRole(SD.Role_Admin))
            {
                var userAdmin = userList.FirstOrDefault(u => u.Role == SD.Role_Admin);
                userList.Remove(userAdmin);
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            bool isLocked = false;
            var userInDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userInDb == null)
                return Json(new { success = false, message = "Error While Lock And UnLock User !!!" });
            if(userInDb!=null && userInDb.LockoutEnd>DateTime.Now)
            {
                userInDb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "User Successfully Locked" : "User Successfully UnLocked" });
        }
        #endregion
    }
}
