using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class AppUser : IdentityUser
{
    [StringLength(maximumLength: 100)]
    public string Fullname { get; set; }

    [StringLength(maximumLength: 100)]
    public string? ImageName { get; set; }
    public DateTime DateOfBirth { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
