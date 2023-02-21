using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class PricingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Car> cars)
    {
        return View(cars);
    }
}