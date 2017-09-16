using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SysDev.Models;
using SysDev.Dtos;

namespace SysDev.Controllers.Api
{
    public class UsersController : ApiController
    {
        private ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers
        public IEnumerable<ApplicationUser> GetAuditTrails()
        {
            return _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Roles)
                .OrderBy(u => u.UserProfile.LastName)
                .ToList();

        }
            
        // GET /api/users/1
        public IHttpActionResult GetUser(string id)
        {
            
            var user = _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Roles)
                .Include(u => u.Logins)
                .SingleOrDefault(a => a.Id == id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var role = User.IsInRole("SuperAdmin")? "SuperAdmin" : "Employee";

            var permission = _context.Permissions
                .Include(p => p.MasterDetail)
                .Include(p => p.IdentityRole)
                .Where(p => p.IdentityRole.Name == role);

            //return Mapper.Map<AuditTrail, AuditTrailDto>(audit);
            return Ok(user);
        }

        // POST /api/auditrails
        [HttpPost]
        public IHttpActionResult CreateUser(UserProfileDto user)
        {
            
            //if (!ModelState.IsValid)
            //    return BadRequest();


            var profile = new UserProfile
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Address = user.Address,
                ContactNo = user.ContactNo,
                CompanyName = user.CompanyName,
                CompanyId = user.CompanyId,
                Gender = user.Gender,
                MaritalStatus = user.MaritalStatus,
                DateCreated = DateTime.Now.ToString("MMM-dd-yyyy hh:mm tt")
            };
            _context.UserProfiles.Add(profile);
           
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var account = new ApplicationUser
            {
                UserName = user.UserName,
                UserProfileId = profile.Id,
                UserProfile = profile,
                Email = user.Email,
                Status = "Active"
            };

            var chkUser = userManager.Create(account, "password1");
            //Add default User to Role Admin   
            if (chkUser.Succeeded)
            {
                userManager.AddToRole(account.Id, "SuperAdmin");
            }
            _context.SaveChanges();

            //ReportsController.AddAuditTrail("Add User",
            //    user.FirstName + " " + user.LastName + " has been added.",
            //    User.Identity.GetUserId());

            //return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
            return Ok(user);
            
        }

        [HttpPut]
        public void UpdateUser(int id, ApplicationUser auditTrailDto)
        {
            //if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //var dbAudit = _context.AuditTrails.SingleOrDefalt(a => a.Id == id);

            //if (dbAudit == null)
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //Mapper.Map(auditTrailDto, dbAudit);
            //_context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteUser(string id)
        {
            var user = _context.Users.SingleOrDefault(a => a.Id == id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
