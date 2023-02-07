using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class CarImage
{
    public int Id { get; set; }
    public int CarId { get; set; }


    [StringLength(maximumLength: 100)]
    public string? ImageName { get; set; }
    public bool isPoster { get; set; }

    public Car? Car { get; set; }
}
