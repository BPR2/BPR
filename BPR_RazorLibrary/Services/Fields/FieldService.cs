using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using Smart.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BPR_RazorLibrary.Services.Fields;

public class FieldService : IFieldService
{
#if DEBUG
    string url = "https://localhost:7109/api/Field";
#else

    string url = "http://fasterholtwebapi-prod.us-east-1.elasticbeanstalk.com/api/Field";
#endif

    HttpClient client;

    private Field? field;

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

    public Field? GetField()
    {
        return field;
    }

    public void SetField(Field? field)
    {
        this.field = field;
    }

    public async Task UpdateField(int FieldId, string FieldName, string FieldDescription, int? FieldPawLevel, string SerialNumber, string unassignReceiver)
    {
        await client.PutAsync($"{url}/updateField?FieldId={FieldId}&FieldName={FieldName}&FieldDescription={FieldDescription}&FieldPawLevel={FieldPawLevel}&SerialNumber={SerialNumber}&unassignReceiver={unassignReceiver}", null);
    }

    public async Task<string> UnassignReceiver(string receiverSerialNumber)
    {
        string fieldIdSerialized = JsonSerializer.Serialize(receiverSerialNumber);
        HttpContent content = new StringContent(
                fieldIdSerialized,
                Encoding.UTF8,
                "application/json"
                );
        
        var message = await client.PutAsync($"{url}/unassignReceiver?receiverSerialNumber={receiverSerialNumber}", content);
        
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

    public async Task<string> RemoveField(int fieldId)
    {
        var message = await client.PutAsync($"{url}/removeField?fieldId={fieldId}", null);

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

    public async Task<Field> GetLatestFieldFromUser(string fieldName, string description, int pawLevelLimit)
    {
        string message = await client.GetStringAsync($"{url}/getLatestFieldByUser?fieldName={fieldName}&description={description}&pawLevelLimit={pawLevelLimit}");

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
