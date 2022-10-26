using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Fields
{
    public interface IFieldService
    {
        Task<List<Field>> GetAllFieldsByUserId(int? userId);
        void SetField(Field? field);
        Field? GetField();
        Task UpdateField(Field field);
        Task<string> UnassignReceiver(int fieldId, int receiverId);
    }
}
