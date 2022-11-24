using BPR_RazorLibrary.Models;
using System.Text.Json;

namespace BPR_RazorLibrary.Services.Charts;

public class ChartService : IChartService
{
#if DEBUG
    string url = "https://localhost:7109/api/Chart";
#else

    string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Chart";
#endif

    HttpClient client;

    public ChartService()
    {
        client = new HttpClient();
    }

    public async Task<List<ChartData>> GetAllChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate)
    {
        string message = await client.GetStringAsync($"{url}/getChartDataByFieldId?fieldId={fieldId}&startDate={startDate}&endDate={endDate}");

        try
        {
            WebContent result = JsonSerializer.Deserialize<WebContent>(message);
            var json = JsonSerializer.Serialize(result.content);
            var chartDataList = JsonSerializer.Deserialize<List<ChartData>>(json);
            return chartDataList;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
