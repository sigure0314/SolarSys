using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Api.Entities;

public class Alarm
{
    public long Id { get; set; }

    public int SiteId { get; set; }

    public int InverterId { get; set; }

    public AlarmLevel Level { get; set; }

    [MaxLength(100)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public bool IsResolved { get; set; }

    public Site? Site { get; set; }

    public Inverter? Inverter { get; set; }
}
