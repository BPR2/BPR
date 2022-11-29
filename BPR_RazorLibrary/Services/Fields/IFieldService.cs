using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Fields
{
    public interface IFieldService
    {
        Task<List<Field>> GetAllFieldsByUserId(int? userId);
        void SetField(Field? field);
        Field? GetField();
        Task UpdateField(Field field, string receiverSerialNumber);
        Task<string> UnassignReceiver(string receiverSerialNumber);
        Task<string> CreateFieldAsync(Field field);
        Task<Field> GetLatestFieldFromUser(int userId);
        Task<string> RemoveField(int fieldId);
    }
}
