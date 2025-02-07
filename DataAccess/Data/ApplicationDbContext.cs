using Microsoft.EntityFrameworkCore;
using HotelListing.Models; 

namespace HotelListing.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
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

            builder.Entity<Hotel>().HasData(
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
}
