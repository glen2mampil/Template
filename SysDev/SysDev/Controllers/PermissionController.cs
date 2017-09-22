﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SysDev.Models;

namespace SysDev.Controllers
{
   
    public class PermissionController : Controller
    {
        private ApplicationDbContext _context;

        public PermissionController()
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

        // GET: Permission
        public ActionResult Index()
        {
            var roles = _context.Roles.ToList();
            var permissions = _context.Permissions.ToList();
            var masterDetails = _context.MasterDetails.ToList();

            var viewModel = new UserPermissionViewModel
            {
                Permissions = permissions,
                Roles = roles,
                MasterDetails = masterDetails,
                Account = LoginUser(),
                Permission = LoginUserPermission()
            };

            return View(viewModel);
        }

        public ActionResult Details(string id)
        {
            var role = _context.Roles.SingleOrDefault(p => p.Id == id);

            if (role == null)
                return HttpNotFound();

            var permissions = _context.Permissions.Where(p => p.IdentityRoleId == role.Id);
            var masterDetails = _context.MasterDetails.ToList();

            var viewModel = new DetailedPermissionViewModel
            {
                Permissions = permissions,
                Role = role,
                MasterDetails = masterDetails
            };

            return View(viewModel);
        }

        public ActionResult UpdateStatus(int? id, string actionName)
        {
            var permission = _context.Permissions.SingleOrDefault(a => a.Id == id);
            string oldVal = "";
            string newVal = "";

            if (permission != null)
            {
                switch (actionName)
                {
                    case "View":
                        oldVal = permission.AllowView == 1 ? "Allowed" : "Not Allowed";
                        newVal = permission.AllowView == 1 ? "Not Allowed" : "Allowed";
                        permission.AllowView = permission.AllowView == 1 ? 0 : 1;
                        break;
                    case "Create":
                        oldVal = permission.AllowCreate == 1 ? "Allowed" : "Not Allowed";
                        newVal = permission.AllowCreate == 1 ? "Not Allowed" : "Allowed";
                        permission.AllowCreate = permission.AllowCreate == 1 ? 0 : 1;
                        break;
                    case "Edit":
                        oldVal = permission.AllowEdit == 1 ? "Allowed" : "Not Allowed";
                        newVal = permission.AllowEdit == 1 ? "Not Allowed" : "Allowed";
                        permission.AllowEdit = permission.AllowEdit == 1 ? 0 : 1;
                        break;
                    case "Delete":
                        oldVal = permission.AllowDelete == 1 ? "Allowed" : "Not Allowed";
                        newVal = permission.AllowDelete == 1 ? "Not Allowed" : "Allowed";
                        permission.AllowDelete = permission.AllowDelete == 1 ? 0 : 1;
                        break;
                    case "GenerateReport":
                        oldVal = permission.AllowGenerateReport == 1 ? "Allowed" : "Not Allowed";
                        newVal = permission.AllowGenerateReport == 1 ? "Not Allowed" : "Allowed";
                        permission.AllowGenerateReport = permission.AllowGenerateReport == 1 ? 0 : 1;
                        break;
                }

                
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Permission");
        }
    }
}