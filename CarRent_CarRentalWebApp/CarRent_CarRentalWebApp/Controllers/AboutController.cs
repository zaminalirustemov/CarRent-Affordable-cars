using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
