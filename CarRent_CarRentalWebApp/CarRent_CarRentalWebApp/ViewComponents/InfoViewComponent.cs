using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Models;
using CarRent_CarRentalWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class InfoViewComponent: ViewComponent
{
    private readonly CarRentDbContext _carRentDbContext;

    public InfoViewComponent(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    public IViewComponentResult Invoke()
    {
        InfoBarViewModel ınfoBarViewModel = new InfoBarViewModel
        {
            Cars= _carRentDbContext.Cars.Where(x => x.isDeleted == false).Where(x => x.Brand.isDeleted == false).Where(x => x.Category.isDeleted == false).ToList(),
            carComments= _carRentDbContext.CarComments.Where(x => x.isActive == true).Where(x => x.isDeleted == false).ToList(),
            blogComments= _carRentDbContext.BlogComments.Where(x => x.isActive == true).Where(x => x.isDeleted == false).ToList(),
            Brands =_carRentDbContext.Brands.Where(x => x.isDeleted == false).ToList(),
            Categories=_carRentDbContext.Categories.Where(x => x.isDeleted == false).ToList(),
        };
        return View(ınfoBarViewModel);
    }
}
