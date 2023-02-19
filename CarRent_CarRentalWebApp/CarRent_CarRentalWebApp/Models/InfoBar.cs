using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class InfoBar
{
    public int Id { get; set; }
    [StringLength(maximumLength: 20)]
    public string Key { get; set; }
    public double Value { get; set; }
    public DateTime? UpdatedDate { get; set; }  
}