using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class DoYouWantController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public DoYouWantController(CarRentDbContext carRentDbContext,IWebHostEnvironment environment)
    {
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    public IActionResult Index()
    {
        List<DoYouWant> doYouWantList = _carRentDbContext.DoYouWants.ToList();
        return View(doYouWantList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        DoYouWant doYouWant = _carRentDbContext.DoYouWants.FirstOrDefault(i => i.Id == id);
        if (doYouWant == null) return View("Error-404");

        return View(doYouWant);
    }
    //Update----------------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        DoYouWant doYouWant = _carRentDbContext.DoYouWants.FirstOrDefault(i => i.Id == id);
        if (doYouWant == null) return View("Error-404");

        return View(doYouWant);
    }
    [HttpPost]
    public IActionResult Update(DoYouWant newDoYouWant)
    {
        DoYouWant existDoYouWant = _carRentDbContext.DoYouWants.FirstOrDefault(i => i.Id == newDoYouWant.Id);
        if (existDoYouWant == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existDoYouWant);
        if (newDoYouWant.ImageFile is not null)
        {
            if (newDoYouWant.ImageFile.ContentType != "image/jpeg" && newDoYouWant.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
                return View(newDoYouWant);
            }
            if (newDoYouWant.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newDoYouWant);
            }
            FileManager.DeleteFile(_environment.WebRootPath, "uploads/doyouwant", existDoYouWant.ImageName);
            existDoYouWant.ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/doyouwant", newDoYouWant.ImageFile);
        }

        existDoYouWant.Title = newDoYouWant.Title;
        existDoYouWant.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}