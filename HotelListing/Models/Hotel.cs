﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Models;

public class Hotel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Address {get; set;}
    public double Rating { get; set;}
    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public Country Country { get; set;}
    
}
