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
    
    public class ReportsController : Controller
    {
        private ApplicationDbContext _context;

        public ReportsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

       
        // GET: Reports
        public ActionResult Index() 
        {
            //things to be added soon...
            return View();
        }

        public ActionResult AuditTrail()
        {
            var audits = _context.AuditTrails.ToList();
            var profile = _context.Users.Include(p => p.UserProfile).ToList();
            var modules = _context.MasterDetails.Include(m => m.MasterData).Where(m => m.MasterData.Name =="Modules").ToList();

            var currentUser = UserProfileController.LoginUser(User.Identity.GetUserId());
            var currentUserPermission = UserProfileController.GetUserPermission(currentUser);

            var reportView = new ReportViewModel
            {
                AuditTrails = audits,
                UserProfiles = profile,
                Account = currentUser,
                Permission = currentUserPermission,
                Modules = modules
            };

            return View(reportView);
        }

        public static bool AddAuditTrail(string action, string descriptions, string id, string activeModule)
        {
            
            var context = new ApplicationDbContext();
            var account = context.Users.Include(m => m.UserProfile).SingleOrDefault(a => a.Id == id);
            //var profile = context.UserProfiles.SingleOrDefault(p => p.Id == account.UserProfileId);
            var module = context.MasterDetails.SingleOrDefault(m => m.Name.Equals(activeModule, StringComparison.OrdinalIgnoreCase));

         
            context.AuditTrails.Add(new AuditTrail
            {
                UserProfileId = account.UserProfile.Id,
                ModuleId = module.Id,
                PageId = module.Id,
                Action = action,
                Description = descriptions,
                DateCreated = DateTime.Now,
                UserProfile = account.UserProfile
            });
            context.SaveChanges();
            
            return true;
        }
    }
}