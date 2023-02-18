using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class AboutUs
{
    public int Id { get; set; }

    [StringLength(maximumLength: 100)]
    public string? ImageName { get; set; }
    [StringLength(maximumLength: 20)]
    public string Title { get; set; }
    [StringLength(maximumLength: 700)]
    public string Description { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
