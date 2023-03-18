using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class ContactController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly CarRentDbContext _carRentDbContext;

    public ContactController(UserManager<AppUser> userManager,CarRentDbContext carRentDbContext)
    {
        _userManager = userManager;
        _carRentDbContext = carRentDbContext;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(ContactViewModel contactVM)
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        if (!ModelState.IsValid) return View(contactVM);
        Contact contact = new Contact
        {
            AppUser = appUser,
            AppUserId = appUser.Id,
            Message = contactVM.Message,
            Subject = contactVM.Subject,
            SendedDate = DateTime.UtcNow.AddHours(4),
            isActive=null,
        };
        
        _carRentDbContext.Contacts.Add(contact);
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}