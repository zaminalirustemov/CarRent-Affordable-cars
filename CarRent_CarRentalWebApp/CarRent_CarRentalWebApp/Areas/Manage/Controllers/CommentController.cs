using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class CommentController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public CommentController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x=>x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).OrderByDescending(x=>x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult ActiveIndex(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult PassiveIndex(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == false).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");
        return View(comment);
    }
}