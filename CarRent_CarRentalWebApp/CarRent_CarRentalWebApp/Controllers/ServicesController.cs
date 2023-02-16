using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class ServicesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}