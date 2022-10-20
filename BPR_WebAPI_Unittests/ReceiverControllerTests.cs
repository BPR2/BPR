using BPR_RazorLibrary.Models;
using BPR_WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BPR_WebAPI_Unittests;

[TestClass]
public class ReceiverControllerTests
{
    private HttpClient _httpClient;

    public ReceiverControllerTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task GetAllReceivers_ShouldReturnReceiverList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Receiver/allReceivers");

        List<Receiver>? result = JsonSerializer.Deserialize<List<Receiver>>(response);

        Assert.IsTrue(result.Count > 0);
    }

    [TestMethod]
    public async Task GetReceiverByUserId_ExistingUserId_ShouldReturnReceivers()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Receiver/receiver?userID=2");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.ContentRetrievalSuccess, result.response);
    }

    [TestMethod]
    public async Task GetReceiverByUserId_ExistingUserIdWithNoReceiversAssigned_ShouldReturnEmptyReceiverList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Receiver/receiver?userID=1");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<List<Receiver>>(json);

        Assert.IsTrue(contentResult.Count == 0);
    }

    [TestMethod]
    public async Task GetReceiverByUserId_NonExistingUserId_ShouldReturnEmptyReceiverList()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Receiver/receiver?userID=9999");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        var json = JsonSerializer.Serialize(result.content);

        var contentResult = JsonSerializer.Deserialize<List<Receiver>>(json);

        Assert.IsTrue(contentResult.Count == 0);
    }
}
