using BPR_RazorLibrary.Models;
using BPR_WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BPR_WebAPI_Unittests;

[TestClass]
public class FieldControllerTests
{
    private HttpClient _httpClient;

    public FieldControllerTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task GetAllFieldsByUserId_ExistingUserId_ShouldReturnFieldList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getAllFieldsByUserId?userId=2");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.ContentRetrievalSuccess, result.response);
    }

    [TestMethod]
    public async Task GetAllFieldsByUserId_ExistingUserIdWithNoFields_ShouldReturnEmptyFieldList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getAllFieldsByUserId?userId=1");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<List<Field>>(json);

        Assert.IsTrue(contentResult.Count == 0);
    }

    [TestMethod]
    public async Task GetAllFieldsByUserId_NonExistingUserId_ShouldReturnEmptyFieldList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getAllFieldsByUserId?userId=9999");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<List<Field>>(json);

        Assert.IsTrue(contentResult.Count == 0);
    }

    [TestMethod]
    public async Task GetFieldByUserId_ExistingUserId_ShouldReturnLastAddedField()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getLatestFieldByUserId?userId=2");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.ContentCreateSuccess, result.response);
    }

    [TestMethod]
    public async Task GetFieldByUserId_ExistingUserIdWithNoFields_ShouldReturnNullField()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getLatestFieldByUserId?userId=1");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<Field>(json);

        Assert.IsTrue(contentResult.Id == 0);
    }

    [TestMethod]
    public async Task GetFieldByUserId_NonExistingUserI_ShouldReturnNullField()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field/getLatestFieldByUserId?userId=9999");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<Field>(json);

        Assert.IsTrue(contentResult.Id == 0);
    }
}
