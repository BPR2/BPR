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

    public async Task<WebResponse> UnassignReceiver(int fieldId, int receiverId)
    {
        var result = await fieldRepo.UnassignReceiver(fieldId, receiverId);

        if (result != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }

    public async Task<WebContent> UpdateFieldAsync(BPR_RazorLibrary.Models.Field field)
    {
        var result = await fieldRepo.UpdateField(field);

        if (result.response != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }
}
