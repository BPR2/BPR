using BPR_RazorLibrary.Models;
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

	public async Task<WebResponse> CreateFieldAsync(BPR_RazorLibrary.Models.Field field)
	{
		return await fieldRepo.CreateFieldAsync(field);
	}

	public async Task<WebContent> GetLatestFieldByUserId(int userId)
    {
        return await fieldRepo.GetLatestFieldByUserId(userId);
    }

    public async Task<WebResponse> UnassignReceiver(string receiverSerialNumber)
    {
        var result = await fieldRepo.UnassignReceiver(receiverSerialNumber);

        if (result != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }

    public async Task<WebResponse> RemoveFieldFromUser(int fieldId)
    {
        var result = await fieldRepo.RemoveFieldFromUser(fieldId);

        if (result != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }

    public async Task<WebContent> UpdateField(int FieldId, string FieldName, string FieldDescription, int FieldPawLevel, string SerialNumber, string unassignReceiver)
    {
        var result = await fieldRepo.UpdateField(FieldId,FieldName,FieldDescription,FieldPawLevel,SerialNumber, unassignReceiver);

        if (result.response != WebResponse.ContentRetrievalSuccess) return result;

        return result;
    }

    public async Task<WebContent> GetLatestFieldByUser(string fieldName, string description, int pawLevelLimit)
    {
        return await fieldRepo.GetLatestFieldByUser(fieldName,description,pawLevelLimit);
    }
}
