using System.ComponentModel.DataAnnotations;

namespace CarRent_CarRentalWebApp.ViewModels;
public class MemberLoginViewModel
{
    [Required]
    [StringLength(maximumLength: 30)]
    public string UserName { get; set; }

    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}
