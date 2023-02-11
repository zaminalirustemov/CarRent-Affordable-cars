using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.Models;
public class OrderItem
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int OrderId { get; set; }

    [StringLength(maximumLength:200)]
    public string Model { get; set; }
    [StringLength(maximumLength:100)]
    public string Brand { get; set; }
    public int Millage { get; set; }
    public double PricePerDay { get; set; }
    public double PricePerMonth { get; set; }

    public Car? Car { get; set; }
    public Order? Order { get; set; }

}
