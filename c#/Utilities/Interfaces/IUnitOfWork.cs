using System;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICityRepository City { get; }
        IRegionRepository Region { get; }
        IFaresRepository Fare { get; }
        IBusRepository Bus { get; }
        IAdminCityRepository AdminCity { get; }
        IAdminRegionRepository AdminRegion { get; }
        IDriverRepository Driver { get; }
        IGenericRepository<Status,StatusDto> Status { get; }
        IRouteRepository Route { get; }
        IScheduleRepository Schedule { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        int Complete();
    }
}
