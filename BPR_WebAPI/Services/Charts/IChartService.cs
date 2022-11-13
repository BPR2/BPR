using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Charts;

public interface IChartService
{
    Task<WebContent> GetChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate);
}
