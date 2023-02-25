using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Services;
public class AdminLayoutService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly CarRentDbContext _carRentDbContext;

    public AdminLayoutService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager,CarRentDbContext carRentDbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _carRentDbContext = carRentDbContext;
    }
    public async Task<AppUser> GetUser()
    {
        string name = _httpContextAccessor.HttpContext.User.Identity.Name;
        AppUser appUser = await _userManager.FindByNameAsync(name);
        return appUser;
    }
    public List<Testimonial> NewQuote()
    {
        List<Testimonial> testimonials=_carRentDbContext.Testimonials.Include(x=>x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).ToList();
        
        return testimonials;
    }
    public bool NewCarComment()
    {
        List<CarComment> comments = _carRentDbContext.CarComments.Where(x => x.isDeleted == false).Where(x => x.isActive == null).ToList();
        if (comments.Count > 0) return true;
        return false;
    }
    public bool NewBlogComment()
    {
        List<BlogComment> comments = _carRentDbContext.BlogComments.Where(x => x.isDeleted == false).Where(x => x.isActive == null).ToList();
        if (comments.Count > 0) return true;
        return false;
    }
    public bool NewContact()
    {
        List<Contact> contacts = _carRentDbContext.Contacts.Where(x => x.isDeleted == false).Where(x => x.isActive == null).ToList();
        if (contacts.Count > 0) return true;
        return false;
    }

}

