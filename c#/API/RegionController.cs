using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities;
using eticketing_mvc.Utilities.Interfaces;
using eticketing_mvc.Utilities.UoW;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace eticketing_mvc.Controllers.API
{
    [Route("api/regions/{action}")]
    public class RegionController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly IUnitOfWork _uow;
        private const string EntityName = "Region";

        public RegionController()
        {
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IHttpActionResult GetAllRegions()
        {
           var regions = _uow.Region.GetAll();
            return Ok(regions?.Where(r => r.Status).OrderBy(o => o.RegionName).ToList() ?? new List<RegionDto>());
        }

        [HttpGet]
        public IHttpActionResult RegionCodeExists(string regionCode)
        {
            var rgx = new Regex(@"[a-zA-Z]{3}[0-9]{2}[1-9]{1}");
            if (!rgx.IsMatch(regionCode))
            {
                var error = new RecordExists
                {
                    Exists = true,
                    Message =
                        "Region Code must contain 3 alphabets and 3 numbers.<br/>Numbers must start from one(1). <span style='font-weight: bold;'>eg. reg001</span>"
                };
                return BadRequest(error.Message);
            }
            var region = _uow.Region.GetByExpression(r => r.RegionCode == regionCode);
            var exist = new RecordExists {Exists = region != null};
            return Ok(exist);
        }

        [HttpGet]
        public IHttpActionResult GetAdminRegions()
        {
           var regions = _uow.AdminRegion.GetAllRegions();
            var reionList = regions?.ToList() ?? new List<AdminRegionDto>();
            return Ok(reionList);
        }

        [HttpPost]
        public IHttpActionResult CreateAdminRegion(RegionDto region)
        {
            if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var destination = Mapper.Map<RegionDto, Region>(region);
                    destination.CreatedAt = DateTime.Now;
                    destination.RegionCode = destination.RegionCode.ToUpper();
                    destination.RegionName = destination.RegionName.ToUpper();
                    destination.AppUserId = User.Identity.GetUserId();
                    _uow.Region.Add(destination);
                    _uow.Complete();
                    region.RegionId = destination.RegionId;
                }
                else
                {
                    return BadRequest(Messages.AuthenticationRequired);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(Messages.EntityCreationError(EntityName));
            }

            return Ok(Messages.EntityCreationSuccess(EntityName));
        }

        [HttpPut]
        public IHttpActionResult UpdateAdminRegion(int regionId, AdminUpdateRegionDto region)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var regionInDb = _context.Regions.FirstOrDefault(r => r.RegionId == regionId);
                    if (regionInDb == null) return NotFound();
                    Mapper.Map(region, regionInDb);
                    regionInDb.RegionName = region.RegionName.ToUpper();
                    regionInDb.Status = region.Status;
                    regionInDb.CreatedAt = DateTime.Now;
                    regionInDb.AppUserId = User.Identity.GetUserId();
                    _uow.Complete();
                    return Ok(Messages.EntityUpdationSuccess(EntityName));
                }
                else
                {
                    return BadRequest(Messages.AuthenticationRequired);
                }
            }
            catch (Exception)
            {

                return BadRequest(Messages.EntityUpdationError(EntityName));
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteRegion(int regionId)
        {
            if (regionId <= 0) return BadRequest(Messages.IdError);
            try
            {
                var region = _context.Regions.Find(regionId);
                if (region == null) return NotFound();
                _context.Regions.Remove(region);
                _uow.Complete();
            }
            catch (Exception)
            {

                return Ok(Messages.EntityDeletionError(EntityName));
            }
            return Ok(Messages.EntityDeletionSuccess(EntityName));
        }
    }
}
