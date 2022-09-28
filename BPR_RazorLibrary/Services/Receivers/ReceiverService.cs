using BPR_RazorLibrary.Models;
using System.Text.Json;

namespace BPR_RazorLibrary.Services.Receivers;

public class ReceiverService : IReceiverService
{
#if DEBUG
    string url = "https://localhost:7109/api/Receiver";
#else
       
        string url = "";
#endif

    private HttpClient client;

    public ReceiverService()
    {
        client = new HttpClient();
    }

    public async Task<string> AssignReceiver(string serialNumber, string username)
    {
        HttpResponseMessage message = await client.PostAsync($"{url}/assignReceiver?serialNumber={serialNumber}&username={username}", null);
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

    public async Task<List<Receiver>> GetAllReceivers()
    {
        string message = await client.GetStringAsync($"{url}/allReceivers");
        try
        {
            List<Receiver> result = JsonSerializer.Deserialize<List<Receiver>>(message);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }
}
