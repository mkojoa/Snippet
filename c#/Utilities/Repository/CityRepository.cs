using System.Data.Entity;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class CityRepository : GenericRepository<City, CityDto>, ICityRepository
    {
        public CityRepository(DbContext context) : base(context)
        {
        }
    }
}