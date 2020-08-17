using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities;
using eticketing_mvc.Utilities.Interfaces;
using eticketing_mvc.Utilities.UoW;
using Microsoft.AspNet.Identity;

namespace eticketing_mvc.Controllers.API
{
    [Route("api/cities/{action}")]
    public class CitiesController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly IUnitOfWork _uow;
        private const string EntityName = "City";

        public CitiesController()
        {
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IHttpActionResult GetAllCities()
        {
            var cities = _uow.City.GetAll();
            var cityDtos = cities?.Where(e => e.Status).ToList() ?? new List<CityDto>();
            if (!cityDtos.Any()) return Ok(cityDtos);
            var getCities = cityDtos.ToList()
                .Select(Mapper.Map<CityDto, CityBrief>);
            return Ok(getCities.OrderBy(c => c.CityName));
        }

        [HttpGet]
        public IHttpActionResult CityCodeExists(string cityCode)
        {
            var rgx = new Regex(@"[a-zA-Z0-9]{6}");
            if (cityCode.Length > 6 || !rgx.IsMatch(cityCode))
            {
                var error = new RecordExists
                {
                    Exists = true,
                    Message =
                        "City Code must contain six (6) alphanumeric characters. <span style='font-weight: bold;'>eg. acc001</span>"
                };
                return BadRequest(error.Message);
            }
            var city = _uow.City.GetByExpression(r => r.CityCode == cityCode);
            var exist = new RecordExists { Exists = city != null };
            return Ok(exist);
        }

        [HttpGet]
        public IHttpActionResult GetAdminCities()
        {
            var cities = _uow.AdminCity.GetAllCities();
            var cityList = cities?.ToList() ?? new List<AdminCityDto>();
            return Ok(cityList);
        }

        [HttpPost]
        public IHttpActionResult CreateAdminCity(CityDto city)
        {
            if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);
            if (!User.Identity.IsAuthenticated) return BadRequest(Messages.AuthenticationRequired);
            try
            {
                var destination = Mapper.Map<CityDto, City>(city);
                destination.CreatedAt = DateTime.Now;
                destination.CityCode = destination.CityCode.ToUpper();
                destination.CityName = destination.CityName.ToUpper();
                destination.AppUserId = User.Identity.GetUserId();
                
                _uow.City.Add(destination);
                _uow.Complete();
                city.CityId = destination.CityId;
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

            return Created(new Uri($"{Request.RequestUri}/{city.CityId}"), Messages.EntityCreationSuccess(EntityName));

        }

        [HttpPut]
        public IHttpActionResult UpdateAdminCity(int cityId, AdminUpdateCityDto city)
        {
            if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);
            var cityInDb = _context.Cities.FirstOrDefault(r => r.CityId == cityId);
            if (cityInDb == null) return NotFound();
            if (!User.Identity.IsAuthenticated) return BadRequest(Messages.AuthenticationRequired);
            try
            {
                Mapper.Map(city, cityInDb);
                cityInDb.Status = city.Status;
                cityInDb.CityName = city.CityName.ToUpper();
                cityInDb.CreatedAt = DateTime.Now;
                cityInDb.AppUserId = User.Identity.GetUserId();
                _uow.Complete();
                return Ok(Messages.EntityUpdationSuccess(EntityName));
            }
            catch (Exception)
            {
                return BadRequest(Messages.ProcessingError);
            }
        }


        [HttpDelete]
        public IHttpActionResult DeleteCity(int cityId)
        {
            if (cityId <= 0) return BadRequest(Messages.IdError);
            try
            {
                var city = _context.Cities.Find(cityId);
                if (city == null) return NotFound();
                _context.Cities.Remove(city);
                _uow.Complete();
            }
            catch (Exception)
            {

                return Ok(Messages.EntityDeletionError(EntityName));
            }
            return Ok(Messages.EntityDeletionSuccess(EntityName));
        }
    }
}
