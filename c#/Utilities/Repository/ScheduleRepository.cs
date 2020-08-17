using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class ScheduleRepository : GenericRepository<Schedule, ScheduleDto>, IScheduleRepository
    {
        public ScheduleRepository(DbContext context) : base (context)
        {
        }

        public IEnumerable<ScheduleDto> GetAdminSchedule()
        {
            var getRecords = Context.Set<Schedule>().Include(b => b.Bus).Include(s => s.Route).Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<Schedule, ScheduleDto>);
            var routes = Context.Set<Route>().Include(s => s.CitySource).Include(s => s.CityDestination).ToList();
            var dtos = getRecords.ToList();
            foreach (var scheduleDto in dtos)
            {
                scheduleDto.RouteSource =
                    routes.FirstOrDefault(d => d.RouteId == scheduleDto.RouteId)?.CitySource.CityName;
                scheduleDto.RouteDestination = routes.FirstOrDefault(d => d.RouteId == scheduleDto.RouteId)?.CityDestination.CityName;
            }

            return !dtos.Any() ? null : dtos;
        }

        public IEnumerable<ScheduleDto> GetSchedules(Expression<Func<Schedule, bool>> predicate)
        {
            var getRecords = Context.Set<Schedule>().Include(s => s.Route).Where(predicate).ToList()
                .Select(Mapper.Map<Schedule, ScheduleDto>);
            var allRecords = getRecords as ScheduleDto[] ?? getRecords.ToArray();
            return !allRecords.Any() ? null : allRecords;
        }
        public ScheduleDto GetSchedule(int id)
        {
            var getRecord = Context.Set<Schedule>().Include(b => b.Bus).Include(r => r.Route).FirstOrDefault(r => r.ScheduleId == id);
            return getRecord == null ? null : Mapper.Map<Schedule, ScheduleDto>(getRecord);
        }
    }
}