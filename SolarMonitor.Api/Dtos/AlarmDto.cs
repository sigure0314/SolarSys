using SolarMonitor.Api.Entities;

namespace SolarMonitor.Api.Dtos;

public record AlarmDto
(
    long Id,
    int SiteId,
    int InverterId,
    AlarmLevel Level,
    string? Code,
    string Message,
    DateTime CreatedAt,
    DateTime? ResolvedAt,
    bool IsResolved
);
