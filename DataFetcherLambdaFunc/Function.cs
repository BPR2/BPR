using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataFetcherLambdaFunc.Models;
using Npgsql;
using System.Net.Http.Headers;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DataFetcherLambdaFunc;

public class Function
{
    private List<string> _serialNumbers;
    private string _dbConnectionString = "Host=localhost;Username=bpr_group4;Password=dingdong420 ;Database=BPR";
    private HttpClient _client;

    public Function()
    {
        _serialNumbers = new List<string>();
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UHm7Hfnwkswp8RwZmZuNr6r3l-0-NgSe583VuUA9VVFvM3Xq2Zak8agJO_EL9zo1aDSb-iXiPl58H6TU-IadNJcdnn1ox13LBsLU_6Y2R8YMX-DmTrMXJFjXPpEvCC9y-nE6ZqWqlYycZL0WNxNL8VFTCKZcHiwXzR4_wRaSX5po6BWkXpsWIPVaDVlMrgB5T9xjr2XUMnJfiwqdtRV7xBcvFSZW82AalFXhqWkrRgaIh4izVMK2G5Ni2eJxtYEl2dXznHX16H1LBc_vHTIww_XAEU_zY-9axAwHkcuQk4jECjtF7cP5jHjDhEDG7IwmqxZ2IcbVe-jLiRnUs7QzVk7-gBKa3loEj3FBJPmER6fFSxKBHAuxe5ZU4aqALRs1VFOz6uqtcNFjYlp9x93_J4riQ7r84JlZFlpeoRqAX3jrFTpROACRiSwbSR4gr27i8BA4EUprjCyBjC5qaIdCs4o3xr-AtJ7bZIHhxE4Pqz_ajtXEPAfITbRsGs7MuFWL");
    }

    public async Task FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        await GetAllSerialNumbers();
    }

    public async Task GetAllSerialNumbers()
    {
        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"SELECT \"serialnumber\" FROM public.\"receiver\"";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    _serialNumbers.Add(reader["serialnumber"].ToString());
                }
        }
        con.Close();

        await GetDataFromAllReceivers();
    }


    public async Task GetDataFromAllReceivers()
    {
        string dateTime = DateTime.Now.AddHours(-1).ToString();
        //string dateTime = "09/14/2022 01:41:25 AM";


        //Receiver data (position)
        string message = await _client.GetStringAsync($"https://api.trusted.dk/api/Positions/AllSince?AfterDate={dateTime}");
        try
        {
            List<ReceiverDataModel> resultReceiverDataList = JsonSerializer.Deserialize<List<ReceiverDataModel>>(message);
            await InsertReceiverDataToDB(resultReceiverDataList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }


        //Sensor Data
        foreach (var serialNumber in _serialNumbers)
        {
            message = await _client.GetStringAsync($"https://api.trusted.dk/api/SensorData/GetSensorTagData?SerialNumber={serialNumber}&AfterDate={dateTime}");
            try
            {
                List<SensorDataModel> resultSensorDataList = JsonSerializer.Deserialize<List<SensorDataModel>>(message);
                await InsertSensorDataToDB(resultSensorDataList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            Thread.Sleep(61000);
        }
    }


    public async Task InsertReceiverDataToDB(List<ReceiverDataModel> receiverData)
    {
        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"INSERT INTO public.\"receiverdata\"(\"receiverid\", \"timestamp\", \"longitude\", \"latitude\") VALUES (@receiverid, @timestamp, @longitude, @latitude);";

        foreach (var item in receiverData)
        {
            int receiverId = await GetReceiverId(item.SerialNumber);

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@receiverid", receiverId);
                cmd.Parameters.AddWithValue("@timestamp", item.position.Timestamp);
                cmd.Parameters.AddWithValue("@longitude", item.position.Longitude);
                cmd.Parameters.AddWithValue("@latitude", item.position.Latitude);

                cmd.ExecuteNonQuery();
            }
        }
        con.Close();
    }

    public async Task<int> GetReceiverId(string receiverSerialNumber)
    {
        int receiverId = 0;

        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"SELECT \"receiverid\" FROM public.\"receiver\" WHERE \"serialnumber\"=@serialnumber; ";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            cmd.Parameters.AddWithValue("@serialnumber", receiverSerialNumber);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    receiverId = (int)reader["receiverid"];
                }
        }
        con.Close();

        return receiverId;
    }


    public async Task InsertSensorDataToDB(List<SensorDataModel> sensorData)
    {
        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"INSERT INTO public.\"sensormeasurement\"(\"sensorid\", \"timestamp\", \"temperature\", \"humidity\") VALUES (@sensorid, @timestamp, @temperature, @humidity);";

        foreach (var item in sensorData)
        {
            int sensorId = await GetSensorId(item.TagNumber);

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@sensorid", sensorId);
                cmd.Parameters.AddWithValue("@timestamp", item.Timestamp);
                cmd.Parameters.AddWithValue("@temperature", Math.Round(item.Temperature, 2));
                cmd.Parameters.AddWithValue("@humidity", Math.Round(item.Humidity, 2));

                cmd.ExecuteNonQuery();
            }
        }
        con.Close();
    }

    public async Task<int> GetSensorId(string sensorTagNumber)
    {
        int sensorId = 0;

        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"SELECT \"sensorid\" FROM public.\"sensor\" WHERE \"tagnumber\"=@sensorTagNumber; ";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            cmd.Parameters.AddWithValue("@sensorTagNumber", sensorTagNumber);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    sensorId = (int)reader["sensorid"];
                }
        }
        con.Close();

        return sensorId;
    }

}
