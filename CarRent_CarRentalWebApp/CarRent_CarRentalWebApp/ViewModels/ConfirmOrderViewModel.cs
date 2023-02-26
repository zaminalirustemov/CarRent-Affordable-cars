using CarRent_CarRentalWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels
{
    public class ConfirmOrderViewModel
    {
        public int OrderId { get; set; }
        public int CarId { get; set; }


        [StringLength(maximumLength: 100)]
        public string Fullname { get; set; }
        public int Day { get; set; }
        public double TotalPrice { get; set; }

        //
        [StringLength(maximumLength: 25)]
        public string Phonenumber { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [StringLength(maximumLength: 150)]
        public string DropOffLocation { get; set; }
        public DateTime PickUp { get; set; }
        public DateTime DropOff { get; set; }
        public TimeSpan PickUpTime { get; set; }
        public string CardNumber { get; set; }
        [StringLength(maximumLength: 5)]
        public string EndTime { get; set; }
        public string CVC { get; set; }
        //


        public Order? Order { get; set; }
        public Car? Car { get; set; }
    }
}
