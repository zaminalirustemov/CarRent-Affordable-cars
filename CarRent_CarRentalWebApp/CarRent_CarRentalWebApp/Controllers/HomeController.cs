using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class HomeController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public HomeController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        HomeViewModel homeVM = new HomeViewModel
        {
            Heroes = _carRentDbContext.Heroes.ToList(),
            FeaturedCars = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Include(x=>x.Category)
                                                 .Where(x => x.isDeleted == false).Where(x=>x.isFeatured==true)
                                                 .ToList()
        };
        return View(homeVM);
    }

}