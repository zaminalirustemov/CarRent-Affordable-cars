using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DeletedBrandController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public DeletedBrandController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Brands
                                              .Where(x=>x.isDeleted==true)
                                              .AsQueryable();

        var paginatedList = PaginatedList<Brand>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore--------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        brand.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return BadRequest();

        _carRentDbContext.Brands.Remove(brand);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}
