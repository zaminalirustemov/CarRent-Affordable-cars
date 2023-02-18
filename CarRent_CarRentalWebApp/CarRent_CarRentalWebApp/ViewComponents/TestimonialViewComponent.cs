using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class TestimonialViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Testimonial> testimonials)
    {
        return View(testimonials);
    }
}