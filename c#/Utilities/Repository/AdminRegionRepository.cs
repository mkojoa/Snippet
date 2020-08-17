using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class AdminRegionRepository: GenericRepository<Region, AdminRegionDto>, IAdminRegionRepository
    {
        public AdminRegionRepository(DbContext context) : base(context)
        {
           
        }

        public IEnumerable<AdminRegionDto> GetAllRegions()
        {
            var getRecords = Context.Set<Region>().Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<Region, AdminRegionDto>);
            var allRecords = getRecords as AdminRegionDto[] ?? getRecords.ToArray();
            return !allRecords.Any() ? null : allRecords;
        }
    }
}