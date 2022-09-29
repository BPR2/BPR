using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Sensor;

public interface ISensorService
{
    Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber);
}
