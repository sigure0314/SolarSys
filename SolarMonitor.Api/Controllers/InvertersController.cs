using Microsoft.AspNetCore.Mvc;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Services;

namespace SolarMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvertersController : ControllerBase
{
    private readonly InverterService _inverterService;

    public InvertersController(InverterService inverterService)
    {
        _inverterService = inverterService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InverterDto>>> GetInverters([FromQuery] int? siteId, CancellationToken cancellationToken)
    {
        var inverters = await _inverterService.GetInvertersAsync(siteId, cancellationToken);
        return Ok(inverters);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InverterDto>> GetInverter(int id, CancellationToken cancellationToken)
    {
        var inverter = await _inverterService.GetInverterAsync(id, cancellationToken);
        return inverter is null ? NotFound() : Ok(inverter);
    }

    [HttpPost]
    public async Task<ActionResult<InverterDto>> CreateInverter([FromBody] InverterUpsertRequest request, CancellationToken cancellationToken)
    {
        var inverter = await _inverterService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetInverter), new { id = inverter.Id }, inverter);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<InverterDto>> UpdateInverter(int id, [FromBody] InverterUpsertRequest request, CancellationToken cancellationToken)
    {
        var updated = await _inverterService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpGet("{id:int}/history")]
    public async Task<ActionResult<InverterHistoryResponse>> GetHistory(int id, [FromQuery] DateOnly date, CancellationToken cancellationToken)
    {
        var history = await _inverterService.GetHistoryAsync(id, date, cancellationToken);
        return history is null ? NotFound() : Ok(history);
    }
}
