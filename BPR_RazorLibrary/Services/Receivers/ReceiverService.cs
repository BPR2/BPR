using BPR_RazorLibrary.Models;
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

	public async Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID)
	{
		string message = await client.GetStringAsync($"{url}/assignfield?receiverID={receiverID}&fieldID={fieldID}");
		try
		{
			WebResponse result = JsonSerializer.Deserialize<WebResponse>(message); //not sure it's possible to serialize&deserialize an enum alone, test it
			return result;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.StackTrace);
			return WebResponse.ContentDataCorrupted;
		}
	}
}
