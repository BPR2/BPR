using BPR_RazorLib.Models;
using BPR_RazorLibrary.Models;
using Npgsql;

namespace BPR_WebAPI.Persistence.Receiver;

public class ReceiverRepo : IReceiverRepo
{
    private readonly IConfiguration configuration;
    string connectionString;

    public ReceiverRepo(IConfiguration iConfig)
    {
        configuration = iConfig;
        connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public async Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName)
    {
        try
        {
            var isDuplicate = await IsReceiverAlreadyExist(serialNumber);

            if (isDuplicate) return WebResponse.ContentDuplicate;

            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"INSERT INTO public.receiver(accountid, serialnumber) VALUES ((SELECT accountid FROM account where username = @username), @serialNumber);";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@serialNumber", serialNumber);

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

    private async Task<bool> IsReceiverAlreadyExist(string desiredSerialNumber)
    {
        var result = await GetReceiverAsyncSerialNumber(desiredSerialNumber);

        if (result.Equals(string.Empty)) return false;

        return true;
    }

    private async Task<string> GetReceiverAsyncSerialNumber(string serialNumber)
    {
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string result = "";

            string command = $"SELECT serialnumber FROM public.Receiver where serialnumber = @SerialNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@SerialNumber", NpgsqlTypes.NpgsqlDbType.Varchar, serialNumber);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        result = reader["serialnumber"].ToString();
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

    public async Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceivers()
    {
        List<BPR_RazorLibrary.Models.Receiver> receivers = new List<BPR_RazorLibrary.Models.Receiver>();

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT * FROM public.Receiver;";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        receivers.Add(new BPR_RazorLibrary.Models.Receiver { SerialNumber = reader["serialnumber"].ToString(), ReceiverId = int.Parse(reader["receiverid"].ToString()) });
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
}