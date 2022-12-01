using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using Npgsql;

namespace BPR_WebAPI.Persistence.Sensors;

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

    public async Task<WebResponse> UpdateSensorAsync(string tagNumber, string serialNumber)
    {
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string receiverCheckCommand = $"SELECT receiverid FROM public.sensor WHERE tagNumber = @TagNumber;";
            string receiverCheck = "";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(receiverCheckCommand, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", NpgsqlTypes.NpgsqlDbType.Varchar, tagNumber);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        receiverCheck = reader["receiverid"].ToString();
                    }
            }

            if (!receiverCheck.Equals(""))
            {
                return WebResponse.ContentUpdateFailure;
            }

            string command = $"UPDATE public.sensor SET receiverid= (SELECT receiverid FROM receiver where serialnumber = @SerialNumber) WHERE tagNumber = @TagNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", tagNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);

                cmd.ExecuteNonQuery();
            }
            con.Close();
            return WebResponse.ContentUpdateSuccess;
        }
        catch (Exception e)
        {
            return WebResponse.ContentUpdateFailure;
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

    public async Task<List<Sensor>> getAllSensorsByReceiverId(int receiverId)
    {
        List<Sensor> sensors = new List<Sensor>();

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT * FROM public.sensor where public.sensor.receiverid = @ReceiverId";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@ReceiverId", NpgsqlTypes.NpgsqlDbType.Integer, receiverId);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        sensors.Add(
                            new Sensor
                            {
                                SensorId = int.Parse(reader["sensorid"].ToString()),
                                ReceiverId = int.Parse(reader["receiverid"].ToString()),
                                TagNumber = reader["tagnumber"].ToString(),
                                BatteryLow = bool.Parse(reader["batterylow"].ToString()),
                                Description = reader["description"].ToString()
                            });
                    }
            }
            con.Close();
            return sensors;
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<WebResponse> UnassignSensorAsync(string tagNumber)
    {
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"UPDATE public.sensor SET receiverid = null WHERE tagNumber = @TagNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", tagNumber);

                cmd.ExecuteNonQuery();
            }
            con.Close();
            return WebResponse.ContentUpdateSuccess;
        }
        catch (Exception e)
        {
            return WebResponse.ContentUpdateFailure;
        }
    }

    public async Task<WebResponse> UpdateSensorDescription(string tagNumber, string description)
    {
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"UPDATE public.sensor SET description = @Description WHERE tagNumber = @TagNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@TagNumber", tagNumber);
                cmd.Parameters.AddWithValue("@Description", description);

                cmd.ExecuteNonQuery();
            }
            con.Close();
            return WebResponse.ContentUpdateSuccess;
        }
        catch (Exception e)
        {
            return WebResponse.ContentUpdateFailure;
        }
    }
}
