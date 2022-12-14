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

    public async Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName, int maxTransmission, int leftTransmission)
    {
        return await receiverRepo.AssignReceiverAsync(serialNumber, userName, maxTransmission, leftTransmission);
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

    public async Task<WebContent> GetAllReceiversList()
    {
        return await receiverRepo.GetAllReceiversList();
    }

    public async Task<WebResponse> UpdateReceiverTimeInterval(int timeInterval, string serialNumber)
    {
        return await receiverRepo.UpdateReceiverTimeInterval(timeInterval, serialNumber);
    }

    public async Task<WebContent> GetReceiverBySerialNumber(string serialNumber)
    {
        return await receiverRepo.GetReceiverBySerialNumber(serialNumber);
    }

    public async Task<WebResponse> UnassignReceiverFromUser(string serialNumber)
    {
        return await receiverRepo.UnassignReceiverFromUser(serialNumber);
    }

    public async Task<WebResponse> UpdateReceiverToUser(string serialNumber, string userName)
    {
        return await receiverRepo.UpdateReceiverToUser(serialNumber, userName);
    }
}

