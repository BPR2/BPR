namespace BPR_RazorLibrary.Services.Sensor;

public interface ISensorService
{
    Task<string> AddNewSensor(string tagNumber, string serialNumber);
    Task<string> UpdateSensor(string tagNumber, string serialNumber);
    Task<string> UpdateSensorDescription(string tagNumber, string description);
    Task<string> UnassignSensor(string tagNumber);
}
