using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class BlogViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Blog> blogs)
    {
        return View(blogs);
    }
}