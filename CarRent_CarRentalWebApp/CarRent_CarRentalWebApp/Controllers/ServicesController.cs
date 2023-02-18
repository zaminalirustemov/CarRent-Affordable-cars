using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class ServicesController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public ServicesController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        ServicesViewModel servicesViewModel = new ServicesViewModel
        {
            DoYouWants = _carRentDbContext.DoYouWants.ToList(),
        };
        return View(servicesViewModel);
    }
}