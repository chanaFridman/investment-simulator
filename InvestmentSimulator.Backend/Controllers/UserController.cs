using InvestmentSimulator.Backend.Models.DTOs;
using InvestmentSimulator.Backend.Models.Requests;
using InvestmentSimulator.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentSimulator.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public ActionResult<UserStateDto> Login([FromBody] LoginRequest request)
    {
        var user = _userService.CreateOrGetUser(request.Name);

        return Ok(new UserStateDto
        (
            user.Id,
            user.Name,
            user.Balance
        ));
    }

    [HttpGet("{userId}/balance")]
    public ActionResult<decimal> GetBalance(string userId)
    {
        var user = _userService.GetUser(userId);
        return user != null ? Ok(user.Balance) : NotFound();
    }
}
