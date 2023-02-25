using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class BlogController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly UserManager<AppUser> _userManager;

    public BlogController(CarRentDbContext carRentDbContext,UserManager<AppUser> userManager)
    {
        _carRentDbContext = carRentDbContext;
        _userManager = userManager;
    }
    public IActionResult Index(int page=1) 
    {
        var query = _carRentDbContext.Blogs.Include(x=>x.BlogComments).Where(x => x.isDeleted == false).AsQueryable();
        var paginatedList = PaginatedList<Blog>.Create(query, 3, page);

        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        ViewBag.NewBlogs = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).OrderByDescending(x=>x.CreatedDate).Take(3).ToList();
        Blog blog = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error");
        BlogDetailViewModel blogDetailVM = new BlogDetailViewModel
        {
            Blog = blog,
            BlogComments = _carRentDbContext.BlogComments.Include(x => x.AppUser).Where(x => x.BlogId == id).Where(x => x.isActive == true).Where(x => x.isDeleted == false).ToList(),
        };
        return View(blogDetailVM);
    }
    //BlogComment--------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> BlogComment(BlogComment getComment)
    {
        Blog blog = _carRentDbContext.Blogs.FirstOrDefault(x => x.Id == getComment.BlogId);
        if (blog == null) return View("Error");

        if (!ModelState.IsValid) return RedirectToAction("Detail", new { id = blog.Id });
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

        BlogComment blogComment = new BlogComment
        {
            AppUser = appUser,
            AppUserId = appUser.Id,
            Blog = blog,
            BlogId = blog.Id,
            Comment = getComment.Comment,
            SendedDate = DateTime.UtcNow.AddHours(4),
            isActive = null,
        };

        _carRentDbContext.BlogComments.Add(blogComment);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Detail), new { id = blog.Id });
    }
}