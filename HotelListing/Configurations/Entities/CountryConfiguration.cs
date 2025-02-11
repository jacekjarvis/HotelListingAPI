using HotelListing.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Configurations.Entities;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
            new Country
            {
                Id = 1,
                Name = "Trinidad and Tobago",
                ShortName = "TT"
            },
            new Country
            {
                Id = 2,
                Name = "Jamaica",
                ShortName = "JM"
            },
            new Country
            {
                Id = 3,
                Name = "Aruba",
                ShortName = "AU"
            },
            new Country
            {
                Id = 4,
                Name = "Bahamas",
                ShortName = "BS"
            }
        );
    }
}