using System;
using System.Collections.Generic;
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

namespace eticketing_mvc.Controllers.API
{
    [Route("api/buses/{action}")]
    public class BusesController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly IUnitOfWork _uow;
        private const string EntityName = "Bus";

        public BusesController()
        {
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IHttpActionResult GetActiveBusesWithDriver()
        {

            var buses = _uow.Bus.GetAdminBuses()?.Where(s => s.DriverId > 0 && s.Active).ToList() ?? new List<BusDto>();
            return Ok(buses);
        }
        [HttpGet]
        public IHttpActionResult GetActiveBusesWithoutSchedule()
        {

            var buses = _uow.Bus.GetAdminBuses()?.Where(s => s.Scheduled == false && s.Active).ToList() ?? new List<BusDto>();
            return Ok(buses);
        }
        [HttpGet]
        public IHttpActionResult GetAllBuses()
        {
            var buses = _uow.Bus.GetAdminBuses();
            return Ok(buses?.ToList() ?? new List<BusDto>());
        }

        [HttpPost]
        public IHttpActionResult CreateAdminBus([FromBody]NewBusDto bus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Messages.ProcessingError);
            }

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    bus.DriverId = bus.DriverId <= 0 ? null : bus.DriverId;
                    var destination = Mapper.Map<NewBusDto, Bus>(bus);
                    destination.CreatedAt = DateTime.Now;
                    destination.BusRegistrationNo = destination.BusRegistrationNo.ToUpper();
                    destination.AppUserId = User.Identity.GetUserId();
                    _uow.Bus.Add(destination);
                    _uow.Complete();
                    return Ok(Messages.EntityCreationSuccess(EntityName));
                }
                else
                {
                    return BadRequest(Messages.AuthenticationRequired);
                }
            }
            catch (Exception)
            {
                return BadRequest(Messages.EntityCreationError(EntityName));
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateAdminBus(int busId, UpdateBusDto bus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Messages.ProcessingError);
            }

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var busIndDb = _context.Busses.Find(busId);
                    bus.DriverId = bus.DriverId < 0 ? null : bus.DriverId;
                    var driver = _context.Drivers.FirstOrDefault(d => d.DriverId == bus.DriverId && d.AssignedBus == false);

                    if (busIndDb == null) return NotFound();
                    Mapper.Map(bus, busIndDb);
                    busIndDb.CreatedAt = DateTime.Now;
                    busIndDb.AppUserId = User.Identity.GetUserId();
                    if (driver != null) driver.AssignedBus = true;
                    _uow.Complete();
                    return Ok(Messages.EntityUpdationSuccess(EntityName));
                }
                catch (Exception)
                {
                    return BadRequest(Messages.EntityCreationError(EntityName));
                }
            }
            else
            {
                return BadRequest(Messages.AuthenticationRequired);
            }
        }

        [HttpGet]
        public IHttpActionResult BusCodeExists(string buscode)
        {
            var rgx = new Regex(@"[a-zA-Z0-9\s\-]+[a-zA-Z0-9]");
            if (buscode.Length < 3 || !rgx.IsMatch(buscode))
            {
                var error = new RecordExists
                {
                    Exists = true,
                    Message =
                        "Bus No. must follow Ghana's license plate registration format. <span style='font-weight: bold;'>eg. GW 233-17</span>"
                };
                return BadRequest(error.Message);
            }

            buscode = buscode.ToUpper();
            var bus = _uow.Bus.GetByExpression(r => r.BusRegistrationNo.ToUpper() == buscode);
            var exist = new RecordExists();
            if (bus == null)
            {
                exist.Exists = false;
            }
            else
            {
                exist.Exists = true;
                exist.Message = $"A Bus with {buscode} already exists in the system.";
            }
            return Ok(exist);
        }

        [HttpDelete]
        public IHttpActionResult DeleteBus(int busId)
        {
            if (busId <= 0) return BadRequest(Messages.IdError);
            try
            {
                var city = _context.Cities.Find(busId);
                if (city == null) return NotFound();
                _context.Cities.Remove(city);
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
