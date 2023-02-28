using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class ServiceController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public ServiceController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Services.Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Service>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Service service = _carRentDbContext.Services.FirstOrDefault(x => x.Id == id);
        if (service == null) return View("Error-404");

        return View(service);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Service service)
    {
        if (!ModelState.IsValid) return View(service);

        service.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Services.Add(service);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Service service = _carRentDbContext.Services.FirstOrDefault(x => x.Id == id);
        if (service == null) return View("Error-404");

        return View(service);
    }
    [HttpPost]
    public IActionResult Update(Service newService)
    {
        Service existService = _carRentDbContext.Services.FirstOrDefault(x => x.Id == newService.Id);
        if (existService == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newService);

        existService.IconKeyword = newService.IconKeyword;
        existService.Title = newService.Title;
        existService.Description = newService.Description;
        existService.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Service service = _carRentDbContext.Services.FirstOrDefault(x => x.Id == id);
        if (service == null) return View("Error-404");

        service.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Services.Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Service>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Service service = _carRentDbContext.Services.FirstOrDefault(x => x.Id == id);
        if (service == null) return View("Error-404");

        service.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Service service = _carRentDbContext.Services.FirstOrDefault(x => x.Id == id);
        if (service == null) return View("Error-404");

        _carRentDbContext.Services.Remove(service);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Service> services = _carRentDbContext.Services.Where(x => x.isDeleted == true).ToList();
        if (services.Count == 0) return BadRequest();
        
        _carRentDbContext.Services.RemoveRange(services);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}
