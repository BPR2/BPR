using BPR_RazorLibrary.Models;
using BPR_WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BPR_WebAPI_Unittests;

[TestClass]
public class ChartControllerTests
{
    private HttpClient _httpClient;

    public ChartControllerTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task GetChartDataByFieldId_ExistingFieldId_ShouldReturnRetrievalSuccess()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Chart/getChartDataByFieldId?fieldId=1&startDate=2022-11-11%2000%3A00%3A00&endDate=2022-11-12%2000%3A00%3A00");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.ContentRetrievalSuccess, result.response);
    }

    [TestMethod]
    public async Task GetChartDataByFieldId_NonExistingFieldId_ShouldReturnEmptyContent()
    {
        var response = await _httpClient.GetStringAsync("http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Chart/getChartDataByFieldId?fieldId=9999&startDate=2022-11-11%2000%3A00%3A00&endDate=2022-11-12%2000%3A00%3A00");

        WebContent result = JsonSerializer.Deserialize<WebContent>(response);

        Assert.AreEqual(WebResponse.ContentRetrievalSuccess, result.response);
    }

}