using CarRent_CarRentalWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels;
public class ContactViewModel
{
    public AppUser AppUser { get; set; }
    [StringLength(maximumLength: 200)]
    public string Message { get; set; }
    [StringLength(maximumLength: 20)]
    public string Subject { get; set; }
}