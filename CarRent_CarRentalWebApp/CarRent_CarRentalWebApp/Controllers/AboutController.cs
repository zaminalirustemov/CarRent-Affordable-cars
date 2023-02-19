using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class AboutController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly UserManager<AppUser> _userManager;

    public AboutController(CarRentDbContext carRentDbContext, UserManager<AppUser> userManager)
    {
        _carRentDbContext = carRentDbContext;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        AboutViewModel aboutVM = new AboutViewModel
        {
            AboutUs = _carRentDbContext.AboutUs.ToList(),
            DoYouWants = _carRentDbContext.DoYouWants.ToList(),
            Testimonials = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).ToList(),
            InfoBars = _carRentDbContext.InfoBars.ToList(),

        };
        return View(aboutVM);
    }
    //Quote-------------------------------------------------------------------
    public async Task<IActionResult> Quote()
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        QuoteViewModel quoteVM = new QuoteViewModel
        {
            AppUser = appUser,
            Testimonials = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).ToList(),
        };
        return View(quoteVM);
    }
    [HttpPost]
    public async Task<IActionResult> Quote(QuoteViewModel quoteVM)
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        Testimonial testimonial = new Testimonial
        {
            AppUser = appUser,
            AppUserId = appUser.Id,
            Quote = quoteVM.Quote,
            isActive = null,
            SendedDate = DateTime.UtcNow.AddHours(4)
        };

        _carRentDbContext.Testimonials.Add(testimonial);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

}
