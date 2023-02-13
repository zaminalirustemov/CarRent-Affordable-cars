using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DashboardController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Index()
    {
        return View();
    }
    //CreateAdmin-----------------------------------------------------------------------------------------
    public async Task<IActionResult> CreateAdmin()
    {
        AppUser appUser = new AppUser
        {
            UserName = "Zamin",
            Fullname = "Zaminali Rustemov",
            Email = "zamin@gmail.com",
            PhoneNumber = "010 123 45 67"
        };
        var result = await _userManager.CreateAsync(appUser, "Zamin123");
        return Ok(result);
    }
    //CreateRoles-----------------------------------------------------------------------------------------
    public async Task<IActionResult> CreateRoles()
    {
        IdentityRole role1 = new IdentityRole("SuperAdmin");
        IdentityRole role2 = new IdentityRole("Admin");
        IdentityRole role3 = new IdentityRole("Editor");
        IdentityRole role4 = new IdentityRole("Member");

        await _roleManager.CreateAsync(role1);
        await _roleManager.CreateAsync(role2);
        await _roleManager.CreateAsync(role3);
        await _roleManager.CreateAsync(role4);

        return Content(">>>Created roles.");
    }
    //AddRole---------------------------------------------------------------------------------------------
    public async Task<IActionResult> AddRole()
    {
        AppUser appUser = await _userManager.FindByNameAsync("Zamin");
        await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        return Content(">>>Added role.");
    }
}
