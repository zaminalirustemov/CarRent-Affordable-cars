using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Drawing2D;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class CarController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public CarController(CarRentDbContext carRentDbContext, IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).Where(x => x.isRent == false).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 5, page);

        return View(paginatedList);
    }
    public IActionResult RenatlIndex(int page = 1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).Where(x=>x.isRent==true).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 5, page);

        return View(paginatedList);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        return View();
    }
    [HttpPost]
    public IActionResult Create(Car car)
    {
        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        if (!ModelState.IsValid) return View(car);
        //Poster Image--------------------------
        if (car.PosterImageFile is null)
        {
            ModelState.AddModelError("PosterImageFile", "Poster image is required");
            return View(car);
        }
        if (car.PosterImageFile.ContentType != "image/jpeg" && car.PosterImageFile.ContentType != "image/png")
        {
            ModelState.AddModelError("PosterImageFile", "The file you upload must be in jpeg or png format.");
            return View(car);
        }
        if (car.PosterImageFile.Length > 2097152)
        {
            ModelState.AddModelError("PosterImageFile", "The size of your uploaded file must be less than 2 MB.");
            return View(car);
        }
        CarImage posterimage = new CarImage
        {
            Car = car,
            ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/car", car.PosterImageFile),
            isPoster = true
        };
        _carRentDbContext.CarImages.Add(posterimage);
        //Gallery----------------------------------
        if (car.ImageFiles is not null)
        {
            foreach (IFormFile image in car.ImageFiles)
            {
                if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFiles", "The file you upload must be in jpeg or png format.");
                    return View(car);
                }
                if (image.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFiles", "The size of your uploaded file must be less than 2 MB.");
                    return View(car);
                }
                CarImage carImage = new CarImage
                {
                    Car = car,
                    ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/car", image),
                    isPoster = false
                };
                _carRentDbContext.CarImages.Add(carImage);
            }
        }

        car.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Cars.Add(car);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update----------------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        Car car = _carRentDbContext.Cars.Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return View("Error-404");

        return View(car);
    }
    [HttpPost]
    public IActionResult Update(Car newCar)
    {
        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        Car existCar = _carRentDbContext.Cars.Include(x => x.CarImages).FirstOrDefault(x => x.Id == newCar.Id);
        if (existCar == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existCar);
        //Poster Image--------------------------
        if (newCar.PosterImageFile is not null)
        {
            if (newCar.PosterImageFile.ContentType != "image/jpeg" && newCar.PosterImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("PosterImageFile", "The file you upload must be in jpeg or png format.");
                return View(newCar);
            }
            if (newCar.PosterImageFile.Length > 2097152)
            {
                ModelState.AddModelError("PosterImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newCar);
            }
            FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", existCar.CarImages.FirstOrDefault(x => x.isPoster == true).ImageName);
            _carRentDbContext.CarImages.Remove(existCar.CarImages.FirstOrDefault(x => x.isPoster == true));

            CarImage posterimage = new CarImage
            {
                Car = newCar,
                ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/car", newCar.PosterImageFile),
                isPoster = true
            };
            existCar.CarImages.Add(posterimage);
        }
        //Gallery----------------------------------
        if (newCar.CarImagesIds is not null)
        {
            foreach (var image in existCar.CarImages.Where(x => !newCar.CarImagesIds.Contains(x.Id) && x.isPoster == false))
            {
                FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", image.ImageName);
            }
            existCar.CarImages?.RemoveAll(x => !newCar.CarImagesIds.Contains(x.Id) && x.isPoster == false);
        }
        else
        {
            foreach (var image in existCar.CarImages.Where(x => x.isPoster == false))
            {
                FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", image.ImageName);
            }
            existCar.CarImages.RemoveAll(x => x.isPoster == false);
        }
        
        
        if (newCar.ImageFiles is not null)
        {
            foreach (IFormFile image in newCar.ImageFiles)
            {
                if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFiles", "The file you upload must be in jpeg or png format.");
                    return View(newCar);
                }
                if (image.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFiles", "The size of your uploaded file must be less than 2 MB.");
                    return View(newCar);
                }
                CarImage carImage = new CarImage
                {
                    CarId = newCar.Id,
                    ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/car", image),
                    isPoster = false
                };
                existCar.CarImages.Add(carImage);
            }
        }

        existCar.BrandId = newCar.BrandId;
        existCar.Model = newCar.Model;
        existCar.PricePerHour = newCar.PricePerHour;
        existCar.PricePerDay = newCar.PricePerDay;
        existCar.PricePerMonth = newCar.PricePerMonth;
        existCar.Millage = newCar.Millage;
        existCar.ConsumptionPer100KM = newCar.ConsumptionPer100KM;
        existCar.Transmission = newCar.Transmission;
        existCar.Seats = newCar.Seats;
        existCar.Luggage = newCar.Luggage;
        existCar.Fuel = newCar.Fuel;
        //---Features---
        existCar.Airconditions = newCar.Airconditions;
        existCar.ChildSeat = newCar.ChildSeat;
        existCar.GPS = newCar.GPS;
        existCar.Music = newCar.Music;
        existCar.SeatBelt = newCar.SeatBelt;
        existCar.SleepingBed = newCar.SleepingBed;
        existCar.Bluetooth = newCar.Bluetooth;
        existCar.OnboardComputer = newCar.OnboardComputer;
        existCar.LongTermTrips = newCar.LongTermTrips;
        existCar.CarKit = newCar.CarKit;
        existCar.RemoteCentralLocking = newCar.RemoteCentralLocking;
        existCar.ClimateControl = newCar.ClimateControl;
        existCar.isNew = newCar.isNew;
        existCar.isFeatured = newCar.isFeatured;
        //--------------
        existCar.Description = newCar.Description;
        existCar.UpdatedDate =DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete-----------------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return BadRequest();

        car.isDeleted=true;
        _carRentDbContext.SaveChanges();
        return Ok();
    }



    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 5, page);

        return View(paginatedList);
    }
    //Restore---------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return View("Error-404");

        car.isDeleted = false;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    //Hard Delete-----------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return BadRequest();


        foreach (CarImage carImage in car.CarImages) FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", carImage.ImageName);

        _carRentDbContext.Cars.Remove(car);
        _carRentDbContext.SaveChanges();
        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Car> cars = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == true).ToList();
        if (cars.Count == 0) return BadRequest();

        foreach (Car car in cars)
        {
            foreach (CarImage carImage in car.CarImages)
            {
                FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", carImage.ImageName);
            }
        }

        _carRentDbContext.Cars.RemoveRange(cars);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}
