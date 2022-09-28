using BPR_RazorLib.Models;
using Npgsql;

namespace BPR_WebAPI.Persistence.Sensor;

public class SensorRepo : ISensorRepo
{
    private readonly IConfiguration configuration;
    string connectionString;

    public SensorRepo(IConfiguration iConfig)
    {
        configuration = iConfig;
        connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public async Task<WebResponse> AddNewSensorAsync(string tagNumber, string serialNumber)
    {
        try
        {
            var isDuplicate = await IsSensorAlreadyExist(tagNumber);

            if (isDuplicate) return WebResponse.ContentDuplicate;

            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"INSERT INTO public.sensor(receiverid, tagnumber) VALUES ((SELECT receiverid FROM receiver where serialnumber = @SerialNumber), @TagNumber);";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", tagNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);

                cmd.ExecuteNonQuery();
            }
            con.Close();
            return WebResponse.ContentCreateSuccess;
        }
        catch (Exception e)
        {
            return WebResponse.ContentCreateFailure;
        }
    }

    private async Task<bool> IsSensorAlreadyExist(string desiredTagNumber)
    {
        var result = await GetSensorAsyncTagNumber(desiredTagNumber);

        if (result.Equals(string.Empty)) return false;

        return true;
    }

    private async Task<string> GetSensorAsyncTagNumber(string tagNumber)
    {
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string result = "";

            string command = $"SELECT tagnumber FROM public.Sensor where tagnumber = @TagNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", NpgsqlTypes.NpgsqlDbType.Varchar, tagNumber);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        result = reader["tagnumber"].ToString();
                    }
            }
            con.Close();
            return result;
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }
}
