using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class CarController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public CarController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<Car> cars = _carRentDbContext.Cars.Include(x=>x.Brand).Include(x=>x.CarImages).Where(x=>x.isDeleted==false).ToList();
        return View(cars);
    }

    public IActionResult Detail(int id)
    {
        ViewBag.NewCars = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages)
                                                .Where(x => x.isDeleted == false).Where(x => x.isNew == true)
                                                .ToList();
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x=>x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return View("Error");
        return View(car);
    }
}