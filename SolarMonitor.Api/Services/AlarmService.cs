using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Persistence;

namespace SolarMonitor.Api.Services;

public class AlarmService
{
    private readonly SolarDbContext _dbContext;

    public AlarmService(SolarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AlarmDto>> GetAlarmsAsync(int? siteId, bool onlyActive, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Alarms.AsQueryable();

        if (siteId.HasValue)
        {
            query = query.Where(a => a.SiteId == siteId.Value);
        }

        if (onlyActive)
        {
            query = query.Where(a => !a.IsResolved);
        }

        var alarms = await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return alarms.Select(ToDto);
    }

    public async Task<AlarmDto?> ResolveAsync(long id, CancellationToken cancellationToken = default)
    {
        var alarm = await _dbContext.Alarms.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (alarm is null)
        {
            return null;
        }

        alarm.IsResolved = true;
        alarm.ResolvedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(alarm);
    }

    private static AlarmDto ToDto(Entities.Alarm alarm) => new(
        alarm.Id,
        alarm.SiteId,
        alarm.InverterId,
        alarm.Level,
        alarm.Code,
        alarm.Message,
        alarm.CreatedAt,
        alarm.ResolvedAt,
        alarm.IsResolved
    );
}
