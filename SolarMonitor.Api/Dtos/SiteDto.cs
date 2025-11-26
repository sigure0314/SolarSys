using SolarMonitor.Api.Entities;

namespace SolarMonitor.Api.Dtos;

public record SiteDto
(
    int Id,
    string Name,
    string? Location,
    double? Latitude,
    double? Longitude,
    double? InstalledCapacityKw,
    IEnumerable<InverterDto> Inverters
);

public record SiteUpsertRequest
(
    string Name,
    string? Location,
    double? Latitude,
    double? Longitude,
    double? InstalledCapacityKw
);
