using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Field;

public interface IFieldService
{
    Task<WebContent> GetAllFieldsByUserId(int userId);
}
