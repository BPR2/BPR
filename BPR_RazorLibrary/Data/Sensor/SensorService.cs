
namespace BPR_RazorLibrary.Data.Sensor;

public class SensorService : ISensorService
{

#if DEBUG
    string url = "https://localhost:7109/api/Sensor";
#else
       
        string url = "";
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
}
