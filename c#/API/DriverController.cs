using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities;
using eticketing_mvc.Utilities.Interfaces;
using eticketing_mvc.Utilities.UoW;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace eticketing_mvc.Controllers.API
{
    [Route("api/drivers/{action}")]
    public class DriverController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly IUnitOfWork _uow;
        private const string EntityName = "Driver";

        public DriverController()
        {
            _uow = new UnitOfWork(_context);
        }

        [HttpPost]
        public IHttpActionResult CreateAdminDriver([FromBody] NewDriverDto driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Messages.ProcessingError);
            }

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var destination = Mapper.Map<NewDriverDto, Driver>(driver);
                    destination.CreatedAt = DateTime.Now;
                    destination.LicenseClassType = destination.LicenseClassType.ToUpper();
                    destination.LicenseNo = destination.LicenseNo.ToUpper();
                    destination.NhisCardNo = destination.NhisCardNo.ToUpper();
                    destination.AppUserId = User.Identity.GetUserId();
                    _uow.Driver.Add(destination);
                    _uow.Complete();
                    driver.DriverId = destination.DriverId;
                }
                else
                {
                    return BadRequest(Messages.AuthenticationRequired);
                }
            }
            catch (Exception)
            {
                return BadRequest(Messages.EntityCreationError(EntityName));
            }
            return Created(new Uri($"{Request.RequestUri}/{driver.DriverId}"), Messages.EntityCreationSuccess(EntityName));
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateAdminDriverWithImage()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/App_Data/DriverImages")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/App_Data/DriverImages"));
            }
            var root = HttpContext.Current.Server.MapPath("~/App_Data/DriverImages");
            var provider = new MultipartFormDataStreamProvider(root);
            var driver = new NewDriverDto();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // Get the Data Part
                var formData = provider.FormData.GetValues("driverDetail");
                MultipartFileData imageFile = null;
                // This illustrates how to get the file names.
                if (provider.FileData.Count > 0)
                {
                     imageFile = provider.FileData[0];
                }
                var driverData = formData?.SingleOrDefault();
                if (driverData != null)
                {
                     driver = JsonConvert.DeserializeObject<NewDriverDto>(driverData);
                    if (!ModelState.IsValid)
                    {
                        if (imageFile != null)
                        {
                            File.Delete(Path.Combine(root, imageFile.LocalFileName));
                        }
                        return BadRequest(Messages.ProcessingError);
                    }
                    try
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            var destination = Mapper.Map<NewDriverDto, Driver>(driver);
                            destination.CreatedAt = DateTime.Now;
                            destination.LicenseClassType = destination.LicenseClassType.ToUpper();
                            destination.LicenseNo = destination.LicenseNo.ToUpper();
                            destination.NhisCardNo = destination.NhisCardNo.ToUpper();
                            destination.AppUserId = User.Identity.GetUserId();
                            _uow.Driver.Add(destination);
                            _uow.Complete();
                            driver.DriverId = destination.DriverId;
                        }
                        else
                        {
                            if (imageFile != null)
                            {
                                File.Delete(Path.Combine(root, imageFile.LocalFileName));
                            }
                            return BadRequest(Messages.AuthenticationRequired);
                        }
                    }
                    catch (Exception)
                    {
                        if (imageFile != null)
                        {
                            File.Delete(Path.Combine(root, imageFile.LocalFileName));
                        }

                        if (driver.DriverId > 0)
                        {
                            var driverInDb = _context.Drivers.Find(driver.DriverId);
                            if (driverInDb != null)
                            {
                                _context.Drivers.Remove(driverInDb);
                                _context.SaveChanges();
                            }
                        }
                    }
                    if (imageFile != null)
                    {
                        var name = imageFile.Headers.ContentDisposition.FileName.Replace("\"", "");
                        var file = Guid.NewGuid();
                        var filename = $"{file}{Path.GetExtension(name)}";

                        if(File.Exists(Path.Combine(root, filename))){
                            file = Guid.NewGuid();
                        }
                        File.Move(imageFile.LocalFileName, Path.Combine(root, filename));
                        var dImage = new DriverImage();
                        dImage.DriverId = driver.DriverId;
                        dImage.FileName = filename;
                        dImage.CreatedAt = DateTime.Now;
                        dImage.AppUserId = User.Identity.GetUserId();
                        _context.DriverImages.Add(dImage);

                    }

                    _uow.Complete();
                    return Created(new Uri($"{Request.RequestUri}/1"), Messages.EntityCreationSuccess(EntityName));
                }

                return BadRequest(Messages.EntityCreationError(EntityName));
            }
            catch (System.Exception ex)
            {
                if (driver.DriverId > 0)
                {
                    var driverInDb = _context.Drivers.Find(driver.DriverId);
                    if (driverInDb != null)
                    {
                        _context.Drivers.Remove(driverInDb);
                        _context.SaveChanges();
                    }
                }
                return BadRequest(Messages.ProcessingError);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateDriver(int driverId, UpdateDriverDto driver)
        {
            if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);
            var driverInDb = _context.Drivers.Find(driverId);
            if (driverInDb == null) return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    Mapper.Map(driver, driverInDb);
                    driverInDb.CreatedAt = DateTime.Now;
                    driverInDb.AppUserId = User.Identity.GetUserId();
                    _uow.Complete();
                    return Ok(Messages.EntityUpdationSuccess(EntityName));
                }
                catch (Exception)
                {
                    return BadRequest(Messages.ProcessingError);
                }
            }
            else
            {
                return BadRequest(Messages.AuthenticationRequired);
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateDriverWithImage(int driverId)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/App_Data/DriverImages")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/App_Data/DriverImages"));
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data/DriverImages");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                // Get the Data Part
                var formData = provider.FormData.GetValues("driverDetail");
                // This illustrates how to get the file names.
                MultipartFileData imageFile = null;
                if (provider.FileData.Count > 0)
                {
                    imageFile = provider.FileData[0];
                }
                var driverData = formData?.SingleOrDefault();
                var driverImage = _context.DriverImages.Find(driverId);

                if (driverData != null)
                {
                    var driver = JsonConvert.DeserializeObject<UpdateDriverDto>(driverData);
                    if (!ModelState.IsValid) return BadRequest(Messages.ProcessingError);
                    var driverInDb = _context.Drivers.Find(driverId);
                    if (driverInDb == null) return NotFound();
                    try
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            driver.LicenseClassType = driver.LicenseClassType.ToUpper();
                            Mapper.Map(driver, driverInDb);
                            driverInDb.CreatedAt = DateTime.Now;
                            driverInDb.AppUserId = User.Identity.GetUserId();
                            _uow.Complete();

                        }
                        else
                        {
                            if (imageFile != null)
                            {
                                File.Delete(Path.Combine(root, imageFile.LocalFileName));
                            }

                            return BadRequest(Messages.AuthenticationRequired);
                        }
                    }
                    catch (Exception)
                    {
                        if (imageFile != null)
                        {
                            File.Delete(Path.Combine(root, imageFile.LocalFileName));
                        }
                    }

                    if (imageFile == null) return Ok(Messages.EntityUpdationSuccess(EntityName));
                    var name = imageFile.Headers.ContentDisposition.FileName.Replace("\"", "");
                    var file = Guid.NewGuid();
                    var filename = $"{file}{Path.GetExtension(name)}";
                    //if (File.Exists(Path.Combine(root, filename)))
                    //{
                    //    File.Delete(Path.Combine(root, filename));
                    //}
                    File.Move(imageFile.LocalFileName, Path.Combine(root, filename));
                    if (driverImage == null)
                    {
                        var dImage = new DriverImage();
                        dImage.DriverId = driverId;
                        dImage.FileName = filename;
                        dImage.CreatedAt = DateTime.Now;
                        dImage.AppUserId = User.Identity.GetUserId();
                        _context.DriverImages.Add(dImage);
                        _context.SaveChanges();
                    }
                    else
                    {
                        driverImage.FileName = filename;
                        _context.SaveChanges();
                    }
                    return Ok(Messages.EntityUpdationSuccess(EntityName));
                }

                return BadRequest(Messages.EntityCreationError(EntityName));

            }
            catch (System.Exception ex)
            {
                return BadRequest(Messages.ProcessingError);
            }

        }

        [HttpGet]
        public IHttpActionResult GetActiveDrivers()
        {
            var drivers = _uow.Driver.GetAllDrivers();
            return Ok(drivers?.Where(s => s.StatusId == _uow.Status.GetByExpression(a => a.StatusName == "Active").StatusId) ?? new List<DriverDto>());
        }
        [HttpGet]
        public IHttpActionResult GetUnassignedDrivers()
        {
            var drivers = _uow.Driver.GetAllDrivers()?.Where(s => s.AssignedBus == false).ToList() ?? new List<DriverDto>();
            return Ok(drivers?.Where(s => s.StatusId == _uow.Status.GetByExpression(a => a.StatusName == "Active").StatusId));
        }

        [HttpGet]
        public IHttpActionResult GetAllDrivers()
        {
            var drivers = _uow.Driver.GetAllDrivers();
            var driverDto = drivers?.ToList() ?? new List<DriverDto>();
            foreach (var driver in driverDto)
            {
                driver.Age = GetAge(driver.BirthDate);
            }
            return Ok(driverDto);
        }

        private static int GetAge(string birthDate)
        {
            var zTime = new DateTime(1, 1, 1);
            var bDate = DateTime.Parse(birthDate);
            var today = DateTime.Today;
            var time = today.Subtract(bDate);
            var age = (zTime + time).Year - 1;
            return age;
        }

        [HttpGet]
        public HttpResponseMessage GetDriverImage(string driverId)
        {
            var id = int.Parse(driverId.Split('?')[0]);
            //Image img = GetImage(imageName, width, height);
            var getFileName = _context.DriverImages.Find(id)?.FileName;
            var imageFile = HttpContext.Current.Server.MapPath($"~/App_Data/DriverImages/{getFileName}");
            if (!File.Exists(imageFile))
            {
                imageFile = HttpContext.Current.Server.MapPath("~/images/noimage.jpg");
                if (!File.Exists(imageFile)) return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var img = Image.FromFile(imageFile);
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                return result;
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteDriver(int driverId)
        {
            var driverInDb = _context.Drivers.Find(driverId);
            if (driverInDb == null) return NotFound();
            var driverImage = _context.DriverImages.Find(driverInDb.DriverId);
            if (driverImage != null)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath($"~/App_Data/DriverImages/{driverImage.FileName}")))
                {
                    try
                    {
                        File.Delete(
                            HttpContext.Current.Server.MapPath($"~/App_Data/DriverImages/{driverImage.FileName}"));
                    }
                    catch (Exception)
                    {
                    }
                }
                _context.DriverImages.Remove(driverImage);
            }

            var bus = _context.Busses.FirstOrDefault(b => b.DriverId == driverId);
            if (bus != null)
            {
                bus.DriverId = null;
            }

            _context.Drivers.Remove(driverInDb);
            _context.SaveChanges();

            return Ok(Messages.EntityDeletionSuccess(EntityName));
        }
    }
}
