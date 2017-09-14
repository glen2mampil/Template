using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity;
using SysDev.Models;

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
            

            //return Mapper.Map<AuditTrail, AuditTrailDto>(audit);
            return Ok(user);
        }

        // POST /api/auditrails
        [HttpPost]
        public IHttpActionResult CreateUser(ApplicationUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var auditTrail = Mapper.Map<AuditTrailDto, AuditTrail>(auditTrailDto);

            //_context.Users.Add(user);
            //_context.SaveChanges();

            //auditTrailDto.Id = auditTrail.Id;

            //return Created(new Uri(Request.RequestUri + "/" + auditTrail.Id), auditTrailDto);
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
