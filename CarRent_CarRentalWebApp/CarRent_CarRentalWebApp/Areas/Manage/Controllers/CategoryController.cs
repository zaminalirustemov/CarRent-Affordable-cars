using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class CategoryController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public CategoryController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    //Read---------------------------------------------------------------------------------
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Categories
                                     .Where(x => x.isDeleted == false)
                                     .AsQueryable();

        var paginatedList = PaginatedList<Category>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (!ModelState.IsValid) return View(category);

        category.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Categories.Add(category);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Category category = _carRentDbContext.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null) return View("Error-404");

        return View(category);
    }
    [HttpPost]
    public IActionResult Update(Category newCategory)
    {
        Category existCategory = _carRentDbContext.Categories.FirstOrDefault(x => x.Id == newCategory.Id);
        if (existCategory == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newCategory);

        existCategory.Name = newCategory.Name;
        existCategory.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Category category = _carRentDbContext.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null) return BadRequest();

        category.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Categories
                                      .Where(x => x.isDeleted == true)
                                      .AsQueryable();

        var paginatedList = PaginatedList<Category>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Category category = _carRentDbContext.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null) return View("Error-404");

        category.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Category category = _carRentDbContext.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null) return BadRequest();

        _carRentDbContext.Categories.Remove(category);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Category> categories = _carRentDbContext.Categories.Where(x => x.isDeleted == true).ToList();
        if (categories.Count == 0) return BadRequest();

        _carRentDbContext.Categories.RemoveRange(categories);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}