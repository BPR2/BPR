using BPR_RazorLibrary.Models;
namespace BPR_RazorLibrary.Services.Receivers;

public interface IReceiverService
{
    Task<string> AssignReceiver(string serialNumber, string username, int maxTransmission, int leftTransmission);
    Task<List<Receiver>> GetAllReceivers();
    Task<List<Receiver>> GetAllReceiversList();
    Task<List<Receiver>> GetReceiversByUserID(int userID);
    Task<string> AssignFieldToReceiver(Receiver receiver);
    Task<string> UpdateReceiverTimeInterval(int timeInterval, string serialNumber);
    Task<Receiver> GetReceiverBySerialNumber(string serialNumber);
}
