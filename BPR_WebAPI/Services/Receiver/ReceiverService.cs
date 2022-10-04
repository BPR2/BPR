using BPR_RazorLibrary.Models;
using BPR_WebAPI.Persistence.Receivers;

namespace BPR_WebAPI.Services.Receiver;

public class ReceiverService : IReceiverService
{
    private IReceiverRepo receiverRepo;
    public ReceiverService(IConfiguration configuration)
    {
        receiverRepo = new ReceiverRepo(configuration);
    }

    public async Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName)
    {
        return await receiverRepo.AssignReceiverAsync(serialNumber,userName);
    }

    public async Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceiversAsync()
    {
        return await receiverRepo.GetAllReceivers();
    }

    public async Task<WebContent> GetReceiversByUserID(int userID)
    {
        return await receiverRepo.GetReceiversByUserID(userID);
	}

	public async Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID)
	{
		return await receiverRepo.AssignFieldToReceiver(receiverID, fieldID);
	}
}
