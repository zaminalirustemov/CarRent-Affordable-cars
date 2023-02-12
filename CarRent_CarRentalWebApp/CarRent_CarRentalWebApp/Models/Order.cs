using CarRent_CarRentalWebApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class Order
{
    public int Id { get; set; }
    public string? AppUserId { get; set; }
    


    [StringLength(maximumLength: 100)]
    public string Fullname { get; set; }
    [StringLength(maximumLength: 25)]
    public string Phonenumber { get; set; }
    [StringLength(maximumLength: 100)]
    public string Email { get; set; }
    [StringLength(maximumLength: 150)]
    public string DropOffLocation { get; set; }
    public DateTime PickUp { get; set; }
    public DateTime DropOff { get; set; }
    public DateTime PickUpTime { get; set; }
    public int? CardNumber { get; set; }
    [StringLength(maximumLength: 5)]
    public string? EndTime { get; set; }
    public int? CVC { get; set; }
    public int? Day { get; set; }
    public double? TotalPrice { get; set; }
    public bool isDeleted { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public AppUser? AppUser { get; set; }
    public OrderItem? OrderItem { get; set; } = new OrderItem(); 
    [NotMapped]
    public int CarId { get; set; }
    [NotMapped]
    public Car Car { get; set; }
} 
