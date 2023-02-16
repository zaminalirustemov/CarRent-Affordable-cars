using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class BlogViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}