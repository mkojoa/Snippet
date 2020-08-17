using System.Collections.Generic;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IAdminCityRepository : IGenericRepository<City, AdminCityDto>
    {
        IEnumerable<AdminCityDto> GetAllCities();
    }
}
