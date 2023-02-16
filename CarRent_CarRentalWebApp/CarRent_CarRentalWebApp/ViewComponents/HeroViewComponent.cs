using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class HeroViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Hero> heroes)
    {
        return View(heroes);
    }
}
