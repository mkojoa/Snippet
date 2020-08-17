using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class BusRepository : GenericRepository<Bus, BusDto>, IBusRepository
    {
        public BusRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<BusDto> GetAdminBuses()
        {
            var getRecords = Context.Set<Bus>().Include(d => d.Driver).Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<Bus, BusDto>);
            var busDtos = getRecords.ToList();
            return !busDtos.Any() ? null : busDtos;
        }
    }
}