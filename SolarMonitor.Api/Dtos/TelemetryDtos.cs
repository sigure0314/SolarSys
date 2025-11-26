using SolarMonitor.Api.Entities;

namespace SolarMonitor.Api.Dtos;

public record TelemetryPointDto
(
    DateTime Timestamp,
    double? DcVoltage,
    double? DcCurrent,
    double? AcVoltage,
    double? AcCurrent,
    double PowerKw,
    double EnergyTodayKwh,
    double EnergyTotalKwh,
    double? InverterTemperature,
    InverterStatus? Status
);

public record InverterHistoryResponse
(
    int InverterId,
    DateOnly Date,
    IReadOnlyCollection<TelemetryPointDto> Points
);

public record DashboardSummaryResponse
(
    int TotalSites,
    int TotalInverters,
    double TotalPowerKw,
    double TotalEnergyTodayKwh,
    double TotalEnergyThisMonthKwh,
    int ActiveAlarmsCount
);
