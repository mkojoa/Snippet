using System;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;
using eticketing_mvc.Utilities.Repository;

namespace eticketing_mvc.Utilities.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            City = new CityRepository(_context);
            Region = new RegionRepository(_context);
            Bus = new BusRepository(_context);
            Fare = new FaresRepository(_context);
            AdminCity = new AdminCityRepository(_context);
            AdminRegion = new AdminRegionRepository(_context);
            Driver = new DriverRepository(_context);
            Status = new GenericRepository<Status, StatusDto>(_context);
            Route = new RouteRepository(_context);
            Schedule = new ScheduleRepository(_context);
            PaymentMethod = new PaymentMethodRepository(_context);
        }

        public ICityRepository City { get; set; }
        public IRegionRepository Region { get; set; }
        public IBusRepository Bus { get; set; }
        public IFaresRepository Fare { get; set; }
        public IAdminCityRepository AdminCity { get; set; }
        public IAdminRegionRepository AdminRegion { get; set; }
        public IDriverRepository Driver { get; set; }
        public IGenericRepository<Status,StatusDto> Status { get; set; }
        public IRouteRepository Route { get; set; }
        public IScheduleRepository Schedule { get; set; }
        public IPaymentMethodRepository PaymentMethod { get; set; }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}