
namespace BPR_RazorLibrary.Data.Sensor;

public interface ISensorService
{
    Task<string> AddNewSensor(string tagNumber, string serialNumber);
}
