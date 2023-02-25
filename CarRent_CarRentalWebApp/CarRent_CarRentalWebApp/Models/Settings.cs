using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class Settings
{
    public int Id { get; set; }
    [StringLength(maximumLength:50)]
    public string Key { get; set; }
    [StringLength(maximumLength:500)]
    public string Value { get; set; }
}