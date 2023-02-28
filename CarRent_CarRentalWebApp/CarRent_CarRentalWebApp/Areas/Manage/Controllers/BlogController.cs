using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class BlogController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BlogController(CarRentDbContext carRentDbContext,IWebHostEnvironment webHostEnvironment)
    {
        _carRentDbContext = carRentDbContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Blogs.Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
        var paginatedList = PaginatedList<Blog>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail----------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Blog blog = _carRentDbContext.Blogs.Include(x=>x.BlogComments).Where(x=>x.isDeleted==false).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error-404");

        return View(blog);
    }

    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Blog blog)
    {
        if (!ModelState.IsValid) return View(blog);

        if (blog.ImageFile is null)
        {
            ModelState.AddModelError("ImageFile", "Image is required");
            return View(blog);
        }
        if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
        {
            ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
            return View(blog);
        }
        if (blog.ImageFile.Length > 2097152)
        {
            ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
            return View(blog);
        }
        blog.ImageName = FileManager.SaveFile(_webHostEnvironment.WebRootPath, "uploads/blog", blog.ImageFile);
        blog.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Blogs.Add(blog);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update----------------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Blog blog = _carRentDbContext.Blogs.Where(x => x.isDeleted == false).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error-404");

        return View(blog);
    }
    [HttpPost]
    public IActionResult Update(Blog newBlog)
    {
        Blog existBlog = _carRentDbContext.Blogs.Include(x => x.BlogComments).Where(x => x.isDeleted == false).FirstOrDefault(i => i.Id == newBlog.Id);
        if (existBlog == null) return View("Error-404");
        if (!ModelState.IsValid) return View(existBlog);
        if (newBlog.ImageFile is not null)
        {
            if (newBlog.ImageFile.ContentType != "image/jpeg" && newBlog.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "The file you upload must be in jpeg or png format.");
                return View(newBlog);
            }
            if (newBlog.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size of your uploaded file must be less than 2 MB.");
                return View(newBlog);
            }
            FileManager.DeleteFile(_webHostEnvironment.WebRootPath, "uploads/blog", existBlog.ImageName);
            existBlog.ImageName = FileManager.SaveFile(_webHostEnvironment.WebRootPath, "uploads/blog", newBlog.ImageFile);
        }

        existBlog.Title = newBlog.Title;
        existBlog.Description = newBlog.Description;
        existBlog.Description = newBlog.Description;
        existBlog.UpdatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete-----------------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Blog blog = _carRentDbContext.Blogs.Where(x => x.isDeleted == false).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error-404");

        blog.isDeleted = true;
        _carRentDbContext.SaveChanges();
        return Ok();
    }



    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Blogs.Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Blog>.Create(query, 5, page);

        return View(paginatedList);
    }
    //Restore---------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Blog blog = _carRentDbContext.Blogs.Where(x => x.isDeleted == true).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error-404");

        blog.isDeleted = false;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete-----------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Blog blog = _carRentDbContext.Blogs.Where(x => x.isDeleted == true).FirstOrDefault(i => i.Id == id);
        if (blog == null) return View("Error-404");

        FileManager.DeleteFile(_webHostEnvironment.WebRootPath, "uploads/blog", blog.ImageName);

        _carRentDbContext.Blogs.Remove(blog);
        _carRentDbContext.SaveChanges();
        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Blog> blogs = _carRentDbContext.Blogs.Where(x => x.isDeleted == true).ToList();
        if (blogs.Count == 0) return BadRequest();

        foreach (Blog blog in blogs)
        {
            FileManager.DeleteFile(_webHostEnvironment.WebRootPath, "uploads/blog", blog.ImageName);
        }

        _carRentDbContext.Blogs.RemoveRange(blogs);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}