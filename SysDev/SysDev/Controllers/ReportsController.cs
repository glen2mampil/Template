using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SysDev.Models;

namespace SysDev.Controllers
{
    [Authorize]
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

        protected ApplicationUser LoginUser()
        {
            string id = User.Identity.GetUserId();
            var account = _context.Users.FirstOrDefault(p => p.Id == id);
            var users = _context.UserProfiles.ToList();
            return account;
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
            var profile = _context.UserProfiles.ToList();

            var reportView = new ReportViewModel
            {
                AuditTrails = audits,
                UserProfiles = profile,
                Account = LoginUser()
            };

            return View(reportView);
        }

        public static bool AddAuditTrail(string action, string descriptions, string id)
        {
            
            var context = new ApplicationDbContext();
            var account = context.Users.SingleOrDefault(a => a.Id == id);
            var profile = context.UserProfiles.SingleOrDefault(p => p.Id == account.UserProfileId);
            
            
            context.AuditTrails.Add(new AuditTrail
            {
                UserProfileId = profile.Id,
                ModuleId = 1,
                PageId = 1,
                Action = action,
                Description = descriptions,
                DateCreated = DateTime.Now,
                UserProfile = profile
            });
            context.SaveChanges();
            
            return true;
        }
    }
}