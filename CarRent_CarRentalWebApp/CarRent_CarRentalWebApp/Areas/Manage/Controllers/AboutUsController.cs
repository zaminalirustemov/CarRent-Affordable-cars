using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class AboutUsController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public AboutUsController(CarRentDbContext carRentDbContext, IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    public IActionResult Index()
    {
        List<AboutUs> aboutUs = _carRentDbContext.AboutUs.ToList();
        return View(aboutUs);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        AboutUs about = _carRentDbContext.AboutUs.FirstOrDefault(i => i.Id == id);
        if (about == null) return View("Error-404");

        return View(about);
    }
    //Update----------------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        AboutUs about = _carRentDbContext.AboutUs.FirstOrDefault(i => i.Id == id);
        if (about == null) return View("Error-404");

        return View(about);
    }
    [HttpPost]
    public IActionResult Update(AboutUs newAbout)
    {
        AboutUs existAbout = _carRentDbContext.AboutUs.FirstOrDefault(i => i.Id == newAbout.Id);
        if (existAbout == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existAbout);
        if (newAbout.ImageFile is not null)
        {
            if (newAbout.ImageFile.ContentType != "image/jpeg" && newAbout.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
                return View(newAbout);
            }
            if (newAbout.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newAbout);
            }
            FileManager.DeleteFile(_environment.WebRootPath, "uploads/aboutus", existAbout.ImageName);
            existAbout.ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/aboutus", newAbout.ImageFile);
        }

        existAbout.Title = newAbout.Title;
        existAbout.Description = newAbout.Description;
        existAbout.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}