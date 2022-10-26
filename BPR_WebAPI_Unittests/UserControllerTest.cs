using BPR_RazorLibrary.Models;
using BPR_WebAPI;
using BPR_WebAPI.Controllers;
using BPR_WebAPI.Services.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text.Json;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BPR_WebAPI_Unittests;

[TestClass]
public class UserControllerTest
{
	private readonly Mock<IUserService> _mockService;
	private readonly UserController _controller;
    private HttpClient _httpClient;

    public UserControllerTest()
	{
		_mockService = new Mock<IUserService>();
		_controller = new UserController(_mockService.Object);
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

	/*[Fact]
	public async Task CreateAccount_ReturnsSuccess()
	{
		var user = new User { AccountId = 0, Username = "asdf", Password = "1234", FullName = "foo", Contact = "bar", Email = "arf@arf.arf", Address = "somewhere" };
		_mockService.Setup(v => v.CreateUserAsync(user).Result).Returns(WebResponse.ContentCreateSuccess);

		var result = await _controller.CreateUser(user);

		var resultCast = (OkObjectResult)result.Result;

		var testResponse = Assert.IsType<WebResponse>(resultCast.Value);

		Assert.Equal(WebResponse.ContentCreateSuccess, testResponse);
	}*/

    [TestMethod]
    public async Task ValidateUser_ExsitingUsernameOrPassword_AuthenticationShouldSuccess()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User/validate?username=peter&password=peter");
        
        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.AuthenticationSuccess, result.response);
    }

    [TestMethod]
    public async Task ValidateUser_NonExsitingUsernameOrPassword_AuthenticationShouldFail()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User/validate?username=peter&password=pete");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.AuthenticationFailure, result.response);
    }

    [TestMethod]
    public async Task GetUserById_ExsitingUserId_ShouldReturnUser()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User/get?id=2");

        User result = JsonSerializer.Deserialize<User>(response);

        Assert.AreEqual("peter", result.Username);
    }

    [TestMethod]
    public async Task GetUserById_NonExsitingUserId_ShouldNotReturnNoContect()
    {
        var response = await _httpClient.GetAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User/get?id=9999");

        var respons = response.StatusCode;

        Assert.AreEqual(System.Net.HttpStatusCode.NoContent, respons);
    }

    [TestMethod]
    public async Task GetAllUsers_ShouldReturnUserList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User/allUsers");

        List<User>? result = JsonSerializer.Deserialize<List<User>>(response);

        Assert.IsTrue(result.Count > 0);
    }
}
