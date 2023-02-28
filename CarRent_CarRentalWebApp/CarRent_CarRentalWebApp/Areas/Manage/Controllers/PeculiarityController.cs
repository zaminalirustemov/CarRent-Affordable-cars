using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class PeculiarityController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public PeculiarityController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).AsQueryable();
        var paginatedList = PaginatedList<Peculiarity>.Create(query, 7, page);
        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Peculiarity peculiarity = _carRentDbContext.Peculiarities.FirstOrDefault(x => x.Id == id);
        if (peculiarity == null) return View("Error-404");

        return View(peculiarity);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Peculiarity peculiarity)
    {
        if (!ModelState.IsValid) return View(peculiarity);

        peculiarity.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Peculiarities.Add(peculiarity);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Peculiarity peculiarity = _carRentDbContext.Peculiarities.Where(x=>x.isDeleted==false).FirstOrDefault(x => x.Id == id);
        if (peculiarity == null) return View("Error-404");

        return View(peculiarity);
    }
    [HttpPost]
    public IActionResult Update(Peculiarity newPeculiarity)
    {
        Peculiarity existPeculiarity = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == newPeculiarity.Id);
        if (existPeculiarity == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newPeculiarity);

        existPeculiarity.Name = newPeculiarity.Name;
        existPeculiarity.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Peculiarity peculiarity = _carRentDbContext.Peculiarities.FirstOrDefault(x => x.Id == id);
        if (peculiarity == null) return View("Error-404");

        List<CarPeculiarity> carPeculiarities = _carRentDbContext.CarPeculiarities.Where(x => x.PeculiarityId == peculiarity.Id).ToList();
        _carRentDbContext.CarPeculiarities.RemoveRange(carPeculiarities);
        peculiarity.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Peculiarity>.Create(query, 7, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Peculiarity peculiarity = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == true).FirstOrDefault(x => x.Id == id);
        if (peculiarity == null) return View("Error-404");


        peculiarity.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Peculiarity peculiarity = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == true).FirstOrDefault(x => x.Id == id);
        if (peculiarity == null) return View("Error-404");

        _carRentDbContext.Peculiarities.Remove(peculiarity);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Peculiarity> peculiarities = _carRentDbContext.Peculiarities.Where(x => x.isDeleted == true).ToList();
        if (peculiarities.Count == 0) return BadRequest();
        
        _carRentDbContext.Peculiarities.RemoveRange(peculiarities);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}