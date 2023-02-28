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
public class TestimonialController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public TestimonialController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Testimonials.Include(x=>x.AppUser).Where(x=>x.isDeleted==false).Where(x => x.isActive == null).AsQueryable();
        var paginatedList = PaginatedList<Testimonial>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult ActiveIndex(int page = 1)
    {
        var query = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).AsQueryable();
        var paginatedList = PaginatedList<Testimonial>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult PassiveIndex(int page = 1)
    {
        var query = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == false).AsQueryable();
        var paginatedList = PaginatedList<Testimonial>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");
        return View(testimonial);
    }
    //Status---------------------------------------------------------------------------------------------------------------------
    public IActionResult New(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");

        testimonial.isActive = null;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Active(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");

        testimonial.isActive=true;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Passive(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");

        testimonial.isActive = false;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return BadRequest();

        testimonial.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }



    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Testimonials.Include(x => x.AppUser).Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Testimonial>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");

        testimonial.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Testimonial testimonial = _carRentDbContext.Testimonials.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (testimonial is null) return View("Error-404");

        _carRentDbContext.Testimonials.Remove(testimonial);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Testimonial> testimonials = _carRentDbContext.Testimonials.Where(x => x.isDeleted == true).ToList();
        if (testimonials.Count == 0) return BadRequest();
        
        _carRentDbContext.Testimonials.RemoveRange(testimonials);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}