using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs;

public class CountryDTO : CreateCountryDTO
{
    public int Id { get; set; }
    public IEnumerable<HotelDTO> Hotels { get; set; } //This doesn't need to be Virtual since  its just a map to the Hotel


}

public class CreateCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country Name Is Too Long")]
    public string Name { get; set; }

    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short Country Name Is Too Long")]
    public string ShortName { get; set; }
}

public class UpdateCountryDTO : CreateCountryDTO
{

}


