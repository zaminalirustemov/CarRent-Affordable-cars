using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class CarViewComponent : ViewComponent
{
	public IViewComponentResult Invoke(List<Car> cars)
	{
		return View(cars);
    }
}
