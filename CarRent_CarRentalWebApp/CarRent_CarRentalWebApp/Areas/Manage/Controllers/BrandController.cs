using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class BrandController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public BrandController(CarRentDbContext carRentDbContext,IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Brands.Where(x => x.isDeleted == false).OrderByDescending(x=>x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Brand>.Create(query, 7, page);
        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        return View(brand);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Brand brand)
    {
        if (!ModelState.IsValid) return View(brand);

        brand.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Brands.Add(brand);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        return View(brand);
    }
    [HttpPost]
    public IActionResult Update(Brand newBrand)
    {
        Brand existBrand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == newBrand.Id);
        if (existBrand == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newBrand);

        existBrand.Name = newBrand.Name;
        existBrand.UpdatedDate=DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return BadRequest();

        brand.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Brands.Where(x => x.isDeleted == true).AsQueryable();

        var paginatedList = PaginatedList<Brand>.Create(query, 7, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        brand.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return BadRequest();

        List<Car> cars = _carRentDbContext.Cars.Include(x=>x.CarImages).Where(x => x.BrandId == brand.Id).ToList();
        foreach (Car car in cars)
        {
            foreach (CarImage carImage in car.CarImages)
            {
                FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", carImage.ImageName);
            }
        }

        _carRentDbContext.Brands.Remove(brand);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Brand> brands = _carRentDbContext.Brands.Where(x => x.isDeleted == true).ToList();
        if (brands.Count == 0) return BadRequest();
        foreach (Brand brand in brands)
        {
            List<Car> cars = _carRentDbContext.Cars.Include(x => x.CarImages).Where(x => x.BrandId == brand.Id).ToList();
            foreach (Car car in cars)
            {
                foreach (CarImage carImage in car.CarImages)
                {
                    FileManager.DeleteFile(_environment.WebRootPath, "uploads/car", carImage.ImageName);
                }
            }
        }
        _carRentDbContext.Brands.RemoveRange(brands);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}
