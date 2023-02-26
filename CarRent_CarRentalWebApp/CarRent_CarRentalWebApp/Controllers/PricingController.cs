using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class PricingController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public PricingController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Cars.Include(x => x.CarImages).Include(x => x.Brand).Include(x => x.Category).Include(x => x.CarComments).Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Car>.Create(query, 10, page);

        return View(paginatedList);
    }
}