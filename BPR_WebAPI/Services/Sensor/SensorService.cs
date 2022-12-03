using BPR_RazorLibrary.Models;
using BPR_WebAPI.Persistence.Sensors;

namespace BPR_WebAPI.Services.Sensor;

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

    public async Task<WebResponse> UnassignSensorAsync(string tagNumber)
    {
        return await sensorRepo.UnassignSensorAsync(tagNumber);
    }

    public async Task<WebResponse> UpdateSensorAsync(string tagNumber, string serialNumber)
    {
        return await sensorRepo.UpdateSensorAsync(tagNumber, serialNumber);
    }

    public async Task<WebResponse> UpdateSensorDescription(string tagNumber, string description)
    {
        return await sensorRepo.UpdateSensorDescription(tagNumber, description);
    }
}
