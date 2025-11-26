using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Entities;
using SolarMonitor.Api.Persistence;

namespace SolarMonitor.Api.Services;

public class InverterService
{
    private readonly SolarDbContext _dbContext;

    public InverterService(SolarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<InverterDto>> GetInvertersAsync(int? siteId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Inverters.AsQueryable();
        if (siteId.HasValue)
        {
            query = query.Where(i => i.SiteId == siteId.Value);
        }

        var inverters = await query.ToListAsync(cancellationToken);
        return inverters.Select(ToDto);
    }

    public async Task<InverterDto?> GetInverterAsync(int id, CancellationToken cancellationToken = default)
    {
        var inverter = await _dbContext.Inverters.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        return inverter is null ? null : ToDto(inverter);
    }

    public async Task<InverterDto> CreateAsync(InverterUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var inverter = new Inverter
        {
            SiteId = request.SiteId,
            Name = request.Name,
            Model = request.Model,
            SerialNumber = request.SerialNumber,
            RatedPowerKw = request.RatedPowerKw,
            InstallDate = request.InstallDate,
            Status = request.Status
        };

        _dbContext.Inverters.Add(inverter);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(inverter);
    }

    public async Task<InverterDto?> UpdateAsync(int id, InverterUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var inverter = await _dbContext.Inverters.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        if (inverter is null)
        {
            return null;
        }

        inverter.SiteId = request.SiteId;
        inverter.Name = request.Name;
        inverter.Model = request.Model;
        inverter.SerialNumber = request.SerialNumber;
        inverter.RatedPowerKw = request.RatedPowerKw;
        inverter.InstallDate = request.InstallDate;
        inverter.Status = request.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(inverter);
    }

    public async Task<InverterHistoryResponse?> GetHistoryAsync(int id, DateOnly date, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Inverters.AnyAsync(i => i.Id == id, cancellationToken);
        if (!exists)
        {
            return null;
        }

        var start = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        var readings = await _dbContext.TelemetryReadings
            .Where(t => t.InverterId == id && t.Timestamp >= start && t.Timestamp <= end)
            .OrderBy(t => t.Timestamp)
            .ToListAsync(cancellationToken);

        var points = readings.Select(r => new TelemetryPointDto(
            r.Timestamp,
            r.DcVoltage,
            r.DcCurrent,
            r.AcVoltage,
            r.AcCurrent,
            r.PowerKw,
            r.EnergyTodayKwh,
            r.EnergyTotalKwh,
            r.InverterTemperature,
            r.Status
        )).ToList();

        return new InverterHistoryResponse(id, date, points);
    }

    private static InverterDto ToDto(Inverter inverter) => new(
        inverter.Id,
        inverter.SiteId,
        inverter.Name,
        inverter.Model,
        inverter.SerialNumber,
        inverter.RatedPowerKw,
        inverter.InstallDate,
        inverter.Status
    );
}
