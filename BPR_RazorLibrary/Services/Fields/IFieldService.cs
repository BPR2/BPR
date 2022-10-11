using BPR_RazorLibrary.Models;

namespace BPR_RazorLibrary.Services.Fields
{
    public interface IFieldService
    {
        Task<List<Field>> GetAllFieldsByUserId(int? userId);
    }
}
