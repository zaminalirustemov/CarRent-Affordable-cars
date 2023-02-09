using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
