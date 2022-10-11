using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Sensors;

public interface ISensorRepo
{
    Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber);
    Task<List<Sensor>> getAllSensorsByReceiverId(int receiverId);
}
