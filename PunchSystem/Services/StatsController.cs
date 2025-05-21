using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Services;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _stats;

    public StatsController(IStatsService stats)
    {
        _stats = stats;
    }

    [HttpGet("utilisations")]
    public async Task<IActionResult> GetUtilisationStats()
    {
        var data = await _stats.GetUtilisationsStatsAsync();
        return Ok(data);
    }

    [HttpGet("etat-poincons")]
    public async Task<IActionResult> GetEtatPoincons()
    {
        var data = await _stats.GetEtatPoinconsAsync();
        return Ok(data);
    }

    [HttpGet("global")]
    public async Task<IActionResult> GetGlobalStats()
    {
        var data = await _stats.GetGlobalStatsAsync();
        return Ok(data);
    }
}
