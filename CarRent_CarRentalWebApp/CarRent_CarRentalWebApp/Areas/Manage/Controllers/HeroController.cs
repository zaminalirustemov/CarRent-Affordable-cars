using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Data;
using System.Drawing.Drawing2D;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]

public class HeroController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public HeroController(CarRentDbContext carRentDbContext,IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    //Read------------------------------------------------------------------------------------------
    public IActionResult Index()
    {
        List<Hero> hero = _carRentDbContext.Heroes.ToList();
        return View(hero);
    }
    //Update----------------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Hero hero=_carRentDbContext.Heroes.FirstOrDefault(i=>i.Id==id);
        if (hero == null) return View("Error-404");

        return View(hero);
    }
    [HttpPost]
    public IActionResult Update(Hero newHero)
    {
        Hero existHero = _carRentDbContext.Heroes.FirstOrDefault(i => i.Id == newHero.Id);
        if (existHero == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existHero);
        if (newHero.ImageFile is not null)
        {
            if (newHero.ImageFile.ContentType != "image/jpeg" && newHero.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
                return View(newHero);
            }
            if (newHero.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newHero);
            }
            FileManager.DeleteFile(_environment.WebRootPath, "uploads/hero", existHero.ImageName);
            existHero.ImageName = FileManager.SaveFile(_environment.WebRootPath,"uploads/hero",newHero.ImageFile);
        }

        existHero.Title = newHero.Title;
        existHero.Description = newHero.Description;
        existHero.VideoText = newHero.VideoText;
        existHero.VideoUrl = newHero.VideoUrl;
        existHero.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
