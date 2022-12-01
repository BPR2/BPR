using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Fields
{
    public interface IFieldService
    {
        Task<List<Field>> GetAllFieldsByUserId(int? userId);
        void SetField(Field? field);
        Field? GetField();
        Task UpdateField(int FieldId, string FieldName, string FieldDescription, int? FieldPawLevel, string SerialNumber, string unassignReceiver);
        Task<string> UnassignReceiver(string receiverSerialNumber);
        Task<string> CreateFieldAsync(Field field);
        Task<Field> GetLatestFieldFromUser(int userId);
        Task<Field> GetLatestFieldFromUser(string fieldName, string description, int pawLevelLimit);
        Task<string> RemoveField(int fieldId);
    }
}
