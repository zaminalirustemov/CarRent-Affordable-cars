using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class AboutController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public AboutController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        AboutViewModel aboutVM = new AboutViewModel
        {
            AboutUs=_carRentDbContext.AboutUs.ToList(),
        };
        return View(aboutVM);
    }
}
