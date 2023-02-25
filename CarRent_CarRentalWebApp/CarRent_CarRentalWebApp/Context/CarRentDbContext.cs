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
	public DbSet<CarComment> CarComments { get; set; }
	public DbSet<Peculiarity> Peculiarities { get; set; }
	public DbSet<CarPeculiarity> CarPeculiarities { get; set; }
	public DbSet<Blog> Blogs { get; set; }
	public DbSet<BlogComment> BlogComments { get; set; }
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Settings> Settings { get; set; }
}