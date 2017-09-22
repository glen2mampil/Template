using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using SysDev.Dtos;
using SysDev.Models;
using System.Data.Entity;

namespace SysDev.Controllers.Api
{
    public class MasterDetailsController : ApiController
    {
        private ApplicationDbContext _context;

        public MasterDetailsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/masterdatas
        public IEnumerable<MasterDetail> GetMasterDetails()
        {
            return _context.MasterDetails.Include(m => m.MasterData).OrderBy(m => m.Name).ToList();
        }

        // GET /api/audittrail/1
        public IEnumerable<MasterDetail> GetMasterDetail(int id)
        {
            var mData = _context.MasterDetails.Include(m => m.MasterData).Where(m => m.MasterDataId == id);

            if (mData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            
            return mData;
        }

        // POST /api/auditrails
        [HttpPost]
        public IHttpActionResult CreateAuditTrail(MasterDataDto masterDataDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var masterdata = Mapper.Map<MasterDataDto, MasterData>(masterDataDto);
            _context.MasterDatas.Add(masterdata);
            _context.SaveChanges();

            masterDataDto.Id = masterdata.Id;

            return Created(new Uri(Request.RequestUri + "/" + masterdata.Id), masterDataDto);
        }

        // PUT /api/audittrails/1
        [HttpPut]
        public IHttpActionResult UpdateMasterData(int id, MasterDataDto masterDataDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var masterData = _context.AuditTrails.SingleOrDefault(a => a.Id == id);

            if (masterData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Mapper.Map(masterDataDto, masterData);
            _context.SaveChanges();

            return Ok("Information has been updated!");
        }

        [HttpDelete]
        public IHttpActionResult DeleteMasterDetails(int id)
        {
            var masterDetail = _context.MasterDetails.SingleOrDefault(a => a.Id == id);
            if (masterDetail == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.MasterDetails.Remove(masterDetail);
            _context.SaveChanges();

            return Ok( masterDetail.Name + " has been remove.");
        }
    }
}
