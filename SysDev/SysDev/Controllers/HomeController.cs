using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SysDev.Models;

namespace SysDev.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        { 
            _context.Dispose();
        }

        protected ApplicationUser LoginUser()
        {
            string id = User.Identity.GetUserId();
            var account = _context.Users.Include(a => a.UserProfile).FirstOrDefault(p => p.Id == id);
            //var users = _context.UserProfiles.ToList();
            return account;
        }
        protected List<Permission> LoginUserPermission()
        {
            var role = User.IsInRole("SuperAdmin") ? "SuperAdmin" : "Employee";
            var userPermission = _context.Permissions.Where(m => m.IdentityRole.Name == role && m.MasterDetail.Name == "Users").ToList();
            return userPermission;
        }

        public ActionResult Index()
        {
           
            var viewModel = new DashboardViewModel
            {
                Account = LoginUser(),
                Permission = LoginUserPermission()
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}