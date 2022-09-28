using BPR_RazorLib.Models;
using BPR_WebAPI.Persistence.Sensor;

namespace BPR_WebAPI.Data.Sensor;

public class SensorService : ISensorService
{
    private ISensorRepo sensorRepo;
    public SensorService(IConfiguration configuration)
    {
        sensorRepo = new SensorRepo(configuration);
    }

    public async Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber)
    {
       return await sensorRepo.AddNewSensorAsync(tagNumber, serialNumber);
    }
}
