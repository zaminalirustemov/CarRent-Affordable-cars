using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Enums;
using CarRent_CarRentalWebApp.Helpers;
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
    public IActionResult Index(int page=1)
    {
        var query = _carRentDbContext.Orders.Include(x=>x.OrderItem).Where(x => x.isDeleted == false).AsQueryable();
        var paginatedList = PaginatedList<Order>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Detail---------------------------------------------------------------------------
    public IActionResult Detail(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error-404");
        ViewBag.Car = car;

        ViewBag.Orders = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.OrderItem.CarId == car.Id).Where(x => x.OrderStatus == OrderStatus.Accepted).ToList();

        return View(order);
    }
    //Soft Delete----------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Order order = _carRentDbContext.Orders.FirstOrDefault(x=>x.Id==id);
        if (order == null) return BadRequest();
        if (order.OrderStatus==OrderStatus.Rejected || order.OrderStatus==OrderStatus.Finished)
        {
            order.isDeleted = true;
            _carRentDbContext.SaveChanges();
        }

        return Ok();
    }

    //ChangeOrderStatus----------------------------------------------------------------
    public IActionResult Pending(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == id);
        if(order is null) return View("Error-404");
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error-404");

        car.isRent = false;
        order.OrderStatus = OrderStatus.Pending;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Accepted(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x=>x.OrderItem).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error-404");

        car.isRent = true;
        order.OrderStatus = OrderStatus.Accepted;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Rejected(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error-404");

        car.isRent = false;
        order.OrderStatus = OrderStatus.Rejected;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Finished(int id)
    {
        Order order = _carRentDbContext.Orders.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == id);
        if (order is null) return View("Error-404");
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.OrderItem.CarId);
        if (car == null) return View("Error-404");

        car.isRent = false;
        order.OrderStatus = OrderStatus.Finished;
        _carRentDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
