using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class SettingsController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public SettingsController(CarRentDbContext carRentDbContext) 
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<Settings> settings = _carRentDbContext.Settings.AsNoTracking().ToList();
        return View(settings);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Settings settings = _carRentDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (settings == null) return View("Error-404");

        return View(settings);
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Settings settings = _carRentDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (settings == null) return View("Error-404");

        return View(settings);
    }
    [HttpPost]
    public IActionResult Update(Settings newSettings)
    {
        Settings existSettings = _carRentDbContext.Settings.FirstOrDefault(x => x.Id == newSettings.Id);
        if (existSettings == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newSettings);

        existSettings.Value = newSettings.Value;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}