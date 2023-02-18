using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class AboutViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<AboutUs> aboutUs)
    {
        return View(aboutUs);
    }
}
