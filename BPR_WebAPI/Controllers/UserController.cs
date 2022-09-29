using BPR_WebAPI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using BPR_RazorLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BPR_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private IUserService userService;

	public UserController(IUserService userService)
	{
		this.userService = userService;
	}

	[HttpGet("validate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<WebContent>> ValidateUser([FromQuery] string username, [FromQuery] string password)
	{
		Console.WriteLine("Received login request");
		User user = new User
		{
			Username = username,
			Password = password
		};

		var result = await userService.ValidateAccount(user);

		Console.WriteLine(result.response);

		return Ok(result);
	}

	[HttpGet("get")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<WebContent>> GetUserByID([FromQuery] int id)
	{
		var result = await userService.GetAccountAsync(id);

		return Ok(result);
	}

	[HttpPost("createAccount")]
	public async Task<ActionResult<WebResponse>> CreateAccount([FromBody] User user)
	{
		var result = await userService.CreateAccountAsync(user);
		return Ok(result);
	}

	[HttpPut("updateAccount")]
	public async Task<ActionResult<WebResponse>> UpdateAccount([FromBody] User user)
	{
		var result = await userService.UpdateAccountAsync(user);
		return Ok(result);
	}
}