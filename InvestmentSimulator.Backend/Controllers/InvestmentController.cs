using InvestmentSimulator.Backend.Models.DTOs;
using InvestmentSimulator.Backend.Models.Requests;
using InvestmentSimulator.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentSimulator.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvestmentController : ControllerBase
{
    private readonly IInvestmentService _investmentService;

    public InvestmentController(
        IInvestmentService investmentService)
    {
        _investmentService = investmentService;
    }

    [HttpGet("options")]
    public ActionResult<IEnumerable<InvestmentOptionDto>> GetOptions()
    {
        var dtos = _investmentService.GetAvailableOptions().Select(o => new InvestmentOptionDto
        (
            o.Name,
            o.RequiredAmount,
            o.ExpectedReturn,
            o.Duration.TotalSeconds
        ));
        return Ok(dtos);
    }

    [HttpGet("active/{userId}")]
    public ActionResult<IEnumerable<ActiveInvestmentDto>> GetActive(string userId)
    {
        var dtos = _investmentService.GetActiveInvestments(userId).Select(i => new ActiveInvestmentDto
        (
            i.Id,
            i.UserId,
            i.InvestmentName,
            i.AmountInvested,
            i.ExpectedReturn,
            i.StartedAtUtc,
            i.EndsAtUtc
        ));
        return Ok(dtos);
    }

    [HttpPost("invest")]
    public async Task<IActionResult> Invest([FromBody] InvestRequest request)
    {
        try
        {
            await _investmentService.RequestInvestmentAsync(request.UserId, request.InvestmentName);
            return Accepted(new { message = "Investment processing started" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
