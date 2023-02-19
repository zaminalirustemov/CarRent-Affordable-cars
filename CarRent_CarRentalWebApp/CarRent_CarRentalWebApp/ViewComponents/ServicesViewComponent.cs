using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class ServicesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Service> services)
    {
        return View(services);
    }
}
