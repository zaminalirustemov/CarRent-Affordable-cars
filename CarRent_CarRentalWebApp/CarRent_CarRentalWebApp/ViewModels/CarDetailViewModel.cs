using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.ViewModels;
public class CarDetailViewModel
{
    public Car Car { get; set; }
    public CarComment CarComment { get; set; }
    public List<CarComment> CarComments { get; set; }
}