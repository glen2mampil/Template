using System;
using System.Collections.Generic;
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
        // GET: Settings
        public ActionResult Index()
        {
            var datas = _context.MasterDatas.ToList();
            var details = _context.MasterDetails.ToList();

            var viewModel = new SettingsViewModel
            {
                MasterDatas = datas,
                MasterDetails = details
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
                
                var masterDetails = _context.MasterDetails.SingleOrDefault(p => p.Id == id);
                var masterData = _context.MasterDatas.SingleOrDefault(md => md.Id == masterDetails.MasterDataId);
                //var masterDetails = _context.MasterDetails.SingleOrDefault(p => p.MasterDataId == masterData.Id);
                if (masterDetails == null)
                    return HttpNotFound();

                var viewModel = new NewMasterDetailsViewModel
                {
                    MasterData = masterData,
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
                    masterData.Status = masterData.Status == "Active" ? "Inactive" : "Active";
                    ReportsController.AddAuditTrail("Update",
                        "MasterData [ " + masterData.Name + "] was set to " + masterData.Status,
                        User.Identity.GetUserId());
                    _context.SaveChanges();
                }
            }
            else if (actionName.Equals("detail"))
            {
                var masterDetail = _context.MasterDetails.SingleOrDefault(d => d.Id == id);
                if (masterDetail != null)
                {
                    masterDetail.Status = masterDetail.Status == "Active" ? "Inactive" : "Active";
                    ReportsController.AddAuditTrail("Update",
                        "MasterDetail [ " + masterDetail.Name + "] was set to " + masterDetail.Status,
                        User.Identity.GetUserId());
                    _context.SaveChanges();
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

                ReportsController.AddAuditTrail("Delete",
                    "MasterData [" + masterData.Name + "] was Deleted",
                    User.Identity.GetUserId());

            }
            if(from.Equals("detail"))
            {
                var masterDetail = _context.MasterDetails.SingleOrDefault(p => p.Id == id);
                if (masterDetail == null)
                    return HttpNotFound();

                _context.MasterDetails.Remove(masterDetail);
                _context.SaveChanges();
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
            if (model.Id == 0)
            {
                model.DateTimeCreated = DateTime.Now;
                model.DateTimeUpdated = DateTime.Now;
                model.Status = "Active";
                _context.MasterDatas.Add(model);
                _context.SaveChanges();
                ReportsController.AddAuditTrail("Add MasterData",
                    model.Name + " has been added.",
                    User.Identity.GetUserId());
            }
            else
            {
                var mData = _context.MasterDatas.SingleOrDefault(d => d.Id == model.Id);
                if (mData != null)
                {
                    mData.Name = model.Name;

                    mData.Description = model.Description;
                    mData.DateTimeUpdated = DateTime.Now;
                    ReportsController.AddAuditTrail("Update",
                        "MasterData [" + model.Name + "] has been added.",
                        User.Identity.GetUserId());
                }

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Settings");
        }

        public ActionResult SaveMasterDetail(NewMasterDetailsViewModel model)
        {
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