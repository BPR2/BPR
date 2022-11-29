namespace BPR_RazorLibrary.Services.Sensor;

public interface ISensorService
{
    Task<string> AddNewSensor(string tagNumber, string serialNumber);
    Task<string> UpdateSensor(string tagNumber, string serialNumber);
    Task<string> UnassignSensor(string tagNumber);
}
