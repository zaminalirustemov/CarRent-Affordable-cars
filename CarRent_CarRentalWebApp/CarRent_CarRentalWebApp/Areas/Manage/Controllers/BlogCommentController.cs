using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class BlogCommentController : Controller
{
	private readonly CarRentDbContext _carRentDbContext;

	public BlogCommentController(CarRentDbContext carRentDbContext)
	{
		_carRentDbContext = carRentDbContext;
	}

    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.BlogComments.Include(x=>x.Blog).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<BlogComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult ActiveIndex(int page = 1)
    {
        var query = _carRentDbContext.BlogComments.Include(x => x.Blog).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<BlogComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult PassiveIndex(int page = 1)
    {
        var query = _carRentDbContext.BlogComments.Include(x => x.Blog).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == false).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<BlogComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.Include(x => x.Blog).Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");
        return View(comment);
    }
    //Status---------------------------------------------------------------------------------------------------------------------
    public IActionResult New(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = null;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = comment.Id });
    }
    public IActionResult Active(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = true;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = comment.Id });
    }
    public IActionResult Passive(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = false;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = comment.Id });
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.BlogComments.Include(x => x.Blog).Include(x => x.AppUser).Where(x => x.isDeleted == true).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<BlogComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        BlogComment comment = _carRentDbContext.BlogComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        _carRentDbContext.BlogComments.Remove(comment);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<BlogComment> comments = _carRentDbContext.BlogComments.Where(x => x.isDeleted == true).ToList();
        if (comments.Count == 0) return BadRequest();

        _carRentDbContext.BlogComments.RemoveRange(comments);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}