using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            return _context.AuditTrails.ToList();
        }

        // GET /api/audittrail/1
        public AuditTrail GetAuditTrail(int id)
        {
            var audit = _context.AuditTrails.SingleOrDefault(a => a.Id == id);

            if (audit == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return audit;
        }

        // POST /api/auditrails
        [HttpPost]
        public AuditTrail CreateAuditTrail(AuditTrail audit)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.AuditTrails.Add(audit);
            _context.SaveChanges();

            return audit;
        }

        // PUT /api/audittrails/1
        [HttpPut]
        public void UpdateAuditTrail(int id, AuditTrail audit)
        {
            if(!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var dbAudit = _context.AuditTrails.SingleOrDefault(a => a.Id == id);

            if (dbAudit == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            dbAudit.Description = audit.Description;
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
