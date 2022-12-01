using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Sensor;

public interface ISensorService
{
    Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber);
    Task<WebResponse> UpdateSensorAsync(string tagNumber, string serialNumber);
    Task<WebResponse> UpdateSensorDescription(string tagNumber, string description);
    Task<WebResponse> UnassignSensorAsync(string tagNumber);
}
