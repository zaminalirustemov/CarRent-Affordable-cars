using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Services;
public class MemberLayoutService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CarRentDbContext _carRentDbContext;

    public MemberLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor,CarRentDbContext carRentDbContext)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _carRentDbContext = carRentDbContext;
    }

    public async Task<AppUser> GetUser()
    {
        AppUser user = null;
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            string name = _httpContextAccessor.HttpContext.User.Identity.Name;

            user = await _userManager.FindByNameAsync(name);
            return user;
        }
        return null;
    }
    public async Task<List<Settings>> GetSettingsAsync()
    {
        return await _carRentDbContext.Settings.ToListAsync();
    }
}
