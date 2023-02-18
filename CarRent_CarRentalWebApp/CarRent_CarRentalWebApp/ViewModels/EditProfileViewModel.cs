using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels;
public class EditProfileViewModel
{
    [StringLength(maximumLength: 100)]
    public string ImageName { get; set; }
    [StringLength(maximumLength: 30)]
    public string Fullname { get; set; }
    [StringLength(maximumLength: 99), DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [StringLength(maximumLength: 70)]
    public string PhoneNumber { get; set; }
}
