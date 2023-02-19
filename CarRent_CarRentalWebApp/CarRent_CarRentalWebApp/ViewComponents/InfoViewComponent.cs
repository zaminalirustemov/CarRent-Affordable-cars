using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class InfoViewComponent: ViewComponent
{
    public IViewComponentResult Invoke(List<InfoBar> InfoBars)
    {
        return View(InfoBars);
    }
}
