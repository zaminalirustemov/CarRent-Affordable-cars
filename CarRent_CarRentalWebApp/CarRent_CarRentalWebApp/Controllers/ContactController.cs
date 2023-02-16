using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}