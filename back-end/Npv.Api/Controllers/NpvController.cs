using Microsoft.AspNetCore.Mvc;
using Npv.Api.Requests;
using Npv.Api.Services;

namespace Npv.Api.Controllers;

/// <summary>
/// NpvController
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NpvController(INpvCalculatorService npvCalculatorService) : ControllerBase
{
    /// <summary>
    /// calculateNPV
    /// </summary>
    /// <param name="request">NpvRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    [HttpPost("calculateNPV")]
    public async Task<IActionResult> CalculateNPV([FromBody] NpvRequest request, CancellationToken cancellationToken)
    {
        if (request.Increment <= 0)
            return BadRequest("Invalid Increment.");

        if (request.LowerBoundRate < 0)
            return BadRequest("Invalid lower bound rate.");

        if (request.CashFlows == null || !request.CashFlows.Any())
            return BadRequest("Cash flows are required.");

        if (request.LowerBoundRate > request.UpperBoundRate)
            return BadRequest("Lower bound cannot exceed upper bound.");

        var results = await npvCalculatorService.CalculateNpv(
                request.CashFlows,
                request.LowerBoundRate,
                request.UpperBoundRate,
                request.Increment,
                cancellationToken);

        return Ok(results);
    }
}
