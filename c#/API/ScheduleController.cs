using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities;
using eticketing_mvc.Utilities.Interfaces;
using eticketing_mvc.Utilities.UoW;
using Microsoft.AspNet.Identity;

namespace eticketing_mvc.Controllers.API
{
    [Route("api/schedules/{action}")]
    public class ScheduleController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly IUnitOfWork _uow;
        private const string EntityName = "Schedule";

        public ScheduleController()
        {
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IHttpActionResult GetActiveSchedules()
        {
            var schedules = _uow.Schedule.GetAdminSchedule();
            var dtos = schedules?.Where(s => s.Active).ToList() ?? new List<ScheduleDto>();
            return Ok(dtos);
        }

        [HttpPost]
        public IHttpActionResult CreateAdminSchedule(NewScheduleDto schedule)
        {
            if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var bus = _context.Busses.FirstOrDefault(b =>
                        b.BusId == schedule.BusId && b.Active && b.Scheduled == false);
                    if (bus == null) return BadRequest(Messages.CreateScheduleBusError);
                    var destination = Mapper.Map<NewScheduleDto, Schedule>(schedule);
                    var arrivalDate = TimeSpan.Parse(_uow.Route.GetById(destination.RouteId).Duration);
                    destination.ArrivalTime = destination.DepartureTime + arrivalDate;
                    destination.AppUserId = User.Identity.GetUserId();
                    destination.CreatedAt = DateTime.Now;
                    _uow.Schedule.Add(destination);
                    bus.Scheduled = true;
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
                return BadRequest(Messages.ProcessingError);
            }
        }
    }
}