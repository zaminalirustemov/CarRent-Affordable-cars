using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class BlogController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public BlogController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<Blog> blogs = _carRentDbContext.Blogs.Include(x=>x.BlogComments).Where(x => x.isDeleted == false).ToList();
        return View(blogs);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        ViewBag.NewBlogs = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).OrderByDescending(x=>x.CreatedDate).Take(3).ToList();
        Blog blog = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error");

        return View(blog);
    }
}