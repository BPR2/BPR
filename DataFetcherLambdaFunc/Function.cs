using Amazon.Lambda.Core;
using DataFetcherLambdaFunc.Models;
using Npgsql;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DataFetcherLambdaFunc;

public class Function
{
    private List<string> _serialNumbers;
    private string _dbConnectionString = "Host=bpr-db.c7szkct1z4j9.us-east-1.rds.amazonaws.com;Username=bpr_group4;Password=dingdong420 ;Database=postgres";
    private HttpClient _client;
    private string _dateTime;

    public Function()
    {
        _serialNumbers = new List<string>();
        _client = new HttpClient();
        _client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
        _dateTime = DateTime.Now.AddMinutes(-30).ToString();
    }

    public async Task FunctionHandler()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetBearerToken());

        await GetReceiversFromTrustedAPI();
        await GetAllSerialNumbers();
    }

    public async Task GetAllSerialNumbers()
    {
        List<ReceiverDataModel> resultReceiverDataList = new List<ReceiverDataModel>();

        string message = await _client.GetStringAsync($"https://api.trusted.dk/api/SensorData/AllSince?AfterDate={_dateTime}");
        try
        {
            resultReceiverDataList = JsonSerializer.Deserialize<List<ReceiverDataModel>>(message);
            foreach (var item in resultReceiverDataList)
            {
                if (!_serialNumbers.Contains(item.SerialNumber))
                {
                    _serialNumbers.Add(item.SerialNumber);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }

        await GetDataFromAllReceivers(resultReceiverDataList);
    }

    public async Task GetDataFromAllReceivers(List<ReceiverDataModel> resultReceiverDataList)
    {
        //Receiver data (position)
        await InsertReceiverDataToDB(resultReceiverDataList);

        //Sensor Data
        foreach (var serialNumber in _serialNumbers)
        {
            string message = await _client.GetStringAsync($"https://api.trusted.dk/api/SensorData/GetSensorTagData?SerialNumber={serialNumber}&AfterDate={_dateTime}");
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

    public async Task<string> GetBearerToken()
    {
        string bearerToken = "";

        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"SELECT \"token\" FROM public.\"bearerToken\"; ";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    bearerToken = reader["token"].ToString();
                }
        }
        con.Close();

        return bearerToken;
    }

    //Update Time interval for receivers

    public async Task<List<ReceiverDataModel>> GetReceiversFromDB()
    {
        List<ReceiverDataModel> receivers = new List<ReceiverDataModel>();

        try
        {
            using var con = new NpgsqlConnection(_dbConnectionString);
            con.Open();

            string command = $"SELECT * FROM public.Receiver;";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        receivers.Add(new ReceiverDataModel { SerialNumber = reader["serialnumber"].ToString(), IntervalSeconds = int.Parse(reader["time_interval"].ToString()) });
                    }
            }
            con.Close();
            return receivers;
        }
        catch (Exception e)
        {
            throw new NotImplementedException();
        }
    }

    public async Task GetReceiversFromTrustedAPI()
    {
        try
        {
            List<ReceiverDataModel> dbReceivers = await GetReceiversFromDB();

            string message = await _client.GetStringAsync($"https://api.trusted.dk/api/Units/GetAll");

            List<ReceiverDataModel> apiReceivers = JsonSerializer.Deserialize<List<ReceiverDataModel>>(message);

            foreach (var apiReceiver in apiReceivers)
            {
                foreach (var dbReceiver in dbReceivers)
                {
                    if (apiReceiver.SerialNumber.Equals(dbReceiver.SerialNumber) && dbReceiver.IntervalSeconds != apiReceiver.IntervalSeconds)
                    {
                        string timeIntervalSerialized = JsonSerializer.Serialize(new IntervalSecond() { IntervalSeconds = dbReceiver.IntervalSeconds });
                        HttpContent _content = new StringContent(
                                timeIntervalSerialized,
                                Encoding.UTF8,
                                "application/json"
                                );

                        await _client.PutAsync($"https://api.trusted.dk/api/Units/Put?serialNumber={dbReceiver.SerialNumber}", _content);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }
}
