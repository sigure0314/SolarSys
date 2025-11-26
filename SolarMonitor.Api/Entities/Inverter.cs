using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Api.Entities;

public class Inverter
{
    public int Id { get; set; }

    [ForeignKey(nameof(Site))]
    public int SiteId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Model { get; set; }

    [MaxLength(200)]
    public string? SerialNumber { get; set; }

    public double? RatedPowerKw { get; set; }

    public DateTime? InstallDate { get; set; }

    public InverterStatus Status { get; set; } = InverterStatus.Normal;

    public Site? Site { get; set; }

    public ICollection<TelemetryReading> TelemetryReadings { get; set; } = new List<TelemetryReading>();

    public ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();
}
