using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class HomeController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public HomeController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        HomeViewModel homeVM = new HomeViewModel
        {
            Heroes = _carRentDbContext.Heroes.ToList(),
            FeaturedCars = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Include(x => x.Category).Where(x => x.isDeleted == false).Where(x => x.isFeatured == true).Where(x => x.Brand.isDeleted == false).Where(x => x.Category.isDeleted == false).ToList(),
            AboutUs = _carRentDbContext.AboutUs.ToList(),
            DoYouWants = _carRentDbContext.DoYouWants.ToList(),
            Testimonials = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).ToList(),
            Services = _carRentDbContext.Services.Where(x => x.isDeleted == false).OrderByDescending(x=>x.CreatedDate).ToList(),
            RecentBlog = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).OrderByDescending(x=>x.CreatedDate).Take(3).ToList(),
            InfoBars = _carRentDbContext.InfoBars.ToList(),
        };
        return View(homeVM);
    }

}