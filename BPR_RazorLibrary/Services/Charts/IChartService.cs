using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Charts;

public interface IChartService
{
    Task<List<ChartData>> GetAllChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate);
}
