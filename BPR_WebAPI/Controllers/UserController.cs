using BPR_WebAPI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using BPR_RazorLibrary.Models;
using System.Text.Json;

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
		User user = new User
		{
			Username = username,
			Password = password
		};

		var result = await userService.ValidateUser(user);

		return Ok(result);
	}

	[HttpGet("get")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<WebContent>> GetUserByID([FromQuery] int id)
	{
		var result = await userService.GetUserAsync(id);

		return Ok(result.content);
	}

	[HttpPost("createUser")]
	public async Task<ActionResult<WebResponse>> CreateUser([FromBody] User user)
	{
		var result = await userService.CreateUserAsync(user);
		return Ok(result);
	}

	[HttpPut("updateUser")]
	public async Task<ActionResult<WebResponse>> UpdateUser([FromBody] User user)
	{
		var result = await userService.UpdateUserAsync(user);
		return Ok(result);
	}

	[HttpGet("allUsers")]
	public async Task<ActionResult<WebResponse>> GetAllUser()
	{
		var result = await userService.GetAllUsersAsync();
		return Ok(result);
	}
}