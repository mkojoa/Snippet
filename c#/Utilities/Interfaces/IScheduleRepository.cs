using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IScheduleRepository : IGenericRepository<Schedule, ScheduleDto>
    {
        IEnumerable<ScheduleDto> GetAdminSchedule();
        IEnumerable<ScheduleDto> GetSchedules(Expression<Func<Schedule, bool>> predicate);
        ScheduleDto GetSchedule(int id);
    }
}