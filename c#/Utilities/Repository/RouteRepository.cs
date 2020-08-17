using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class RouteRepository: GenericRepository<Route, RouteDto>, IRouteRepository
    {
        public RouteRepository(DbContext context) : base(context) 
        {
        }

        public IEnumerable<RouteDto> GetAllAdminRoute()
        {
            var getRecords = Context.Set<Route>().Include(s => s.CitySource).Include(d => d.CityDestination).Include(e => e.AppUser).ToList()
                .Select(Mapper.Map<Route, RouteDto>);
            var dtos = getRecords.ToList();
            return !dtos.Any() ? null : dtos;
        }

        public RouteDto GetRoute(int id)
        {
            var getRecord = Context.Set<Route>().Include(b => b.CityDestination).Include(r => r.CitySource).FirstOrDefault(r => r.RouteId == id);
            return getRecord == null ? null : Mapper.Map<Route, RouteDto>(getRecord);
        }
    }
}