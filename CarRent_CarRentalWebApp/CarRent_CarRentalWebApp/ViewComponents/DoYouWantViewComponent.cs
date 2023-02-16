using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class DoYouWantViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}