using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CarRent_CarRentalWebApp.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _environment;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, CarRentDbContext carRentDbContext,IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _carRentDbContext = carRentDbContext;
        _environment = environment;
    }
    //Register-----------------------------------------------------------------------------------------------------
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
    {
        if (!ModelState.IsValid) return View();
        if (DateManager.LittleThan18(memberRegisterVM.DateOfBith))
        {
            ModelState.AddModelError("DateOfBith", "You must be over 18 years old to register");
            return View();
        }
        AppUser appUser = null;

        appUser = await _userManager.FindByNameAsync(memberRegisterVM.Username);
        if (appUser is not null)
        {
            ModelState.AddModelError("Username", "Already exist!");
            return View();
        }

        appUser = await _userManager.FindByEmailAsync(memberRegisterVM.Email);
        if (appUser is not null)
        {
            ModelState.AddModelError("Email", "Already exist!");
            return View();
        }

        appUser = new AppUser
        {
            Fullname = memberRegisterVM.Fullname,
            UserName = memberRegisterVM.Username,
            DateOfBirth=memberRegisterVM.DateOfBith,
            Email = memberRegisterVM.Email,
            PhoneNumber = memberRegisterVM.PhoneNumber
        };


        var result = await _userManager.CreateAsync(appUser, memberRegisterVM.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        string link = Url.Action(nameof(Verify), "Account", new { email = appUser.Email, token = token }, Request.Scheme, Request.Host.ToString());

        MailExtension.SendMessage(appUser.Email, "Verify Email", link);

        await _userManager.AddToRoleAsync(appUser, "Member");


        return RedirectToAction("index", "home");
    }


    public async Task<IActionResult> Verify(string email, string token)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }
        await _userManager.ConfirmEmailAsync(user, token);
        await _signInManager.SignInAsync(user, true);
        return RedirectToAction("index", "Home");
    }

    //Log in-------------------------------------------------------------------------------------------------------
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
    {
        if (!ModelState.IsValid) return View();
        AppUser user = await _userManager.FindByNameAsync(memberLoginVM.UserName);
        if (user is null)
        {
            ModelState.AddModelError("", "Username or password is false");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(user, memberLoginVM.Password, false, false);

        if (!result.Succeeded)
        {
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please verify email");
                return View();

            }
            ModelState.AddModelError("", "Username or password is false");
            return View();
        }

        return RedirectToAction("index", "home");
    }
    //Log out------------------------------------------------------------------------------------------------------
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("index", "home");
    }
    //Orders-------------------------------------------------------------------------------------------------------
    public async Task<IActionResult> Orders(int page=1)
    {
        AppUser member = null;
        if (User.Identity.IsAuthenticated) member = await _userManager.FindByNameAsync(User.Identity.Name);

        var query = _carRentDbContext.Orders.Where(x => x.AppUserId == member.Id).Where(x => x.isDeleted == false).AsQueryable();
        var paginatedList = PaginatedList<Order>.Create(query, 7, page);
        return View(paginatedList);
    }
    public IActionResult Detail(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error");

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error");
        ViewBag.Car = car;

        return View(order);
    }
    //Profile------------------------------------------------------------------------------------------------------
    public async Task<IActionResult> Profile()
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        return View(appUser);
    }
    public async Task<IActionResult> EditProfile()
    {
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        return View(appUser);
    }
    [HttpPost]
    public async Task<IActionResult> EditProfile(AppUser newAppUser)
    {
        AppUser existAppUser =await _userManager.FindByNameAsync(newAppUser.UserName);
        if (existAppUser == null) return View("Error");
        if (!ModelState.IsValid) return View(newAppUser);
        if (newAppUser.ImageFile is not null)
        {
            if (newAppUser.ImageFile.ContentType != "image/jpeg" && newAppUser.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
                return View(newAppUser);
            }
            if (newAppUser.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newAppUser);
            }
            if (existAppUser.ImageName is not null)
            {
                FileManager.DeleteFile(_environment.WebRootPath, "uploads/user", existAppUser.ImageName);
            }
            existAppUser.ImageName = FileManager.SaveFile(_environment.WebRootPath, "uploads/user", newAppUser.ImageFile);
        }

        existAppUser.Fullname = newAppUser.Fullname;
        existAppUser.Email = newAppUser.Email;
        existAppUser.PhoneNumber = newAppUser.PhoneNumber;
        
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Profile));
    }
}
