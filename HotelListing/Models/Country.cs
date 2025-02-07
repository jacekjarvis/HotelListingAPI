using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HotelListing.Models;

public class Country
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage ="Country Name Is Too Long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short Country Name Is Too Long")]
    public string ShortName { get; set; }

    public virtual IEnumerable<Hotel> Hotels { get; set; }
}
