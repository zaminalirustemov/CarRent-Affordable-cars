using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Enums;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class OrderController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public OrderController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IActionResult Index()
    {
        List<Order> orders = _carRentDbContext.Orders.Where(x => x.isDeleted == false).ToList();
        return View(orders);
    }
    public IActionResult Detail(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return NotFound();
        ViewBag.Car = car;

        return View(order);
    }

    //ChangeOrderStatus----------------------------------------------------------------
    public IActionResult Pending(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if(order is null) return View("Error-404");

        order.OrderStatus = OrderStatus.Pending;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Accepted(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");

        order.OrderStatus = OrderStatus.Accepted;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Rejected(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");

        order.OrderStatus = OrderStatus.Rejected;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Finished(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");

        order.OrderStatus = OrderStatus.Finished;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
