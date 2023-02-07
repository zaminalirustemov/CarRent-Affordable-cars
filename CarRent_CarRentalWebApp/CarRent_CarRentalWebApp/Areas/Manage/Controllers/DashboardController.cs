using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
