using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SysDev.Models;
 
namespace SysDev.Controllers
{
    
    public class UserProfileController : Controller
    {
        private ApplicationDbContext _context;

        public UserProfileController()
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

        // GET: UserProfile
        public ActionResult Index()
        {
            var users = _context.UserProfiles.ToList();
            var accounts = _context.Users.ToList();
            var useraccount = new UserProfileViewModel
            {
                UserProfiles = users,
                Accounts = accounts,
                Account = LoginUser(),
                AccountRole = ""
            };
            return View(useraccount);
        }

        public ActionResult List()
        {
            var users = _context.UserProfiles.ToList();
            var accounts = _context.Users.ToList();
            var useraccount = new UserProfileViewModel
            {
                UserProfiles = users,
                Accounts = accounts,
                Account = LoginUser(),
                AccountRole = ""
            };
            return View(useraccount);
        }

        public ActionResult Details(int id)
        {
            var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == id);
            var account = _context.Users.FirstOrDefault(a => a.UserProfileId == profile.Id);

            if (profile == null)
                return HttpNotFound();

            var viewModel = new UserProfileViewModel
            {
                Account = account,
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ResetPassword(int id)
        {
            var account = _context.Users.SingleOrDefault(m=> m.UserProfileId == id);

            var password = "password1";
            var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();

            account.PasswordHash = passwordHasher.HashPassword(password);
            _context.SaveChanges();

            return RedirectToAction("Index", "UserProfile");
        }

        public ActionResult UpdateStatus(int id)
        {
            
            var account = _context.Users.SingleOrDefault(a => a.UserProfileId == id);
            if (account != null)
            {
                account.Status = (account.Status == "Active" ? "Inactive" : "Active");
                _context.SaveChanges();
                //return Json(new { success = false, responseText = "account: " + account.Email + " | Status: " + account.Status }, JsonRequestBehavior.AllowGet);
                var user = _context.UserProfiles.SingleOrDefault(u => u.Id == id);
                ReportsController.AddAuditTrail("Update User",
                    "User named " + user.FirstName + " " + user.LastName + " was set to " + account.Status,
                    User.Identity.GetUserId());
                return Json(new { success = false, responseText = "Model: " + user + " | account: " + account }, JsonRequestBehavior.AllowGet);

            }
            return RedirectToAction("Index", "UserProfile");
        }

        public ActionResult Edit(int id)
        {
            var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == id);
            var account = _context.Users.SingleOrDefault(p => p.UserProfileId == id);

            if (profile == null)
                return HttpNotFound();

            var prof = new AddUserViewModel
            {
                Profile = profile,
                Account = account
            };

            return View(prof);
        }

        [HttpPost]
        public ActionResult Save(AddUserViewModel model)
        {
            //return Json(new { success = false, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        
            if (model.Profile.Id == 0)
            {
                model.Profile.DateCreated = DateTime.Now.ToString("MMM-dd-yyyy hh:mm tt");
                _context.UserProfiles.Add(model.Profile);
                _context.SaveChanges();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var user = new ApplicationUser
                {
                    UserName = model.Account.UserName,
                    UserProfileId = model.Profile.Id,
                    UserProfile = model.Profile,
                    Email = model.Account.Email,
                    Status = "Active"
                };

                var chkUser = userManager.Create(user, "password1");
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "SuperAdmin");

                }
                ReportsController.AddAuditTrail("Add User",
                    model.Profile.FirstName + " " + model.Profile.LastName + " has been added.",
                    User.Identity.GetUserId());
            }
            else
            {
                var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == model.Profile.Id);
                //TryUpdateModel(prifle);
                if (profile != null)
                {
                    profile.FirstName = model.Profile.FirstName;
                    profile.LastName = model.Profile.LastName;
                    profile.MiddleName = model.Profile.MiddleName;
                    profile.Gender = model.Profile.Gender;
                    profile.ContactNo = model.Profile.ContactNo;
                    profile.Address = model.Profile.Address;
                    profile.CompanyId = model.Profile.CompanyId;
                    profile.CompanyName = model.Profile.CompanyName;
                    profile.MaritalStatus = model.Profile.MaritalStatus;
                }


                var account = _context.Users.SingleOrDefault(a => a.UserProfileId == model.Profile.Id);
                if (account != null)
                {
                    account.Email = model.Account.Email;
                    account.UserName = model.Account.UserName;
                }
                _context.SaveChanges();
                ReportsController.AddAuditTrail("Update User",
                    model.Profile.FirstName + " " + model.Profile.LastName + "'s information was Updated",
                    User.Identity.GetUserId());
            }

            return RedirectToAction("Index", "UserProfile");
        }

        public ActionResult Delete(int id)
        {
            var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == id);

            if (profile == null)
                return HttpNotFound();

            _context.UserProfiles.Remove(profile);
            _context.SaveChanges();

            ReportsController.AddAuditTrail("Update User",
               "User named " + profile.FirstName + " " + profile.LastName + " was Deleted",
                User.Identity.GetUserId());

            return RedirectToAction("Index", "UserProfile");
        }
    }
}