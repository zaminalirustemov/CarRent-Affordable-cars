using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.ViewModels;
public class AboutViewModel
{
    public List<AboutUs> AboutUs { get; set; }
    public List<DoYouWant> DoYouWants { get; set; }
    public List<Testimonial> Testimonials { get; set; }
    public List<InfoBar> InfoBars { get; set; }
}