using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class Hero
{
    public int Id { get; set; }

    [StringLength(maximumLength:100)]
    public string? ImageName { get; set; }
    [StringLength(maximumLength:75)]
    public string Title { get; set; }
    [StringLength(maximumLength:250)]
    public string Description { get; set; }
    [StringLength(maximumLength:50)]
    public string VideoText { get; set; }
    [StringLength(maximumLength:500)]
    public string VideoUrl { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}