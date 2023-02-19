using CarRent_CarRentalWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels;
public class QuoteViewModel
{
    public AppUser AppUser { get; set; }
    [StringLength(maximumLength:200)]
    public string Quote { get; set; }
    public List<Testimonial> Testimonials { get; set; }
}