using CarRent_CarRentalWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRent_CarRentalWebApp.Context;
public class CarRentDbContext:DbContext
{
	public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options) { }

	public DbSet<Hero> Heroes { get; set; }
	public DbSet<Brand> Brands { get; set; }
	public DbSet<Car> Cars { get; set; }
	public DbSet<CarImage> CarImages { get; set; }
}