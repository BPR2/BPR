namespace BPR_RazorLibrary.Services.Sensor;

public interface ISensorService
{
    Task<string> AddNewSensor(string tagNumber, string serialNumber);
}
