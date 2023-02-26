using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Drawing2D;
using System.Security.Cryptography.X509Certificates;

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
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).Where(x => x.isDeleted == false).Where(x => x.isRent == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 5, page);

        return View(paginatedList);
    }
    public IActionResult RenatlIndex(int page = 1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Include(x => x.Category).Where(x => x.isDeleted == false).Where(x=>x.isRent==true).OrderByDescending(x => x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 5, page);

        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x=>x.CarPeculiarities).Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        foreach (CarPeculiarity carPeculiarity in car.CarPeculiarities)
        {
            carPeculiarity.Peculiarity = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == carPeculiarity.PeculiarityId);
        }
        if (car == null) return View("Error-404");

        return View(car);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        ViewBag.Categories = _carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList();
        ViewBag.Pecularities = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).ToList();

        return View();
    }
    [HttpPost]
    public IActionResult Create(Car car)
    {
        List<Brand> brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        List<Category> categories = _carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList();
        ViewBag.Pecularities = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).ToList();

        ViewBag.Brands = brands;
        ViewBag.Categories = categories;
        if (!ModelState.IsValid) return View(car);
        if(brands.Count==0)
        {
            ModelState.AddModelError("BrandId", "Brand is required");
            return View(car);
        }
        if (categories.Count == 0)
        {
            ModelState.AddModelError("CategoryId", "Category is required");
            return View(car);
        }
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

        //Pecularites-----------------------------
        if(car.PecularitiesIds is not null)
        {
            foreach (int pecularityId in car.PecularitiesIds)
            {
                CarPeculiarity carPeculiarity = new CarPeculiarity
                {
                    CarId = car.Id,
                    Car = car,
                    PeculiarityId = pecularityId
                };
                _carRentDbContext.CarPeculiarities.Add(carPeculiarity);
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
        ViewBag.Categories = _carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList();
        ViewBag.Pecularities = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).ToList();
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).Include(x=>x.CarPeculiarities).FirstOrDefault(x => x.Id == id);
        
        if (car == null) return View("Error-404");

        return View(car);
    }
    [HttpPost]
    public IActionResult Update(Car newCar)
    {
        List<CarPeculiarity> carPeculiarities = new List<CarPeculiarity>();

        ViewBag.Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList();
        ViewBag.Categories = _carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList();
        ViewBag.Pecularities = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).ToList();
        Car existCar = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).FirstOrDefault(x => x.Id == newCar.Id);
        if (existCar == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existCar);
        //Poster Image-------------------------------------------------------
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

        //Pecularites-----------------------------
        if (newCar.PecularitiesIds is not null)
        {
            foreach (int pecularityId in newCar.PecularitiesIds)
            {
                CarPeculiarity carPeculiarity = new CarPeculiarity
                {
                    CarId = existCar.Id,
                    Car = existCar,
                    PeculiarityId = pecularityId
                };
                List<CarPeculiarity> deletedcarPeculiarities= _carRentDbContext.CarPeculiarities.Where(x => x.CarId == existCar.Id).ToList();
                _carRentDbContext.CarPeculiarities.RemoveRange(deletedcarPeculiarities);

                _carRentDbContext.CarPeculiarities.Add(carPeculiarity);
            }
        }
        else
        {
            List<CarPeculiarity> deletedcarPeculiarities = _carRentDbContext.CarPeculiarities.Where(x => x.CarId == existCar.Id).ToList();
            _carRentDbContext.CarPeculiarities.RemoveRange(deletedcarPeculiarities);

        }

        existCar.BrandId = newCar.BrandId;
        existCar.CategoryId = newCar.CategoryId;
        existCar.Model = newCar.Model;
        existCar.PricePerWeek = newCar.PricePerWeek;
        existCar.PricePerDay = newCar.PricePerDay;
        existCar.PricePerMonth = newCar.PricePerMonth;
        existCar.Millage = newCar.Millage;
        existCar.ConsumptionPer100KM = newCar.ConsumptionPer100KM;
        existCar.Transmission = newCar.Transmission;
        existCar.Seats = newCar.Seats;
        existCar.Luggage = newCar.Luggage;
        existCar.Fuel = newCar.Fuel;
        existCar.isNew = newCar.isNew;
        existCar.isFeatured = newCar.isFeatured;
        existCar.Description = newCar.Description;
        existCar.UpdatedDate =DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete-----------------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
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
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).Where(x => x.isDeleted == true).AsQueryable();
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
        return RedirectToAction(nameof(DeletedIndex));
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
