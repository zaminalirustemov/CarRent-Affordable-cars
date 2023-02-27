using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.ViewModels
{
    public class InfoBarViewModel
    {
        public List<Car> Cars { get; set; }
        public List<BlogComment> blogComments { get; set; }
        public List<CarComment> carComments { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
    }
}
