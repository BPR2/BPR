

using BPR_RazorLib.Models;
using BPR_RazorLibrary.Models;
using System.Text.Json;

namespace BPR_RazorLibrary.Data.Receiver;

public class ReceiverService : IReceiverService
{
#if DEBUG
    string url = "https://localhost:7109/api/Receiver";
#else
       
        string url = "";
#endif

    HttpClient client;

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

    public async Task<List<Models.Receiver>> GetAllReceivers()
    {
        string message = await client.GetStringAsync($"{url}/allReceivers");
        try
        {
            List<Models.Receiver> result = JsonSerializer.Deserialize<List<Models.Receiver>>(message);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }
}
