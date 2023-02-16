using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class AboutViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
