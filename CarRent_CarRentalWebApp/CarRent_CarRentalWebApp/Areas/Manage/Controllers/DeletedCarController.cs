using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DeletedCarController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public DeletedCarController(CarRentDbContext carRentDbContext, IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Cars.Include(x=>x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == true).AsQueryable();
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

        _carRentDbContext.Cars.Remove(car);
        _carRentDbContext.SaveChanges();
        return Ok();
    }
}
