using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Api.Entities;

public class TelemetryReading
{
    public long Id { get; set; }

    public int SiteId { get; set; }

    public int InverterId { get; set; }

    public DateTime Timestamp { get; set; }

    public double? DcVoltage { get; set; }

    public double? DcCurrent { get; set; }

    public double? AcVoltage { get; set; }

    public double? AcCurrent { get; set; }

    public double PowerKw { get; set; }

    public double EnergyTodayKwh { get; set; }

    public double EnergyTotalKwh { get; set; }

    public double? InverterTemperature { get; set; }

    public InverterStatus? Status { get; set; }

    public Site? Site { get; set; }

    public Inverter? Inverter { get; set; }
}
