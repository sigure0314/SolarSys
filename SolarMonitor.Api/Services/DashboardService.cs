using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Persistence;

namespace SolarMonitor.Api.Services;

public class DashboardService
{
    private readonly SolarDbContext _dbContext;

    public DashboardService(SolarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardSummaryResponse> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var totalSites = await _dbContext.Sites.CountAsync(cancellationToken);
        var totalInverters = await _dbContext.Inverters.CountAsync(cancellationToken);

        var latestTelemetryByInverter = await _dbContext.TelemetryReadings
            .GroupBy(t => t.InverterId)
            .Select(g => g.OrderByDescending(x => x.Timestamp).First())
            .ToListAsync(cancellationToken);

        double totalPowerKw = latestTelemetryByInverter.Sum(t => t.PowerKw);
        double totalEnergyTodayKwh = latestTelemetryByInverter.Sum(t => t.EnergyTodayKwh);

        var activeAlarmsCount = await _dbContext.Alarms.CountAsync(a => !a.IsResolved, cancellationToken);

        return new DashboardSummaryResponse(
            totalSites,
            totalInverters,
            totalPowerKw,
            totalEnergyTodayKwh,
            0d,
            activeAlarmsCount
        );
    }
}
