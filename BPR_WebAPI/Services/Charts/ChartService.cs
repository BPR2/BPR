using BPR_RazorLibrary.Models;
using BPR_WebAPI.Persistence.Charts;

namespace BPR_WebAPI.Services.Charts;

public class ChartService : IChartService
{
    private IChartRepo chartRepo;
    public ChartService(IConfiguration configuration)
    {
        chartRepo = new ChartRepo(configuration);
    }

    public async Task<WebContent> GetChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate)
    {
        var result = await chartRepo.GetChartDataByFieldId(fieldId, startDate, endDate);

        if (result.response != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }
}
