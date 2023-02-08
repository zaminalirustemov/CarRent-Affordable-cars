using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class CarViewComponent : ViewComponent
{
	private readonly CarRentDbContext _carRentDbContext;

	public CarViewComponent(CarRentDbContext carRentDbContext)
	{
		_carRentDbContext = carRentDbContext;
	}
	public IViewComponentResult Invoke()
	{
		List<Car> cars=_carRentDbContext.Cars
										.Include(x=>x.Brand)
										.Include(x => x.CarImages)
										.Where(x => x.isDeleted == false)
										.ToList();
		return View(cars);
    }
}
