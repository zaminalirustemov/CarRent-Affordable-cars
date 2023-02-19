using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Service
{
    public int Id { get; set; }
    [StringLength(maximumLength: 20)]
    public string IconKeyword { get; set; }
    [StringLength(maximumLength: 25)]
    public string Title { get; set; }
    [StringLength(maximumLength: 50)]
    public string Description { get; set; }
    public bool isDeleted { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}