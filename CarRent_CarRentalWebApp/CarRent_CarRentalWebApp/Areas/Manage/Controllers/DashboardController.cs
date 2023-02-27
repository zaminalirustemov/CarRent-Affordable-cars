using CarRent_CarRentalWebApp.Areas.Manage.ViewModels;
using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DashboardController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly CarRentDbContext _carRentDbContext;

    public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, CarRentDbContext carRentDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        DashboardViewModel dashboardVM = new DashboardViewModel
        {
            Cars = _carRentDbContext.Cars.Where(x => x.isDeleted == false).Where(x => x.Brand.isDeleted == false).Where(x => x.Category.isDeleted == false).ToList(),
            carComments = _carRentDbContext.CarComments.Where(x => x.isActive == true).Where(x => x.isDeleted == false).ToList(),
            blogComments = _carRentDbContext.BlogComments.Where(x => x.isActive == true).Where(x => x.isDeleted == false).ToList(),
            Brands = _carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList(),
            Categories = _carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList(),
            Services = _carRentDbContext.Services.Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).ToList(),
            Blogs = _carRentDbContext.Blogs.Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).ToList(),
            Orders = _carRentDbContext.Orders.Include(x=>x.AppUser).Where(x => x.isDeleted == false).Where(x => x.OrderStatus == Enums.OrderStatus.Pending).OrderByDescending(x => x.PickUp).ToList(),
            Contacts= _carRentDbContext.Contacts.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).Take(4).ToList(),
        };
        return View(dashboardVM);
    }
    ////CreateAdmin-----------------------------------------------------------------------------------------
    //public async Task<IActionResult> CreateAdmin()
    //{
    //    AppUser appUser = new AppUser
    //    {
    //        UserName = "Zamin",
    //        Fullname = "Zaminali Rustemov",
    //        Email = "zamin@gmail.com",
    //        PhoneNumber = "010 123 45 67"
    //    };
    //    var result = await _userManager.CreateAsync(appUser, "Zamin123");
    //    return Ok(result);
    //}
    ////CreateRoles-----------------------------------------------------------------------------------------
    //public async Task<IActionResult> CreateRoles()
    //{
    //    IdentityRole role1 = new IdentityRole("SuperAdmin");
    //    IdentityRole role2 = new IdentityRole("Admin");
    //    IdentityRole role3 = new IdentityRole("Editor");
    //    IdentityRole role4 = new IdentityRole("Member");

    //    await _roleManager.CreateAsync(role1);
    //    await _roleManager.CreateAsync(role2);
    //    await _roleManager.CreateAsync(role3);
    //    await _roleManager.CreateAsync(role4);

    //    return Content(">>>Created roles.");
    //}
    ////AddRole---------------------------------------------------------------------------------------------
    //public async Task<IActionResult> AddRole()
    //{
    //    AppUser appUser = await _userManager.FindByNameAsync("Zamin");
    //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

    //    return Content(">>>Added role.");
    //}
}
