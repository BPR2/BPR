using BPR_RazorLibrary.Models;
using BPR_WebAPI.Controllers;
using BPR_WebAPI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BPR_WebAPI_Unittests;

public class UserControllerTest
{
	private readonly Mock<IUserService> _mockService;
	private readonly UserController _controller;

	public UserControllerTest()
	{
		_mockService = new Mock<IUserService>();
		_controller = new UserController(_mockService.Object);
	}

	[Fact]
	public async Task CreateAccount_ReturnsSuccess()
	{
		var user = new User { AccountId = 0, Username = "asdf", Password = "1234", FullName = "foo", Contact = "bar", Email = "arf@arf.arf", Address = "somewhere" };
		_mockService.Setup(v => v.CreateUserAsync(user).Result).Returns(WebResponse.ContentCreateSuccess);

		var result = await _controller.CreateUser(user);

		var resultCast = (OkObjectResult)result.Result;

		var testResponse = Assert.IsType<WebResponse>(resultCast.Value);

		Assert.Equal(WebResponse.ContentCreateSuccess, testResponse);
	}
}
