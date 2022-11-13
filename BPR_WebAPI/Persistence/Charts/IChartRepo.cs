using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Charts
{
    public interface IChartRepo
    {
        Task<WebContent> GetChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate);
    }
}
