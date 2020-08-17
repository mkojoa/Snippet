using System.Collections.Generic;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IBusRepository : IGenericRepository<Bus, BusDto>
    {
        IEnumerable<BusDto> GetAdminBuses();
    }
}
