using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class InfoBarController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public InfoBarController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<InfoBar> infoBars = _carRentDbContext.InfoBars.ToList();
        return View(infoBars);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        InfoBar infoBar = _carRentDbContext.InfoBars.FirstOrDefault(x => x.Id == id);
        if (infoBar == null) return View("Error-404");

        return View(infoBar);
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        InfoBar infoBar = _carRentDbContext.InfoBars.FirstOrDefault(x => x.Id == id);
        if (infoBar == null) return View("Error-404");

        return View(infoBar);
    }
    [HttpPost]
    public IActionResult Update(InfoBar newInfoBar)
    {
        InfoBar existInfoBar = _carRentDbContext.InfoBars.FirstOrDefault(x => x.Id == newInfoBar.Id);
        if (existInfoBar == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newInfoBar);

        existInfoBar.Key = newInfoBar.Key;
        existInfoBar.Value = newInfoBar.Value;
        existInfoBar.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}