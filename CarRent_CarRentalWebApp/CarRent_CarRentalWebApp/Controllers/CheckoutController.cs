using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Controllers;
public class CheckoutController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;
    private readonly UserManager<AppUser> _userManager;

    public CheckoutController(CarRentDbContext carRentDbContext, UserManager<AppUser> userManager)
    {
        _carRentDbContext = carRentDbContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        OrderViewModel orderViewModel = null;

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).FirstOrDefault(x => x.Id == id);
        if (car == null) return NotFound();
        orderViewModel = new OrderViewModel
        {
            Car = car,
            CarId = car.Id,
            Fullname = member?.Fullname,
            Phonenumber = member?.PhoneNumber,
            Email = member?.Email
        };
        return View(orderViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(OrderViewModel orderVM)
    {
        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).FirstOrDefault(x => x.Id == orderVM.CarId);
        orderVM.Car = car;
        if (!ModelState.IsValid) return View(orderVM);
        if (orderVM.PickUp.CompareTo(DateTime.Now) <0)
        {
            ModelState.AddModelError("PickUp", "The pick-up date cannot be selected later than the current time");
            return View(orderVM);
        }
        if (orderVM.PickUp >= orderVM.DropOff)
        {
            ModelState.AddModelError("DropOff", "The drop-off date must be after the pick-up date");
            return View(orderVM);
        }
        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        Order order = null;
        OrderItem orderItem = null;

        order = new Order
        {
            Fullname = orderVM.Fullname,
            Email = orderVM.Email,
            Phonenumber = orderVM.Phonenumber,
            DropOffLocation = orderVM.DropOffLocation,
            PickUp = orderVM.PickUp,
            DropOff = orderVM.DropOff,
            PickUpTime = orderVM.PickUpTime,
            AppUserId = member?.Id

        };

        TempData.Put<Order>("order", order);


        orderItem = new OrderItem
        {
            CarId = car.Id,
            Model = car.Model,
            Millage = car.Millage,
            Brand = car.Brand.Name,
            PricePerDay = car.PricePerDay,
            PricePerMonth = car.PricePerMonth,
            Order = order
        };

        TempData.Put<OrderItem>("orderItem", orderItem);

        //OrderItem orderItemRoute = TempData.Get<OrderItem>("orderItem");
        //Order orderroute = TempData.Get<Order>("order");

        return RedirectToAction("confirmorder");
    }

    public async Task<IActionResult> ConfirmOrder()
    {
        ConfirmOrderViewModel confirmOrderVM = null;
        OrderItem orderItem = TempData.Get<OrderItem>("orderItem");
        Order order = TempData.Get<Order>("order");

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).FirstOrDefault(x => x.Id == orderItem.CarId);
        int day = order.DropOff.Subtract(order.PickUp).Days;
        var totalPrice = day * orderItem.PricePerDay;

        confirmOrderVM = new ConfirmOrderViewModel
        {
            Order = order,
            OrderId = order.Id,
            Fullname = order.Fullname,
            TotalPrice = totalPrice,
            Day = day,
            Car=car,
            CarId=car.Id,

            Phonenumber=order.Phonenumber,
            Email=order.Email,
            DropOffLocation=order.DropOffLocation,
            PickUp=order.PickUp,
            DropOff=order.DropOff,
            PickUpTime=order.PickUpTime,

        };
        


        return View(confirmOrderVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmOrder(ConfirmOrderViewModel confirmOrderVM)
    {
        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).FirstOrDefault(x => x.Id == confirmOrderVM.CarId);
        confirmOrderVM.Car = car;
        if(!ModelState.IsValid) return View(confirmOrderVM);

        Order order = new Order
        {
            Fullname = confirmOrderVM.Fullname,
            Email = confirmOrderVM.Email,
            Phonenumber = confirmOrderVM.Phonenumber,
            DropOffLocation = confirmOrderVM.DropOffLocation,
            PickUp = confirmOrderVM.PickUp,
            DropOff = confirmOrderVM.DropOff,
            PickUpTime = confirmOrderVM.PickUpTime,
            CardNumber = confirmOrderVM.CardNumber,
            EndTime = confirmOrderVM.EndTime,
            CVC = confirmOrderVM.CVC,
            Day=confirmOrderVM.Day,
            TotalPrice=confirmOrderVM.TotalPrice,
            AppUserId=member?.Id
        };

        OrderItem orderItem = new OrderItem
        {
            CarId = car.Id,
            Model = car.Model,
            Millage = car.Millage,
            Brand = car.Brand.Name,
            PricePerDay = car.PricePerDay,
            PricePerMonth = car.PricePerMonth,
            Order = order
        };

        order.OrderItems.Add(orderItem);

        _carRentDbContext.Orders.Add(order);
        _carRentDbContext.SaveChanges();
        return RedirectToAction("index","home");
    }
}
