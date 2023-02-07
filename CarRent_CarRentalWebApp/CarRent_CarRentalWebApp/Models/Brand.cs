using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Brand
{
    public int Id { get; set; }

    [StringLength(maximumLength: 100)]
    public string Name { get; set; }
    public bool isDeleted { get; set; }

    public List<Car>? Cars { get; set; }
}