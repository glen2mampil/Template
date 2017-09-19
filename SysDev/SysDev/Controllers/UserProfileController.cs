using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SysDev.Models;
using System.Data.Entity;

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
            var account = _context.Users.Include(a => a.UserProfile).FirstOrDefault(p => p.Id == id);
            //var users = _context.UserProfiles.ToList();
            return account;
        }

        protected Permission LoginUserPermission()
        {
            var role = User.IsInRole("SuperAdmin") ? "SuperAdmin" : "Employee";
            var userPermission = _context.Permissions.SingleOrDefault(m => m.IdentityRole.Name == role && m.MasterDetail.Name =="Users");
            return userPermission;
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            List<UserProfile> users = _context.UserProfiles.ToList();
            List<IdentityRole> roles = _context.Roles.ToList();
            var accounts = _context.Users.Include(a => a.UserProfile).ToList();
            var useraccount = new UserProfileViewModel
            {
                UserProfiles = users,
                Accounts = accounts,
                Account = LoginUser(),
                Roles = roles,
                Permission = LoginUserPermission()
            };
            if (User.IsInRole("SuperAdmin"))
            {
                return View("IndexReadOnly",useraccount);
                //return View(useraccount);
            }
            return View("NotAllowed", useraccount);
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
                Account = account
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            var account = _context.Users.Include(a => a.UserProfile).SingleOrDefault(m=> m.Id == id);


            if (account == null)
                return HttpNotFound();

            var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();

            account.PasswordHash = passwordHasher.HashPassword("password1");
            _context.SaveChanges();

            string fullName = account.UserProfile.FirstName + " " + account.UserProfile.LastName;
            ReportsController.AddAuditTrail(UserAction.Update,
                "<strong>" + fullName + "</strong>'s password was reseted ",
                User.Identity.GetUserId(), Page.Users);

            return Json(new { success = true, responseText = "Password successfuly Change!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(string newpassword)
        {
            var user = LoginUser();
            var account = _context.Users.SingleOrDefault(m => m.Id == user.Id);

            if (account == null)
                return HttpNotFound();

            var password = newpassword;
            var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();

            account.PasswordHash = passwordHasher.HashPassword(password);
            _context.SaveChanges();

            string fullName = account.UserProfile.FirstName + " " + account.UserProfile.LastName;
            ReportsController.AddAuditTrail(UserAction.Update,
                "<strong>" + fullName + "</strong> reset his/her password ",
                User.Identity.GetUserId(), Page.Users);

            return Json(new { success = true, responseText = "Password successfuly Change!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatus(string id)
        {
            
            var account = _context.Users.SingleOrDefault(a => a.Id == id);
            var user = _context.UserProfiles.SingleOrDefault(u => u.Id == account.UserProfileId);

            if (account != null && user != null)
            {
                account.Status = (account.Status == "Active" ? "Inactive" : "Active");
                _context.SaveChanges();

                string fullName = user.FirstName + " " + user.LastName;
                ReportsController.AddAuditTrail(UserAction.Update,
                    "<strong>" + fullName + "</strong> was set to " + account.Status,
                    User.Identity.GetUserId(), Page.Users);
                return Json(new { success = true, responseText = user + " has set to " + account.Status }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, responseText = "Nothings happen, Try again later." }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Save(UserProfileViewModel model)
        {
            //return Json(new { success = false, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        
            if (model.EditProfile.Id == 0 && model.EditAccount.Id == null)
            {
                model.EditProfile.DateCreated = DateTime.Now.ToString("MMM-dd-yyyy hh:mm tt");
                _context.UserProfiles.Add(model.EditProfile);
                _context.SaveChanges();

                //Temp code
                //var roleStore = new RoleStore<IdentityRole>(_context);
                //var roleManager = new RoleManager<IdentityRole>(roleStore);
                //roleManager.Create(new IdentityRole("CanManageUsers"));
                


                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
               
                var user = new ApplicationUser
                {
                    UserName = model.EditAccount.UserName,
                    UserProfileId = model.EditProfile.Id,
                    UserProfile = model.EditProfile,
                    Email = model.EditAccount.Email,
                    Status = "Active"
                };

                var chkUser = userManager.Create(user, "password1");
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Employee");

                }
                string fullName = model.EditProfile.FirstName + " " + model.EditProfile.LastName;
                ReportsController.AddAuditTrail(UserAction.Create,
                    "<strong>" +fullName + "</strong> has been added." ,
                    User.Identity.GetUserId(), Page.Users);
                return Json(new { success = true, responseText = fullName + " <small>was added.</small>", data = fullName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                

                try
                {
                    var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == model.EditProfile.Id);
                    //TryUpdateModel(prifle);
                    if (profile != null)
                    {
                        profile.FirstName = model.EditProfile.FirstName;
                        profile.LastName = model.EditProfile.LastName;
                        profile.MiddleName = model.EditProfile.MiddleName;
                        profile.Gender = model.EditProfile.Gender;
                        profile.ContactNo = model.EditProfile.ContactNo;
                        profile.Address = model.EditProfile.Address;
                        profile.CompanyId = model.EditProfile.CompanyId;
                        profile.CompanyName = model.EditProfile.CompanyName;
                        profile.MaritalStatus = model.EditProfile.MaritalStatus;
                    }


                    var account = _context.Users.SingleOrDefault(a => a.UserProfileId == model.EditProfile.Id);
                    if (account != null)
                    {
                        account.Email = model.EditAccount.Email;
                        account.UserName = model.EditAccount.UserName;
                    }
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    //return Json(new { success = false, responseText = "Error @" + ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors }, JsonRequestBehavior.AllowGet);
                    
                }
                string fullName = model.EditProfile.FirstName + " " + model.EditProfile.LastName;
                ReportsController.AddAuditTrail(UserAction.Update,
                    "<strong>" + fullName + "</strong>'s information has been updated.",
                    User.Identity.GetUserId(), Page.Users);
                return Json(new { success = true, responseText = fullName + " <small>has been Updated.</small>" }, JsonRequestBehavior.AllowGet);
            }

            //return RedirectToAction("Index", "UserProfile");
        }

        public ActionResult Delete(string id)
        {
            var account = _context.Users
                .Include(a => a.UserProfile)
                .SingleOrDefault(p => p.Id == id);

            if (account == null)
                return HttpNotFound();

            //var profile = _context.UserProfiles.SingleOrDefault(p=> p.Id == account.UserProfileId);
            //if (profile == null)
            //    return HttpNotFound();

            string fullName = account.UserProfile.FirstName + " " + account.UserProfile.LastName;

            _context.Users.Remove(account);
            _context.SaveChanges();

            ReportsController.AddAuditTrail(UserAction.Delete,
                "<strong>" + fullName + "</strong> has been Deleted.",
                User.Identity.GetUserId(), Page.Users);

            return Json(new { success = true, responseText = "<small>User </small>" + fullName + "<small> has been removed.</small>" }, JsonRequestBehavior.AllowGet);
        }
    }
}