using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Field;

public interface IFieldService
{
    Task<WebContent> GetAllFieldsByUserId(int userId);
	Task<WebResponse> CreateFieldAsync(BPR_RazorLibrary.Models.Field field);
	Task<WebContent> GetLatestFieldByUserId(int userId);
}
