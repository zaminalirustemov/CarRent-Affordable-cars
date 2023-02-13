using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class DeletedOrderController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public DeletedOrderController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Orders.Where(x => x.isDeleted == true).AsQueryable();
        var paginatedList = PaginatedList<Order>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if (order == null) return View("Error-404");

        order.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if (order == null) return BadRequest();

        _carRentDbContext.Orders.Remove(order);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}