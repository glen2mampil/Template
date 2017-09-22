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
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SysDev.App_Start;

namespace SysDev.Controllers
{
    
    public class UserProfileController : Controller
    {
        private ApplicationDbContext _context;
        public static List<Permission> CurrentUserPermission { get; set; }
        public static ApplicationUser CurrentUser { get; set; }


        public UserProfileController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            CurrentUser = LoginUser(User.Identity.GetUserId());
            CurrentUserPermission = GetUserPermission(CurrentUser);
        }


        public static ApplicationUser LoginUser(string id)
        {
            var context = new ApplicationDbContext();
            CurrentUser = context.Users.Include(u => u.Role).Include(u => u.UserProfile).FirstOrDefault(u => u.Id == id);
            return CurrentUser;
        }


        public static List<Permission> GetUserPermission(ApplicationUser user)
        {
            var context = new ApplicationDbContext();
            CurrentUserPermission = context.Permissions.Include(p => p.Module).Include(p => p.Role).Where(p => p.Role.Name == user.Role.Name).ToList();
            return CurrentUserPermission;
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            List<UserProfile> users = _context.UserProfiles.ToList();
            List<IdentityRole> roles = _context.Roles.ToList();
            var accounts = _context.Users.Include(a => a.UserProfile).ToList();


            var currentUser = LoginUser(User.Identity.GetUserId());
            var currentPermission = GetUserPermission(currentUser);
            List<MasterDetail> userRoles = _context.MasterDetails.Include(m => m.MasterData).Where(m => m.MasterData.Name == Module.Roles).ToList();

            

            var useraccount = new UserProfileViewModel
            {
                UserProfiles = users,
                Accounts = accounts,
                Account = currentUser,
                Roles = roles,
                Permission = CurrentUserPermission,
                UserRole = userRoles
                
            };
            
            if(CurrentUser.Role.Name.Equals(RoleName.SuperAdmin, StringComparison.OrdinalIgnoreCase))
                return View("IndexReadOnly", useraccount);
           

            return View("NotAllowed", useraccount);
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
                User.Identity.GetUserId(), Module.Users);

            return Json(new { success = true, responseText = "Password successfuly Change!" }, JsonRequestBehavior.AllowGet);
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string newPassword, string oldP)
        {
            
            string id = User.Identity.GetUserId();

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), oldP, newPassword);

            if (result.Succeeded)
            {
                var account = _context.Users.Include(u => u.UserProfile).FirstOrDefault(p => p.Id == User.Identity.GetUserId());
                string fullName = account.UserProfile.FirstName + " " + account.UserProfile.LastName;
                ReportsController.AddAuditTrail(UserAction.Update,
                    "<strong>" + fullName + "</strong> reset his/her password ",
                    User.Identity.GetUserId(), Module.Users);

                return Json(new { success = true, responseText = "Password successfuly Change!" }, JsonRequestBehavior.AllowGet);
            }
            AddErrors(result);
            return Json(new { success = false, feild = "currentpw", responseText = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        private string errorMessage = "";
        private void AddErrors(IdentityResult result)
        {
            
            foreach (var error in result.Errors)
            {
                errorMessage += error + " ";
            }
           
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
                    User.Identity.GetUserId(), Module.Users);
                return Json(new { success = true, responseText = user + " has set to " + account.Status }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Nothings happen, Try again later." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(UserProfileViewModel model)
        {
        
            if (model.EditProfile.Id == 0 && model.EditAccount.Id == null)
            {
                model.EditProfile.DateCreated = DateTime.Now.ToString("MMM-dd-yyyy hh:mm tt");
                _context.UserProfiles.Add(model.EditProfile);
                _context.SaveChanges();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                string role = Request.Form["UserRoles"].ToString();

                var user = new ApplicationUser
                {
                    UserName = model.EditAccount.UserName,
                    UserProfileId = model.EditProfile.Id,
                    UserProfile = model.EditProfile,
                    Email = model.EditAccount.Email,
                    Status = "Active",
                    //RoleId  = 2067
                    RoleId = Int32.Parse(role)
                };

                var chkUser = userManager.Create(user, "password1");
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Employee");

                }
                string fullName = model.EditProfile.FirstName + " " + model.EditProfile.LastName;
                ReportsController.AddAuditTrail(UserAction.Create,
                    "<strong>" +fullName + "</strong> has been added." ,
                    User.Identity.GetUserId(), Module.Users);
                return Json(new { success = true, responseText = fullName + " <small>was added.</small>", data = fullName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    var profile = _context.UserProfiles.SingleOrDefault(p => p.Id == model.EditProfile.Id);
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
                    User.Identity.GetUserId(), Module.Users);
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

            var profile = _context.UserProfiles.SingleOrDefault(p=> p.Id == account.UserProfileId);
            if (profile == null)
                return HttpNotFound();

            string fullName = account.UserProfile.FirstName + " " + account.UserProfile.LastName;

            _context.UserProfiles.Remove(profile);
            _context.SaveChanges();

            ReportsController.AddAuditTrail(UserAction.Delete,
                "<strong>" + fullName + "</strong> has been Deleted.",
                User.Identity.GetUserId(), Module.Users);

            return Json(new { success = true, responseText = "<small>User </small>" + fullName + "<small> has been removed.</small>" }, JsonRequestBehavior.AllowGet);
        }
    }
}