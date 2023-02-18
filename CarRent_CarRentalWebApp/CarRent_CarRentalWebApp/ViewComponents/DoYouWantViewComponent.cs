using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class DoYouWantViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<DoYouWant> doYouWants)
    {
        return View(doYouWants);
    }
}