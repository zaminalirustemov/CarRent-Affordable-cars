using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class CommentController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public CommentController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x=>x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).OrderByDescending(x=>x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult ActiveIndex(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult PassiveIndex(int page = 1)
    {
        var query = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == false).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");
        return View(comment);
    }
    //Status---------------------------------------------------------------------------------------------------------------------
    public IActionResult New(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = null;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new {id=comment.Id});
    }
    public IActionResult Active(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = true;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = comment.Id });
    }
    public IActionResult Passive(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isActive = false;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = comment.Id });
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
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
        var query = _carRentDbContext.CarComments.Include(x => x.Car.CarImages).Include(x => x.AppUser).Where(x => x.isDeleted == true).OrderByDescending(x => x.SendedDate).AsQueryable();
        var paginatedList = PaginatedList<CarComment>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        comment.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        CarComment comment = _carRentDbContext.CarComments.FirstOrDefault(x => x.Id == id);
        if (comment is null) return View("Error-404");

        _carRentDbContext.CarComments.Remove(comment);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<CarComment> carComments = _carRentDbContext.CarComments.Where(x => x.isDeleted == true).ToList();
        if(carComments.Count==0) return BadRequest();

        _carRentDbContext.CarComments.RemoveRange(carComments);
        _carRentDbContext.SaveChanges();

        return Ok();
    }

}