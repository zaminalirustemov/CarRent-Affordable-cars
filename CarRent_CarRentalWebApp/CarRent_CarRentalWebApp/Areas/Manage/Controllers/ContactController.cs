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
public class ContactController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public ContactController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Contacts.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == null).AsQueryable();
        var paginatedList = PaginatedList<Contact>.Create(query, 5, page);
        return View(paginatedList);
    }
    public IActionResult OldIndex(int page = 1)
    {
        var query = _carRentDbContext.Contacts.Include(x => x.AppUser).Where(x => x.isDeleted == false).Where(x => x.isActive == true).AsQueryable();
        var paginatedList = PaginatedList<Contact>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");
        return View(contact);
    }
    //Status---------------------------------------------------------------------------------------------------------------------
    public IActionResult New(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");

        contact.isActive = null;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = contact.Id });
    }
    public IActionResult WasRead(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");

        contact.isActive = true;
        _carRentDbContext.SaveChanges();
        return RedirectToAction("Detail", new { id = contact.Id });
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");

        contact.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Contacts.Include(x => x.AppUser).Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Contact>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");

        contact.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Contact contact = _carRentDbContext.Contacts.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);
        if (contact is null) return View("Error-404");

        _carRentDbContext.Contacts.Remove(contact);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Contact> contacts = _carRentDbContext.Contacts.Where(x => x.isDeleted == true).ToList();
        if (contacts.Count == 0) return BadRequest();

        _carRentDbContext.Contacts.RemoveRange(contacts);
        _carRentDbContext.SaveChanges();

        return Ok();
    }

}