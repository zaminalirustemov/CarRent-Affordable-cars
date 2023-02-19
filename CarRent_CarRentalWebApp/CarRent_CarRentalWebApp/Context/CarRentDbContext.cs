using CarRent_CarRentalWebApp.Models;
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
	public DbSet<Category> Categories { get; set; }
	public DbSet<AboutUs> AboutUs { get; set; }
	public DbSet<DoYouWant> DoYouWants { get; set; }
	public DbSet<Testimonial> Testimonials { get; set; }
	public DbSet<Service> Services { get; set; }
	public DbSet<InfoBar> InfoBars { get; set; }
}