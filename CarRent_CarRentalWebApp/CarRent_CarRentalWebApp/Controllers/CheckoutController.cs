using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Enums;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

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
        OrderViewModel orderViewModel = null;
        List<Order> orders = null;
        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated) member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == id);
        if (car == null) return View("Error");

        ViewBag.RelatedOrders = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.isDeleted == false).Where(x => x.OrderItem.CarId == id).Where(x => x.OrderStatus == OrderStatus.Accepted || x.OrderStatus == OrderStatus.Pending).ToList();

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
        ViewBag.RelatedOrders = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.isDeleted == false).Where(x => x.OrderItem.CarId == orderVM.CarId).Where(x => x.OrderStatus == OrderStatus.Accepted || x.OrderStatus == OrderStatus.Pending).ToList();

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == orderVM.CarId);
        if (car == null) return View("Error");
        orderVM.Car = car;
        if (!ModelState.IsValid) return View(orderVM);

        if ((orderVM.PickUp - DateTime.Now).TotalDays < 1)
        {
            ModelState.AddModelError("", "The car must be reserved at least 2 day in advance");
            return View(orderVM);
        }
        if (!DateManager.DateLogical(orderVM.PickUp, orderVM.DropOff))
        {
            ModelState.AddModelError("", "This time interval does not match the current time");
            return View(orderVM);
        }

        List<Order> orders = _carRentDbContext.Orders.Include(x => x.OrderItem).Where(x => x.OrderItem.CarId == car.Id).Where(x => x.OrderStatus == OrderStatus.Accepted || x.OrderStatus == OrderStatus.Pending).ToList();
        foreach (Order o in orders)
        {
            if (DateManager.IntersectionTimeIntervals(orderVM.PickUp, orderVM.DropOff, o.PickUp, o.DropOff))
            {
                ModelState.AddModelError("", "Car was rented between the selected dates");
                return View(orderVM);
            }
        }

        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated) member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        Order order = null;
        OrderItem orderItem = null;

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

        order = new Order
        {
            Fullname = orderVM.Fullname,
            Email = orderVM.Email,
            Phonenumber = orderVM.Phonenumber,
            DropOffLocation = orderVM.DropOffLocation,
            PickUp = orderVM.PickUp,
            DropOff = orderVM.DropOff,
            PickUpTime = orderVM.PickUpTime,
            AppUserId = member?.Id,
            OrderItem = orderItem,
            CarId = car.Id,
        };

        TempData.Put<Order>("order", order);

        return RedirectToAction("confirmorder", TempData.Get<Order>("order"));
    }

    //ConfirmOrder--------------------------------------------------------------------------------------------------------------------------------------
    public async Task<IActionResult> ConfirmOrder(Order order)
    {
        ConfirmOrderViewModel confirmOrderVM = null;

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == order.CarId);
        if (car == null) return View("Error");

        int day = order.DropOff.Subtract(order.PickUp).Days;
        double totalPrice = 0;
        
        if (day >= 30)
        {
            var month = Math.Floor((double)day / 30);
            var restDay = day - (month * 30);
            if (restDay >= 7)
            {
                var week = Math.Floor((double)restDay / 7);
                restDay = restDay - (week * 7);
                totalPrice = (car.PricePerMonth * month) +(car.PricePerWeek * week) + (car.PricePerDay * restDay);
            }
            else
            {
                totalPrice= (car.PricePerMonth * month) + (car.PricePerDay * restDay);
            }
        }
        else if (day >= 7)
        {
            var week = Math.Floor((double)day / 7);
            var restDay = day - (week * 7);

            totalPrice = (car.PricePerWeek * week) + (car.PricePerDay * restDay);
        }
        else
        {
            totalPrice = car.PricePerDay * day;
        }

        confirmOrderVM = new ConfirmOrderViewModel
        {
            Order = order,
            OrderId = order.Id,
            Fullname = order.Fullname,
            TotalPrice = totalPrice,
            Day = day,
            Car = car,
            CarId = car.Id,
            Phonenumber = order.Phonenumber,
            Email = order.Email,
            DropOffLocation = order.DropOffLocation,
            PickUp = order.PickUp,
            DropOff = order.DropOff,
            PickUpTime = order.PickUpTime,
        };

        return View(confirmOrderVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmOrder(ConfirmOrderViewModel confirmOrderVM)
    {
        AppUser member = null;
        if (HttpContext.User.Identity.IsAuthenticated) member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        Car car = _carRentDbContext.Cars.Include(x => x.Brand).Include(x => x.CarImages).Where(x => x.isDeleted == false).FirstOrDefault(x => x.Id == confirmOrderVM.CarId);
        if (car == null) return View("Error");
        confirmOrderVM.Car = car;
        if (!ModelState.IsValid) return View(confirmOrderVM);
        if (confirmOrderVM.DropOff.Subtract(confirmOrderVM.PickUp).Days != confirmOrderVM.Day)
        {
            ModelState.AddModelError("Day", "Error");
            return View(confirmOrderVM);
        }
        double totalPrice = 0;
        if (confirmOrderVM.Day >= 30)
        {
            var month = Math.Floor((double)confirmOrderVM.Day / 30);
            var restDay = confirmOrderVM.Day - (month * 30);
            if (restDay >= 7)
            {
                var week = Math.Floor((double)restDay / 7);
                restDay = restDay - (week * 7);
                totalPrice = (car.PricePerMonth * month) + (car.PricePerWeek * week) + (car.PricePerDay * restDay);
            }
            else
            {
                totalPrice = (car.PricePerMonth * month) + (car.PricePerDay * restDay);
            }
        }
        else if (confirmOrderVM.Day >= 7)
        {
            var week = Math.Floor((double)confirmOrderVM.Day / 7);
            var restDay = confirmOrderVM.Day - (week * 7);

            totalPrice = (car.PricePerWeek * week) + (car.PricePerDay * restDay);
        }
        else
        {
            totalPrice = car.PricePerDay * confirmOrderVM.Day;
        }
        if (totalPrice != confirmOrderVM.TotalPrice)
        {
            ModelState.AddModelError("TotalPrice", "Error");
            return View(confirmOrderVM);
        }
        if (!CardInformation.IsCreditCardInfoValid(confirmOrderVM.CardNumber, confirmOrderVM.EndTime, confirmOrderVM.CVC))
        {
            ModelState.AddModelError("", "Card information is incorrect");
            return View(confirmOrderVM);
        }

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
            Day = confirmOrderVM.Day,
            TotalPrice = confirmOrderVM.TotalPrice,
            OrderStatus = OrderStatus.Pending,
            Car = car,
            CreatedDate = DateTime.UtcNow.AddHours(4),
            AppUserId = member?.Id
        };

        OrderItem orderItem = new OrderItem
        {
            CarId = car.Id,
            Model = car.Model,
            Millage = car.Millage,
            Brand = car.Brand.Name,
            PricePerDay = car.PricePerDay,
            PricePerMonth = car.PricePerMonth,
            Order = order,


        };
        order.OrderItem = orderItem;

        _carRentDbContext.Orders.Add(order);
        _carRentDbContext.SaveChanges();
        return RedirectToAction("index", "home");
    }
}
