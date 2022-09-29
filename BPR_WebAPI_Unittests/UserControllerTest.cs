using BPR_RazorLibrary.Models;
using BPR_WebAPI.Controllers;
using BPR_WebAPI.Services.Accounts;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BPR_WebAPI_Unittests;

public class UserControllerTest
{
	private readonly Mock<IAccountService> _mockService;
	private readonly AccountController _controller;

	public UserControllerTest()
	{
		_mockService = new Mock<IAccountService>();
		_controller = new AccountController(_mockService.Object);
	}

	[Fact]
	public async Task CreateAccount_ReturnsSuccess()
	{
		var account = new Account(0, "asdf", "1234", "foo", "bar", "arf@arf.arf", "somewhere");
		_mockService.Setup(v => v.CreateAccountAsync(account).Result).Returns(WebResponse.ContentCreateSuccess);

		var result = await _controller.CreateAccount(account);

		var resultCast = (OkObjectResult)result.Result;

		var testResponse = Assert.IsType<WebResponse>(resultCast.Value);

		Assert.Equal(WebResponse.ContentCreateSuccess, testResponse);
	}
}
