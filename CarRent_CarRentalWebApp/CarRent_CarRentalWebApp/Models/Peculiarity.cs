using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Peculiarity
{
    public int Id { get; set; }
    [StringLength(maximumLength: 50)]
    public string Name { get; set; }
    public bool isDeleted { get; set; }

    public List<CarPeculiarity>? CarPeculiarities { get; set; }
}
