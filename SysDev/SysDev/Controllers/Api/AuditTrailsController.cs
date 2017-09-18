using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Web.Http;
using AutoMapper;
using SysDev.Dtos;
using SysDev.Models;

namespace SysDev.Controllers.Api
{
    public class AuditTrailsController : ApiController
    {
        private ApplicationDbContext _context;

        public AuditTrailsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/audittrails
        public IEnumerable<AuditTrail> GetAuditTrails()
        {

            /*
             * return _context.AuditTrails
                .Include(c => c.UserProfile)
                .Include(c => c.MasterDetails)
                .ToList().OrderByDescending(c => c.DateCreated)
                .Select(Mapper.Map<AuditTrail, AuditTrailDto>); */

            return _context.AuditTrails
                .Include(c => c.UserProfile)
                .Include(c => c.Module)
                .OrderByDescending(c => c.DateCreated)
                .ToList();
        }

        // GET /api/audittrail/1
        public IHttpActionResult GetAuditTrail(int id)
        {
            var audit = _context.AuditTrails.SingleOrDefault(a => a.Id == id);

            if (audit == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            //return Mapper.Map<AuditTrail, AuditTrailDto>(audit);
            return Ok(Mapper.Map<AuditTrail, AuditTrailDto>(audit));
        }

        // POST /api/auditrails
        [HttpPost]
        public IHttpActionResult CreateAuditTrail(AuditTrailDto auditTrailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var auditTrail = Mapper.Map<AuditTrailDto, AuditTrail>(auditTrailDto);
            _context.AuditTrails.Add(auditTrail);
            _context.SaveChanges();

            auditTrailDto.Id = auditTrail.Id;

            return Created(new Uri(Request.RequestUri + "/" + auditTrail.Id), auditTrailDto);
        }

        // PUT /api/audittrails/1
        [HttpPut]
        public void UpdateAuditTrail(int id, AuditTrailDto auditTrailDto)
        {
            if(!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var dbAudit = _context.AuditTrails.SingleOrDefault(a => a.Id == id);

            if (dbAudit == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Mapper.Map(auditTrailDto, dbAudit);
            _context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteAuditTrail(int id)
        {
            var dbAudit = _context.AuditTrails.SingleOrDefault(a => a.Id == id);
            if (dbAudit == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.AuditTrails.Remove(dbAudit);
            _context.SaveChanges();
        }
    }
}
