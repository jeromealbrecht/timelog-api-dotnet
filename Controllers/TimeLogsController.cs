using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimeLog.Models;
using TimeLog.Services;

namespace TimeLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeLogsController : ControllerBase
{
    private readonly TimeLogService _timeLogService;

    public TimeLogsController(TimeLogService timeLogService)
    {
        _timeLogService = timeLogService;
    }

    [HttpGet]
    public ActionResult<List<Models.TimeLog>> Get() => _timeLogService.Get();

    [HttpGet("{id}")]
    public ActionResult<Models.TimeLog> Get(string id)
    {
        var log = _timeLogService.Get(id);
        if (log == null) return NotFound();
        return log;
    }

    [HttpPost]
    public ActionResult<Models.TimeLog> Create(Models.TimeLog log)
    {
        _timeLogService.Create(log);
        return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, Models.TimeLog log)
    {
        var existing = _timeLogService.Get(id);
        if (existing == null) return NotFound();
        _timeLogService.Update(id, log);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existing = _timeLogService.Get(id);
        if (existing == null) return NotFound();
        _timeLogService.Remove(id);
        return NoContent();
    }
}
