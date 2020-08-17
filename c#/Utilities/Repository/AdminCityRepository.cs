using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class AdminCityRepository : GenericRepository<City, AdminCityDto>, IAdminCityRepository
    {
        public AdminCityRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<AdminCityDto> GetAllCities()
        {
            var getRecords = Context.Set<City>().Include(r => r.Region).Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<City, AdminCityDto>);
            var allRecords = getRecords as AdminCityDto[] ?? getRecords.ToArray();
            
            return !allRecords.Any() ? null : allRecords;
        }
    }
}