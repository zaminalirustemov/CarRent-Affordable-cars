using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class BlogController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}