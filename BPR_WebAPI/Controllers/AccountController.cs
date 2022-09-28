using BPR_RazorLib.Models;
using BPR_WebAPI.Data.Accounts;
using BPR_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BPR_WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private IAccountService accountService;

		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService;
		}

		// GET: api/<AccountController>
		[HttpGet("validate")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<WebContent>> ValidateAccount([FromQuery] string username, [FromQuery] string password)
		{
			Account user = new Account
			{
				Username = username,
				Password = password
			};

			var result = await accountService.ValidateAccount(user);

			return Ok(result.content);
		}

		[HttpGet("get")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<WebContent>> GetUserByID([FromQuery] int id)
		{
			var result = await accountService.GetAccountAsync(id);

			return Ok(result);
		}

		[HttpPost("createAccount")]
		public async Task<ActionResult<WebResponse>> CreateAccount([FromBody] Account user)
		{
			var result = await accountService.CreateAccountAsync(user);
			return Ok(result);
		}

		[HttpPut("updateAccount")]
		public async Task<ActionResult<WebResponse>> UpdateAccount([FromBody] Account user)
		{
			var result = await accountService.UpdateAccountAsync(user);
			return Ok(result);
		}
	}
}
