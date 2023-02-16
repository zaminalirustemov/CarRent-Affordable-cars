using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class PricingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}