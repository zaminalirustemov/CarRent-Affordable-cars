using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class PricingController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public PricingController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<Car> cars = _carRentDbContext.Cars.Include(x => x.CarImages).Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarComments).Where(x => x.isDeleted == false).ToList();
        return View(cars);
    }
}