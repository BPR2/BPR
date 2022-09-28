using BPR_RazorLib.Models;

namespace BPR_WebAPI.Persistence.Sensor
{
    public interface ISensorRepo
    {
        Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber);
    }
}
