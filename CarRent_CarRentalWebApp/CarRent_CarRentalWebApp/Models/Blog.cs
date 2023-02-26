using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent_CarRentalWebApp.Models;
public class Blog
{
    public int Id { get; set; }

    [StringLength(maximumLength: 100)]
    public string? ImageName { get; set; }
    [StringLength(maximumLength: 30)]
    public string Title { get; set; }
    [StringLength(maximumLength: 2000)]
    public string Description { get; set; }
    [StringLength(maximumLength: 50)]
    public string Paragraph { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool isDeleted { get; set; }


    public List<BlogComment>? BlogComments { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}