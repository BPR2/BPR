using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Field;

public interface IFieldService
{
    Task<WebContent> GetAllFieldsByUserId(int userId);
    Task<WebContent> UpdateField(int FieldId, string FieldName, string FieldDescription, int FieldPawLevel, string SerialNumber, string unassignReceiver);
    Task<WebResponse> UnassignReceiver(string receiverSerialNumber);
    Task<WebResponse> CreateFieldAsync(BPR_RazorLibrary.Models.Field field);
    Task<WebContent> GetLatestFieldByUserId(int userId);
    Task<WebResponse> RemoveFieldFromUser(int fieldId);
}
