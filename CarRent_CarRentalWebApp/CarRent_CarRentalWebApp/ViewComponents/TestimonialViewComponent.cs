using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class TestimonialViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}