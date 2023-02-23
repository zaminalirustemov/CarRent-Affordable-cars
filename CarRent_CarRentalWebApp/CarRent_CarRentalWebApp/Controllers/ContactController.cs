using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Controllers;
public class ContactController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public ContactController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        return View(appUser);
    }
}