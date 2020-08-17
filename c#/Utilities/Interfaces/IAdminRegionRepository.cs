using System.Collections.Generic;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IAdminRegionRepository: IGenericRepository<Region,AdminRegionDto>
    {
        IEnumerable<AdminRegionDto> GetAllRegions();
    }
}
