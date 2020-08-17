using System;
using System.Web;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to Dto
            CreateMap<City, CityDto>();
            CreateMap<CityDto, CityBrief>();
            CreateMap<City, AdminCityDto>()
                .ForMember(c => c.RegionName, opt => opt.MapFrom(s => s.Region.RegionName))
                .ForMember(r => r.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")));
            CreateMap<Driver, DriverDto>()
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")))
                .ForMember(d => d.AppUserName, opt => opt.MapFrom(s => s.AppUser.FirstName + " " + s.AppUser.LastName))
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => s.BirthDate.ToString("dd-MMM-yyyy")))
                .ForMember(d => d.LicenseExpiryDate, opt => opt.MapFrom(s => s.LicenseExpiryDate.ToString("dd-MMM-yyyy")))
                .ForMember(d => d.LicenseIssueDate, opt => opt.MapFrom(s => s.LicenseIssueDate.ToString("dd-MMM-yyyy")))
                .ForMember(d => d.NhisCardExpiryDate, opt => opt.MapFrom(s => s.NhisCardExpiryDate.ToString("dd-MMM-yyyy")))
                .ForMember(d => d.NhisCardValidityDate, opt => opt.MapFrom(s => s.NhisCardValidityDate.ToString("dd-MMM-yyyy")))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Male ? "Male" : "Female"))
                .ForMember(d => d.Age, opt => opt.Ignore())
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.StatusName));

            CreateMap<Region, RegionDto>();
            CreateMap<Region, AdminRegionDto>()
                .ForMember(r => r.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")))
                .ForMember(r => r.AppUserName, opt => opt.MapFrom(s => s.AppUser.FirstName + " " +s.AppUser.LastName));

            CreateMap<Status, StatusDto>()
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")));
            CreateMap<Bus, BusDto>()
                .ForMember(b => b.AppUserName, opt => opt.MapFrom(s => s.AppUser.FirstName + " " + s.AppUser.LastName))
                .ForMember(b => b.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")))
                .ForMember(b => b.DriverName, opt => opt.MapFrom(s => s.Driver.FirstName + " " + s.Driver.LastName));

            CreateMap<Route, RouteDto>()
                .ForMember(r => r.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToString("dd-MMM-yyyy HH:mm")))
                .ForMember(r => r.AppUserName, opt => opt.MapFrom(s => s.AppUser.FirstName + " " + s.AppUser.LastName))
                .ForMember(r => r.CitySourceName, opt => opt.MapFrom(s => s.CitySource.CityName))
                .ForMember(r => r.Fare, opt => opt.MapFrom(s => s.Fare.ToString("F")))
                .ForMember(r => r.CityDestinationName, opt => opt.MapFrom(s => s.CityDestination.CityName));
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(s => s.RouteSource, opt => opt.Ignore())
                .ForMember(s => s.RouteDestination, opt => opt.Ignore())
                .ForMember(s => s.BusNumber, opt => opt.MapFrom(s => s.Bus.BusRegistrationNo))
                .ForMember(b => b.AppUserName, opt => opt.MapFrom(s => s.AppUser.FirstName + " " + s.AppUser.LastName));




            //Dto to Domain
            CreateMap<CityDto, City>()
                .ForMember(c => c.CityId, opt => opt.Ignore())
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.AppUser, opt => opt.Ignore())
                .ForMember(c => c.Region, opt => opt.Ignore())
                .ForMember(c => c.AppUserId, opt => opt.Ignore());
            CreateMap<RegionDto, Region>()
                .ForMember(r => r.AppUser, opt => opt.Ignore())
                .ForMember(r => r.CreatedAt, opt => opt.Ignore())
                .ForMember(r => r.AppUserId, opt => opt.Ignore());
            CreateMap<AdminUpdateRegionDto, Region>()
                .ForMember(r => r.AppUser, opt => opt.Ignore())
                .ForMember(r => r.RegionId, opt => opt.Ignore())
                .ForMember(r => r.RegionCode, opt => opt.Ignore())
                .ForMember(r => r.AppUserId, opt => opt.Ignore())
                .ForMember(r => r.CreatedAt, opt => opt.Ignore());
            CreateMap<AdminUpdateCityDto, City>()
                .ForMember(r => r.AppUser, opt => opt.Ignore())
                .ForMember(r => r.CityId, opt => opt.Ignore())
                .ForMember(r => r.CityCode, opt => opt.Ignore())
                .ForMember(r => r.CreatedAt, opt => opt.Ignore())
                .ForMember(r => r.Region, opt => opt.Ignore())
                .ForMember(c => c.AppUserId, opt => opt.Ignore());
            CreateMap<NewDriverDto, Driver>()
                .ForMember(d => d.DriverId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.AppUser, opt => opt.Ignore())
                .ForMember(d => d.AppUserId, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.AssignedBus, opt => opt.Ignore())
                .ForMember(d => d.DriverImage, opt => opt.Ignore());
            CreateMap<UpdateDriverDto, Driver>()
                .ForMember(d => d.AppUser, opt => opt.Ignore())
                .ForMember(d => d.DriverId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.AppUserId, opt => opt.Ignore())
                .ForMember(d => d.AssignedBus, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.DriverImage, opt => opt.Ignore());
            CreateMap<NewStatusDto, Status>()
                .ForMember(s => s.StatusId, opt => opt.Ignore())
                .ForMember(s => s.AppUser, opt => opt.Ignore())
                .ForMember(s => s.AppUserId, opt => opt.Ignore())
                .ForMember(s => s.CreatedAt, opt => opt.Ignore());
            CreateMap<NewBusDto, Bus>()
                .ForMember(b => b.CreatedAt, opt => opt.Ignore())
                .ForMember(b => b.AppUser, opt => opt.Ignore())
                .ForMember(b => b.AppUserId, opt => opt.Ignore())
                .ForMember(b => b.BusId, opt => opt.Ignore())
                .ForMember(b => b.Scheduled, opt => opt.Ignore())
                .ForMember(b => b.Driver, opt => opt.Ignore());
            CreateMap<UpdateBusDto, Bus>()
                .ForMember(b => b.AppUser, opt => opt.Ignore())
                .ForMember(b => b.Driver, opt => opt.Ignore())
                .ForMember(b => b.BusRegistrationNo, opt => opt.Ignore())
                .ForMember(b => b.CreatedAt, opt => opt.Ignore())
                .ForMember(b => b.BusId, opt => opt.Ignore())
                .ForMember(b => b.Scheduled, opt => opt.Ignore())
                .ForMember(b => b.AppUserId, opt => opt.Ignore());
            CreateMap<NewRouteDto, Route>()
                .ForMember(r => r.CreatedAt, opt => opt.Ignore())
                .ForMember(r => r.AppUser, opt => opt.Ignore())
                .ForMember(r => r.AppUserId, opt => opt.Ignore())
                .ForMember(r => r.CitySource, opt => opt.Ignore())
                .ForMember(r => r.CityDestination, opt => opt.Ignore())
                .ForMember(r => r.RouteId, opt => opt.Ignore());
            CreateMap<UpdateRouteDto, Route>()
                .ForMember(r => r.CreatedAt, opt => opt.Ignore())
                .ForMember(r => r.RouteCode, opt => opt.Ignore())
                .ForMember(r => r.AppUser, opt => opt.Ignore())
                .ForMember(r => r.AppUserId, opt => opt.Ignore())
                .ForMember(r => r.CitySource, opt => opt.Ignore())
                .ForMember(r => r.CityDestination, opt => opt.Ignore())
                .ForMember(r => r.RouteId, opt => opt.Ignore());
            CreateMap<NewScheduleDto, Schedule>()
                .ForMember(s => s.AppUser, opt => opt.Ignore())
                .ForMember(s => s.CreatedAt, opt => opt.Ignore())
                .ForMember(s => s.Route, opt => opt.Ignore())
                .ForMember(s => s.AppUserId, opt => opt.Ignore())
                .ForMember(s => s.Bus, opt => opt.Ignore())
                .ForMember(s => s.ArrivalTime, opt => opt.Ignore())
                .ForMember(s => s.ScheduleId, opt => opt.Ignore());
        }
    }
}