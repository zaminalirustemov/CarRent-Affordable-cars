using CarRent_CarRentalWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels
{
    public class OrderViewModel
    {
        public int CarId { get; set; }


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
        public int CardNumber { get; set; }
        [StringLength(maximumLength: 5)]
        public string EndTime { get; set; }
        public int CVC { get; set; }

        public Car? Car { get; set; }

    }
}
