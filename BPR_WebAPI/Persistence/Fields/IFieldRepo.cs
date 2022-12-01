using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Fields
{
    public interface IFieldRepo
    {
        Task<WebContent> GetAllFieldsByUserId(int userId);
        Task<WebResponse> UnassignReceiver(string receiverSerialNumber);
        Task<WebContent> UpdateField(int FieldId, string FieldName, string FieldDescription, int FieldPawLevel, string SerialNumber, string unassignReceiver);
        Task<WebResponse> CreateFieldAsync(Field field);
        Task<WebContent> GetLatestFieldByUserId(int userId);
        Task<WebResponse> RemoveFieldFromUser(int fieldId);
    }
}
