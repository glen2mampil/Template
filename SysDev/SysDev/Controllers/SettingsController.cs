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
    public class SettingsController : Controller
    {
        private ApplicationDbContext _context;

        public SettingsController()
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
            return account;
        }

        protected Permission LoginUserPermission()
        {
            var role = User.IsInRole("SuperAdmin") ? "SuperAdmin" : "Employee";
            var userPermission = _context.Permissions.SingleOrDefault(m => m.IdentityRole.Name == role && m.MasterDetail.Name == "Users");
            return userPermission;
        }

        // GET: Settings
        public ActionResult Index()
        {
            var datas = _context.MasterDatas.ToList();
            var details = _context.MasterDetails.ToList();

            var viewModel = new SettingsViewModel
            {
                MasterDatas = datas,
                MasterDetails = details,
                Account = LoginUser(),
                Permission = LoginUserPermission()
            };

            return View(viewModel);
        }

        public ActionResult ViewMasterData()
        {
            var datas = _context.MasterDatas.ToList();
            return View(datas);
        }

        public ActionResult ViewMasterDetails(int id)
        {
            var data = _context.MasterDatas.SingleOrDefault(d => d.Id == id);
            var detials = _context.MasterDetails.Where(d => d.MasterDataId == id).ToList();

            var viewModel = new MasterDetailsViewModel
            {
                MasterData = data,
                MasterDetails = detials
            };

            return View(viewModel);
        }

        public ActionResult Edit(int id, string from)
        {
            if (from.Equals("data"))
            {
                var masterData = _context.MasterDatas.SingleOrDefault(p => p.Id == id);
                //var masterDetails = _context.MasterDetails.SingleOrDefault(p => p.MasterDataId == masterData.Id);
                if (masterData == null)
                    return HttpNotFound();

                ViewBag.ModalTitle = "Edit MasterData";
                return View("Create",masterData);
            }

            if (from.Equals("detail"))
            {
                
                var masterDetails = _context.MasterDetails.Include(md => md.MasterData).SingleOrDefault(p => p.Id == id);
                if (masterDetails == null)
                    return HttpNotFound();

                var viewModel = new NewMasterDetailsViewModel
                {
                    MasterData = masterDetails.MasterData,
                    MasterDetail = masterDetails
                };


                ViewBag.ModalTitle = "Edit MasterDetails";
                return View("CreateDetail",viewModel);
            }

            return View();
        }

        public ActionResult UpdateStatus(int id, string actionName)
        {
            if (actionName.Equals("data"))
            {
                var masterData = _context.MasterDatas.SingleOrDefault(d => d.Id == id);
                if (masterData != null)
                {
                    string status = masterData.Status == "Active" ? "Inactive" : "Active";
                    masterData.Status = status;
                    _context.SaveChanges();

                    string description = "[Master Data] <strong>" + masterData.Name + "</strong> was set to " + status;
                    ReportsController.AddAuditTrail(UserAction.Update, description, User.Identity.GetUserId(), Page.Settings);
                    return Json(new { success = true, responseText = description }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (actionName.Equals("detail"))
            {
                var masterDetail = _context.MasterDetails.SingleOrDefault(d => d.Id == id);
                if (masterDetail != null)
                {
                    string status = masterDetail.Status == "Active" ? "Inactive" : "Active";
                    masterDetail.Status = status;
                    _context.SaveChanges();

                    string description = "[Master Detail] <strong>" + masterDetail.Name + "</strong> was set to " + status;
                    ReportsController.AddAuditTrail(UserAction.Update, description, User.Identity.GetUserId(), Page.Settings);
                    return Json(new { success = true, responseText = description }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index", "Settings");
        }

        public ActionResult Delete(int id, string from)
        {
            if (from.Equals("data"))
            {
                var masterData = _context.MasterDatas.SingleOrDefault(p => p.Id == id);

                if (masterData == null)
                    return HttpNotFound();

                _context.MasterDatas.Remove(masterData);
                _context.SaveChanges();

                string description = "[Master Data] <strong>" + masterData.Name + "</strong> was deleted ";
                ReportsController.AddAuditTrail(UserAction.Delete, description, User.Identity.GetUserId(), Page.Settings);
                return Json(new { success = true, responseText = description }, JsonRequestBehavior.AllowGet);

            }
            if(from.Equals("detail"))
            {
                var masterDetail = _context.MasterDetails.SingleOrDefault(p => p.Id == id);
                if (masterDetail == null)
                    return HttpNotFound();

                _context.MasterDetails.Remove(masterDetail);
                _context.SaveChanges();

                string description = "[Master Detail] <strong>" + masterDetail.Name + "</strong> was deleted";
                ReportsController.AddAuditTrail(UserAction.Delete, description, User.Identity.GetUserId(), Page.Settings);
                return Json(new { success = true, responseText = description }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Settings");
        }

        public ActionResult Create()
        {
            ViewBag.ModalTitle = "Create MasterData";
            return View();
        }

        public ActionResult CreateDetail(int id)
        {
            var masterData = _context.MasterDatas.SingleOrDefault(d => d.Id == id);
            ViewBag.ModalTitle = "Create MasterDetails";

            var viewModel = new NewMasterDetailsViewModel
            {
                MasterData = masterData
            };

           
            return View(viewModel);
        }

        public ActionResult SaveMasterData(MasterData model)
        {
            var duplicate = _context.MasterDatas.FirstOrDefault(m => m.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
           
            if (duplicate != null)
            {
                return Json(new { success = false, responseText = "Master Data " + model.Name + " already exist" }, JsonRequestBehavior.AllowGet);
            }

            string description = "";
            if (model.Id == 0)
            {
                model.DateTimeCreated = DateTime.Now;
                model.DateTimeUpdated = DateTime.Now;
                model.Status = "Active";
                _context.MasterDatas.Add(model);
                //_context.SaveChanges();

                description = "[Master Data] <strong>" + model.Name + "</strong> has been added";
                ReportsController.AddAuditTrail(UserAction.Create, description, User.Identity.GetUserId(), Page.Settings);
            }
            else
            {
                var mData = _context.MasterDatas.SingleOrDefault(d => d.Id == model.Id);
                if (mData != null)
                {
                    mData.Name = model.Name;

                    mData.Description = model.Description;
                    mData.DateTimeUpdated = DateTime.Now;
                    
                }
                description = "[Master Data] <strong>" + model.Name + "</strong>'s information has been updated.";
                ReportsController.AddAuditTrail(UserAction.Update, description, User.Identity.GetUserId(), Page.Settings);
            }
            _context.SaveChanges();

            return Json(new { success = true, responseText = description }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("Index", "Settings");
        }

        public ActionResult SaveMasterDetail(NewMasterDetailsViewModel model)
        {
            var duplicate = _context.MasterDetails.FirstOrDefault(m => m.Name.Equals(model.MasterDetail.Name, StringComparison.OrdinalIgnoreCase));

            if (duplicate != null)
            {
                return Json(new { success = false, responseText = "Master Data " + model.MasterDetail.Name + " already exist" }, JsonRequestBehavior.AllowGet);
            }

            if (model.MasterDetail.Id == 0)
            {
                var masterData = _context.MasterDatas.SingleOrDefault(m => m.Id == model.MasterData.Id);
                var masterDetail = new MasterDetail
                {
                    MasterData = masterData,
                    MasterDataId = masterData.Id,
                    Name = model.MasterDetail.Name,
                    Description = model.MasterDetail.Description,
                    DateTimeCreated = DateTime.Now,
                    DateTimeUpdated = DateTime.Now,
                    Status = "Active",
                    Value = model.MasterDetail.Name
                };
                _context.MasterDetails.Add(masterDetail);
            }
            else
            {
                var mData = _context.MasterDetails.SingleOrDefault(d => d.Id == model.MasterDetail.Id);
                if (mData != null)
                {
                    mData.Name = model.MasterDetail.Name;

                    mData.Description = model.MasterDetail.Description;
                    mData.DateTimeUpdated = DateTime.Now;
                }

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Settings");
        }
    }
}