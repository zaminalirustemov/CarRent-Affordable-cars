using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class BrandController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public BrandController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    //Read---------------------------------------------------------------------------------
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Brands
                                     .Where(x=>x.isDeleted==false)
                                     .AsQueryable();

        var paginatedList = PaginatedList<Brand>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult Detail(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return NotFound();

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

        _carRentDbContext.Brands.Add(brand);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return NotFound();

        return View(brand);
    }
    [HttpPost]
    public IActionResult Update(Brand newBrand)
    {
        Brand existBrand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == newBrand.Id);
        if (existBrand == null) return NotFound();
        if (!ModelState.IsValid) return View(newBrand);

        existBrand.Name = newBrand.Name;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return NotFound();

        brand.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
