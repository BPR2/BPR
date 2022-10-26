using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Field;

public interface IFieldService
{
    Task<WebContent> GetAllFieldsByUserId(int userId);
    Task<WebContent> UpdateFieldAsync(BPR_RazorLibrary.Models.Field field);
    Task<WebResponse> UnassignReceiver(int fieldId, int receiverId);
	  Task<WebResponse> CreateFieldAsync(BPR_RazorLibrary.Models.Field field);
	  Task<WebContent> GetLatestFieldByUserId(int userId);
}
