using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Entities;
using SolarMonitor.Api.Persistence;

namespace SolarMonitor.Api.Services;

public class SiteService
{
    private readonly SolarDbContext _dbContext;

    public SiteService(SolarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SiteDto>> GetSitesAsync(CancellationToken cancellationToken = default)
    {
        var sites = await _dbContext.Sites
            .Include(s => s.Inverters)
            .ToListAsync(cancellationToken);

        return sites.Select(ToDto);
    }

    public async Task<SiteDto?> GetSiteAsync(int id, CancellationToken cancellationToken = default)
    {
        var site = await _dbContext.Sites
            .Include(s => s.Inverters)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return site is null ? null : ToDto(site);
    }

    public async Task<SiteDto> CreateAsync(SiteUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var site = new Site
        {
            Name = request.Name,
            Location = request.Location,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            InstalledCapacityKw = request.InstalledCapacityKw
        };

        _dbContext.Sites.Add(site);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(site);
    }

    public async Task<SiteDto?> UpdateAsync(int id, SiteUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var site = await _dbContext.Sites.Include(s => s.Inverters)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (site is null)
        {
            return null;
        }

        site.Name = request.Name;
        site.Location = request.Location;
        site.Latitude = request.Latitude;
        site.Longitude = request.Longitude;
        site.InstalledCapacityKw = request.InstalledCapacityKw;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(site);
    }

    private static SiteDto ToDto(Site site) => new(
        site.Id,
        site.Name,
        site.Location,
        site.Latitude,
        site.Longitude,
        site.InstalledCapacityKw,
        site.Inverters.Select(i => new InverterDto(
            i.Id,
            i.SiteId,
            i.Name,
            i.Model,
            i.SerialNumber,
            i.RatedPowerKw,
            i.InstallDate,
            i.Status
        ))
    );
}
