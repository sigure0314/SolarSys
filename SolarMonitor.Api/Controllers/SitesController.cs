using Microsoft.AspNetCore.Mvc;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Services;

namespace SolarMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SitesController : ControllerBase
{
    private readonly SiteService _siteService;

    public SitesController(SiteService siteService)
    {
        _siteService = siteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SiteDto>>> GetSites(CancellationToken cancellationToken)
    {
        var sites = await _siteService.GetSitesAsync(cancellationToken);
        return Ok(sites);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SiteDto>> GetSite(int id, CancellationToken cancellationToken)
    {
        var site = await _siteService.GetSiteAsync(id, cancellationToken);
        return site is null ? NotFound() : Ok(site);
    }

    [HttpPost]
    public async Task<ActionResult<SiteDto>> CreateSite([FromBody] SiteUpsertRequest request, CancellationToken cancellationToken)
    {
        var site = await _siteService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetSite), new { id = site.Id }, site);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SiteDto>> UpdateSite(int id, [FromBody] SiteUpsertRequest request, CancellationToken cancellationToken)
    {
        var updated = await _siteService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }
}
