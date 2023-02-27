using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.ViewModels;
public class HomeViewModel
{
    public List<Hero> Heroes { get; set; }
    public List<Car> FeaturedCars { get; set; }
    public List<AboutUs> AboutUs { get; set; }
    public List<DoYouWant> DoYouWants { get; set; }
    public List<Testimonial> Testimonials { get; set; }
    public List<Service> Services { get; set; }
    public List<Blog> RecentBlog { get; set; }
}