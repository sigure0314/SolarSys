using SolarMonitor.Api.Entities;

namespace SolarMonitor.Api.Dtos;

public record InverterDto
(
    int Id,
    int SiteId,
    string Name,
    string? Model,
    string? SerialNumber,
    double? RatedPowerKw,
    DateTime? InstallDate,
    InverterStatus Status
);

public record InverterUpsertRequest
(
    int SiteId,
    string Name,
    string? Model,
    string? SerialNumber,
    double? RatedPowerKw,
    DateTime? InstallDate,
    InverterStatus Status
);
