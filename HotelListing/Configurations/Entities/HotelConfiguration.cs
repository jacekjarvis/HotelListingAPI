using HotelListing.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Configurations.Entities;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
            new Hotel
            {
                Id = 1,
                Name = "Hilton",
                Address = "Port of Spain",
                CountryId = 1,
                Rating = 4
            },
            new Hotel
            {
                Id = 2,
                Name = "Sandals Resort and Spa",
                Address = "Negril",
                CountryId = 2,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 3,
                Name = "Comfort Suites",
                Address = "GorgeTown",
                CountryId = 3,
                Rating = 3
            },
            new Hotel
            {
                Id = 4,
                Name = "Hyatt",
                Address = "Nassau",
                CountryId = 4,
                Rating = 4
            }
        );
    }
}