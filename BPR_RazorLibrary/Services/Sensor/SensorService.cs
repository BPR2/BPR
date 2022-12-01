namespace BPR_RazorLibrary.Services.Sensor;

public class SensorService : ISensorService
{

#if DEBUG
    string url = "https://localhost:7109/api/Sensor";
#else

    string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Sensor";
#endif

    HttpClient client;

    public SensorService()
    {
        client = new HttpClient();
    }

    public async Task<string> AddNewSensor(string tagNumber, string serialNumber)
    {
        HttpResponseMessage message = await client.PostAsync($"{url}/addNewSensor?tagNumber={tagNumber}&serialNumber={serialNumber}", null);
        try
        {
            string result = await message.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }

    public async Task<string> UnassignSensor(string tagNumber)
    {
        HttpResponseMessage message = await client.PutAsync($"{url}/unassignSensor?tagNumber={tagNumber}", null);
        try
        {
            string result = await message.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }

    public async Task<string> UpdateSensor(string tagNumber, string serialNumber)
    {
        HttpResponseMessage message = await client.PutAsync($"{url}/updateSensor?tagNumber={tagNumber}&serialNumber={serialNumber}", null);
        try
        {
            string result = await message.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }

    public async Task<string> UpdateSensorDescription(string tagNumber, string description)
    {
        HttpResponseMessage message = await client.PutAsync($"{url}/updateSensorDescription?tagNumber={tagNumber}&description={description}", null);
        try
        {
            string result = await message.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }
}
