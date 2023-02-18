using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class DoYouWant
{
    public int Id { get; set; }

    [StringLength(maximumLength: 100)]
    public string? ImageName { get; set; }
    [StringLength(maximumLength: 90)]
    public string Title { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
