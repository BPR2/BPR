using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using BPR_WebAPI.Persistence.Fields;

namespace BPR_WebAPI.Services.Field;

public class FieldService : IFieldService
{
    private IFieldRepo fieldRepo;
    public FieldService(IConfiguration configuration)
    {
        fieldRepo = new FieldRepo(configuration);
    }

    public async Task<WebContent> GetAllFieldsByUserId(int userId)
    {
        var result = await fieldRepo.GetAllFieldsByUserId(userId);

        if (result.response != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }
}
