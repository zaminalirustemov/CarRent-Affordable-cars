﻿using CarRent_CarRentalWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Context;
public class CarRentDbContext:IdentityDbContext
{
	public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options) { }

	public DbSet<Hero> Heroes { get; set; }
	public DbSet<Brand> Brands { get; set; }
	public DbSet<Car> Cars { get; set; }
	public DbSet<CarImage> CarImages { get; set; }
	public DbSet<AppUser> AppUsers { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderItem> OrderItems { get; set; }
}