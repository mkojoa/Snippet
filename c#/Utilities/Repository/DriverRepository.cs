using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class DriverRepository : GenericRepository<Driver,DriverDto>, IDriverRepository
    {
        public DriverRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<DriverDto> GetAllDrivers()
        {
            var getRecords = Context.Set<Driver>().Include(s => s.Status).Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<Driver, DriverDto>);
            var driverDtos = getRecords.ToList();
            return !driverDtos.Any() ? null : driverDtos;
        }
    }
}