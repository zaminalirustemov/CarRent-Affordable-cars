﻿using Microsoft.AspNetCore.Mvc;

namespace CarRent_CarRentalWebApp.ViewComponents;
public class ServicesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}