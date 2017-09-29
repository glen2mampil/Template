using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using SysDev.Dtos;
using SysDev.Models;

namespace SysDev.Controllers.Api
{
    public class MasterDatasController : ApiController
    {
        private ApplicationDbContext _context;

        public MasterDatasController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/masterdatas
        public IEnumerable<MasterData> GetMasterDatas()
        {
            return _context.MasterDatas.OrderBy(data => data.Name).ToList();
        }

        // GET /api/audittrail/1
        public IHttpActionResult GetMasterData(int id)
        {
            var mData = _context.MasterDatas.FirstOrDefault(m => m.Id == id);

            if (mData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

           
            return Ok(Mapper.Map<MasterData, MasterDataDto>(mData));
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
        public IHttpActionResult DeleteMasterData(int id)
        {
            var masterData = _context.MasterDatas.SingleOrDefault(a => a.Id == id);
            if (masterData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            var name = masterData.Name;
            _context.MasterDatas.Remove(masterData);
            _context.SaveChanges();

            return Ok(name + " has been remove.");
        }
    }
}
