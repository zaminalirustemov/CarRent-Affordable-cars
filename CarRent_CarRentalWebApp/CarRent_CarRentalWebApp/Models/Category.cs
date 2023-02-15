using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Category
{
    public int Id { get; set; }

    [StringLength(maximumLength: 100)]
    public string Name { get; set; }
    public bool isDeleted { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public List<Car>? Cars { get; set; }
}
