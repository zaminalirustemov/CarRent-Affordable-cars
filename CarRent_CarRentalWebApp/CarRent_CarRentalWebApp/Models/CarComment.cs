using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class CarComment
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public int CarId { get; set; }


    [StringLength(maximumLength: 200)]
    public string Comment { get; set; }
    public byte Rating { get; set; }
    public bool? isActive { get; set; }
    public DateTime? SendedDate { get; set; }
    public bool isDeleted { get; set; }

    public AppUser? AppUser { get; set; }
    public Car? Car { get; set; }
}