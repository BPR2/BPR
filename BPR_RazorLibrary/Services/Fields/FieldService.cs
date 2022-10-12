using BPR_RazorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BPR_RazorLibrary.Services.Fields;

public class FieldService : IFieldService
{
#if DEBUG
	string url = "https://localhost:7109/api/Field";
#else
   
    string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/User";
#endif
	HttpClient client;

	public FieldService()
	{
		client = new HttpClient();
	}

	public async Task<List<Field>> GetAllFieldsByUserId(int? userId)
	{
		string message = await client.GetStringAsync($"{url}/getAllFieldsByUserId?userid={userId}");

		try
		{
			WebContent result = JsonSerializer.Deserialize<WebContent>(message);
			var json = JsonSerializer.Serialize(result.content);
			var fields = JsonSerializer.Deserialize<List<Field>>(json);
			return fields;
		}
		catch (Exception)
		{

			throw;
		}
	}

	public async Task<string> CreateFieldAsync(Field field)
	{
		string fieldSerialized = JsonSerializer.Serialize(field);

		HttpContent content = new StringContent(
				fieldSerialized,
				Encoding.UTF8,
				"application/json"
				);

		HttpResponseMessage message = await client.PostAsync($"{url}/createField", content);

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

	public async Task<Field> GetLatestFieldFromUser(int userId)
	{
		string message = await client.GetStringAsync($"{url}/getLatestFieldByUserId?userid={userId}");

		try
		{
			WebContent result = JsonSerializer.Deserialize<WebContent>(message);
			var json = JsonSerializer.Serialize(result.content);
			var field = JsonSerializer.Deserialize<Field>(json);
			return field;
		}
		catch (Exception)
		{
			throw;
		}
	}
}
