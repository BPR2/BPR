using BPR_RazorLibrary.Models;
using Npgsql;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BPR_RazorLibrary.Services.Receivers;

public class ReceiverService : IReceiverService
{
#if DEBUG
    string url = "https://localhost:7109/api/Receiver";
#else
       
        string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Receiver";
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

    public async Task<List<Receiver>> GetReceiversByUserID(int userID)
    {
        string message = await client.GetStringAsync($"{url}/receiver?userID={userID}");
        try
        {
            WebContent result = JsonSerializer.Deserialize<WebContent>(message);

            if (result.response != WebResponse.ContentRetrievalSuccess)
            {
                return null;
            }

            var json = JsonSerializer.Serialize(result.content);

            var contentResult = JsonSerializer.Deserialize<List<Receiver>>(json);

            return contentResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }

    public async Task<string> AssignFieldToReceiver(Receiver receiver)
    {
        string receiverSerialized = JsonSerializer.Serialize(receiver);

        HttpContent content = new StringContent(
                receiverSerialized,
                Encoding.UTF8,
                "application/json"
                );

        string fullurl = $"{url}/assignField";
        var message = await client.PutAsync($"{url}/assignField", content);
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

    public async Task<List<Receiver>> GetAllReceiversList()
    {
        string message = await client.GetStringAsync($"{url}/allReceiversList");
        try
        {
            WebContent result = JsonSerializer.Deserialize<WebContent>(message);

            if (result.response != WebResponse.ContentRetrievalSuccess)
            {
                return null;
            }

            var json = JsonSerializer.Serialize(result.content);

            var contentResult = JsonSerializer.Deserialize<List<Receiver>>(json);

            return contentResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
        return null;
    }

    public async Task<string> UpdateReceiverTimeInterval(int timeInterval, string serialNumber)
    {
        //Fasterholt API
        HttpContent content = new StringContent(
                timeInterval.ToString(),
                Encoding.UTF8,
                "application/json"
                );

        var message = await client.PutAsync($"{url}/updateTimeInterval?serialNumber={serialNumber}", content);

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

    public async Task<Receiver> GetReceiverBySerialNumber(string serialNumber)
    {
        string message = await client.GetStringAsync($"{url}/getReceiverBySerialNumber?serialNumber={serialNumber}");
        try
        {
            WebContent result = JsonSerializer.Deserialize<WebContent>(message);

            if (result.response != WebResponse.ContentRetrievalSuccess)
            {
                return null;
            }

            var json = JsonSerializer.Serialize(result.content);

            var contentResult = JsonSerializer.Deserialize<Receiver>(json);

            return contentResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }
}
