using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.ViewModels;
public class BlogDetailViewModel
{
    public Blog Blog { get; set; }
    public BlogComment BlogComment { get; set; }
    public List<BlogComment> BlogComments { get; set; }
}