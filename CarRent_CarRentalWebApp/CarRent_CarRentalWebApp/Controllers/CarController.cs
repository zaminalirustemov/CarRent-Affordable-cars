using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarRent_CarRentalWebApp.Controllers;
public class CarController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly UserManager<AppUser> _userManager;

    public CarController(CarRentDbContext carRentDbContext, UserManager<AppUser> userManager)
    {
        _carRentDbContext = carRentDbContext;
        _userManager = userManager;
    }
    public IActionResult Index(string filter,DateTime? oldDateTime = null, DateTime? newDateTime = null,double? fromLowToHighDayPrice= 0,double? fromHighToLowDayPrice = 0,
                               double? fromLowToHighWeekPrice = 0, double? fromHighToLowWeekPrice = 0, double? fromLowToHighMonthPrice = 0, double? fromHighToLowMonthPrice = 0,
                               string? popular=null,int page=1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).Where(x => x.isDeleted == false).Where(x => x.Brand.isDeleted == false).Where(x => x.Category.isDeleted == false).AsQueryable();
        
        if (filter == "popular") query = query.OrderByDescending(x => x.CarComments.Where(x=>x.isDeleted==false).Where(x=>x.isActive==true).Count());

        if (filter== "oldDateTime") query = query.OrderBy(x => x.CreatedDate);
        
        if (filter == "newDateTime") query = query.OrderByDescending(x => x.CreatedDate);
        
        if (filter == "fromLowToHighDayPrice") query = query.OrderBy(x => x.PricePerDay);
        
        if (filter == "fromHighToLowDayPrice") query = query.OrderByDescending(x => x.PricePerDay);
        
        if (filter == "fromLowToHighWeekPrice") query = query.OrderBy(x => x.PricePerWeek);
        
        if (filter == "fromHighToLowWeekPrice") query = query.OrderByDescending(x => x.PricePerWeek);
        
        if (filter == "fromLowToHighMonthPrice") query = query.OrderBy(x => x.PricePerMonth);
        
        if (filter == "fromHighToLowMonthPrice") query = query.OrderByDescending(x => x.PricePerMonth);

        var paginatedList = PaginatedList<Car>.Create(query, 9, page);

        return View(paginatedList);
    }

    public IActionResult Detail(int id)
    {
        List<Peculiarity> peculiarities = new List<Peculiarity>();
        ViewBag.NewCars = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages)
                                                .Where(x => x.isDeleted == false).Where(x => x.isNew == true).Where(x => x.Brand.isDeleted == false).Where(x => x.Category.isDeleted == false)
                                                .ToList();
        Car car = _carRentDbContext.Cars.Include(x=>x.CarPeculiarities).Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarImages).Include(x => x.CarComments).FirstOrDefault(x => x.Id == id);
        if (car == null) return View("Error");
        CarDetailViewModel carDetailVM = new CarDetailViewModel
        {
            Car = car,
            CarComments = _carRentDbContext.CarComments.Include(x => x.AppUser).Where(x=>x.isActive==true).Where(x=>x.isDeleted==false).Where(x => x.CarId == id).ToList(),
        };

        foreach (var item in car.CarPeculiarities)
        {
            Peculiarity peculiarity = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == item.PeculiarityId);
            if (peculiarity is not null) peculiarities.Add(peculiarity);
        }
        ViewBag.Pecularities=peculiarities;

        return View(carDetailVM);
    }
    //CarComment--------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> CarComment(CarComment carComment)
    {
        Car car = _carRentDbContext.Cars.FirstOrDefault(x => x.Id == carComment.CarId);
        if (car == null) return View("Error");

        if (!ModelState.IsValid) return RedirectToAction("Detail", new { id = car.Id });
        if (carComment.Rating < 1 || carComment.Rating > 5)
        {
            ModelState.AddModelError("Rating", "Error");
            return RedirectToAction("Detail", new { id = car.Id });
        }
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

        CarComment comment = new CarComment
        {
            AppUser = appUser,
            AppUserId = appUser.Id,
            CarId = carComment.CarId,
            Car = car,
            Comment = carComment.Comment,
            Rating = carComment.Rating,
            SendedDate = DateTime.UtcNow.AddHours(4),
            isActive = null,
        };

        _carRentDbContext.CarComments.Add(comment);
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Detail), new { id = car.Id });
    }
}