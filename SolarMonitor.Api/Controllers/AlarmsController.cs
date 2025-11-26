using Microsoft.AspNetCore.Mvc;
using SolarMonitor.Api.Dtos;
using SolarMonitor.Api.Services;

namespace SolarMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlarmsController : ControllerBase
{
    private readonly AlarmService _alarmService;

    public AlarmsController(AlarmService alarmService)
    {
        _alarmService = alarmService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlarmDto>>> GetAlarms([FromQuery] int? siteId, [FromQuery] bool onlyActive = false, CancellationToken cancellationToken = default)
    {
        var alarms = await _alarmService.GetAlarmsAsync(siteId, onlyActive, cancellationToken);
        return Ok(alarms);
    }

    [HttpPost("{id:long}/resolve")]
    public async Task<ActionResult<AlarmDto>> ResolveAlarm(long id, CancellationToken cancellationToken)
    {
        var alarm = await _alarmService.ResolveAsync(id, cancellationToken);
        return alarm is null ? NotFound() : Ok(alarm);
    }
}
