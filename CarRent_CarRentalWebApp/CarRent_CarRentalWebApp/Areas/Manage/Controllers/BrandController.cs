﻿using CarRent_CarRentalWebApp.Context;
using CarRent_CarRentalWebApp.Helpers;
using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CarRent_CarRentalWebApp.Areas.Manage.Controllers;
[Area("Manage")]
public class BrandController : Controller
{
    private readonly CarRentDbContext _carRentDbContext;

    public BrandController(CarRentDbContext carRentDbContext)
    {
        _carRentDbContext = carRentDbContext;
    }
    //Read---------------------------------------------------------------------------------
    public IActionResult Index(int page = 1)
    {
        var query = _carRentDbContext.Brands
                                     .Where(x => x.isDeleted == false)
                                     .AsQueryable();

        var paginatedList = PaginatedList<Brand>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Create-------------------------------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Brand brand)
    {
        if (!ModelState.IsValid) return View(brand);

        brand.CreatedDate = DateTime.UtcNow.AddHours(4);
        _carRentDbContext.Brands.Add(brand);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Update-------------------------------------------------------------------------------
    public IActionResult Update(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        return View(brand);
    }
    [HttpPost]
    public IActionResult Update(Brand newBrand)
    {
        Brand existBrand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == newBrand.Id);
        if (existBrand == null) return View("Error-404");
        if (!ModelState.IsValid) return View(newBrand);

        existBrand.Name = newBrand.Name;
        existBrand.UpdatedDate=DateTime.UtcNow.AddHours(4);
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    //Soft Delete--------------------------------------------------------------------------
    public IActionResult SoftDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return BadRequest();

        brand.isDeleted = true;
        _carRentDbContext.SaveChanges();

        return Ok();
    }


    //*************************************************************************************
    //*************************************Recycle Bin*************************************
    //*************************************************************************************
    //Deleted Index------------------------------------------------------------------------
    public IActionResult DeletedIndex(int page = 1)
    {
        var query = _carRentDbContext.Brands
                                      .Where(x => x.isDeleted == true)
                                      .AsQueryable();

        var paginatedList = PaginatedList<Brand>.Create(query, 5, page);
        return View(paginatedList);
    }
    //Restore------------------------------------------------------------------------------
    public IActionResult Restore(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return View("Error-404");

        brand.isDeleted = false;
        _carRentDbContext.SaveChanges();

        return RedirectToAction(nameof(DeletedIndex));
    }
    //Hard Delete--------------------------------------------------------------------------
    public IActionResult HardDelete(int id)
    {
        Brand brand = _carRentDbContext.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) return BadRequest();

        _carRentDbContext.Brands.Remove(brand);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
    //All Delete-----------------------------------------------------------------------
    public IActionResult AllDelete()
    {
        List<Brand> brands = _carRentDbContext.Brands.Where(x => x.isDeleted == true).ToList();
        if (brands.Count == 0) return BadRequest();

        _carRentDbContext.Brands.RemoveRange(brands);
        _carRentDbContext.SaveChanges();

        return Ok();
    }
}
