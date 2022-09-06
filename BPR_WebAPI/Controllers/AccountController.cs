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
		public async Task<ActionResult<Account>> ValidateUser([FromQuery] string email, [FromQuery] string password)
		{
			if (email == string.Empty || password == string.Empty)
			{
				return BadRequest();
			}

			Account user = new Account
			{
				Email = email,
				Password = password
			};

			Account validatedAccount = await accountService.ValidateAccount(user);

			if (validatedAccount == null)
			{
				return Ok(null);
			}
			else
			{
				return Ok(validatedAccount);
			}
		}

		[HttpGet("get")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Account>> GetUserByID([FromQuery] int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			Account account = await accountService.GetAccountAsync(id);

			return Ok(account);
		}

		[HttpPost("createAccount")]
		public async Task<ActionResult> CreateAccount([FromBody] Account user)
		{
			await accountService.CreateAccountAsync(user);
			return Ok();
		}

		[HttpPut("updateAccount")]
		public async Task<ActionResult> UpdateAccount([FromBody] Account user)
		{
			await accountService.UpdateAccountAsync(user);
			return Ok();
		}
	}
}
