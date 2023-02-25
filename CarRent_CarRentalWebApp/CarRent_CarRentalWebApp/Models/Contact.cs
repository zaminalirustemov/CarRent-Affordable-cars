using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Contact
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    [StringLength(maximumLength: 20)]
    public string Subject { get; set; }
    [StringLength(maximumLength: 200)]
    public string Message { get; set; }
    public bool? isActive { get; set; }
    public DateTime? SendedDate { get; set; }
    public bool isDeleted { get; set; }

    public AppUser AppUser { get; set; }
}