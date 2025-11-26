using System.ComponentModel.DataAnnotations;

namespace SolarMonitor.Api.Entities;

public class Site
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public double? InstalledCapacityKw { get; set; }

    public ICollection<Inverter> Inverters { get; set; } = new List<Inverter>();
}
