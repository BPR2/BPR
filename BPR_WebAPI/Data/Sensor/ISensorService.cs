using BPR_RazorLib.Models;

namespace BPR_WebAPI.Data.Sensor
{
    public interface ISensorService
    {
        Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber);
    }
}
