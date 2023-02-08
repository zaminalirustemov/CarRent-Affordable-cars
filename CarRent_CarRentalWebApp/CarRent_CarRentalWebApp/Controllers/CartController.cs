using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
