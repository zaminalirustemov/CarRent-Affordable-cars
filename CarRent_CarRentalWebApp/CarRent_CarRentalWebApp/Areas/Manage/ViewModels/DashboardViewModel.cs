using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.Areas.Manage.ViewModels
{
    public class DashboardViewModel
    {
        public List<Car> Cars { get; set; }
        public List<BlogComment> blogComments { get; set; }
        public List<CarComment> carComments { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Service> Services { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Order> Orders { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
