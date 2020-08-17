using System.Collections.Generic;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IRouteRepository : IGenericRepository<Route, RouteDto>
    {
        IEnumerable<RouteDto> GetAllAdminRoute();
        RouteDto GetRoute(int id);
    }
}
