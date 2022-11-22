using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using Npgsql;
using System.Collections.Generic;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace BPR_WebAPI.Persistence.Receivers;

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

	public async Task<List<Receiver>> GetAllReceivers()
	{
		List<Receiver> receivers = new List<Receiver>();

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
						receivers.Add(new Receiver { SerialNumber = reader["serialnumber"].ToString(), ReceiverId = int.Parse(reader["receiverid"].ToString()), TimeInterval = int.Parse(reader["time_interval"].ToString()) });
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

	/// <summary>
	/// Returns a list of the receivers assigned to specified user
	/// </summary>
	/// <param name="userID"></param>
	/// <returns></returns>
	public async Task<WebContent> GetReceiversByUserID(int userID)
	{
		List<Receiver> receivers = new List<Receiver>();

		try
		{
			using var con = new NpgsqlConnection(connectionString);
			con.Open();

			string command = $"SELECT * FROM public.Receiver WHERE accountid = @AccountID;";

			await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
			{
				cmd.Parameters.AddWithValue("@AccountID", userID);

				await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
					while (await reader.ReadAsync())
					{
						receivers.Add(new Receiver
						{
							SerialNumber = reader["serialnumber"].ToString(),
							ReceiverId = int.Parse(reader["receiverid"].ToString()),
							FieldId = reader["fieldid"] as int?,
							TimeInterval = int.Parse(reader["time_interval"].ToString())
                        });
					}
			}
			con.Close();
			return new WebContent(WebResponse.ContentRetrievalSuccess, receivers);
		}
		catch (Exception e)
		{
			return new WebContent(WebResponse.ContentRetrievalFailure, null);
		}
	}

	public async Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID)
	{
		try
		{
			//TODO check for receiver already assigned to a field

			using var con = new NpgsqlConnection(connectionString);
			con.Open();

			string command = $"UPDATE public.receiver SET fieldid = @FieldID WHERE receiverid = @ReceiverID";
			await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
			{
				cmd.Parameters.AddWithValue("@FieldID", fieldID);
				cmd.Parameters.AddWithValue("@ReceiverID", receiverID);

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

	public async Task<WebContent> GetAllReceiversList()
	{
        List<Receiver> receivers = new List<Receiver>();

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT distinct on (r.serialnumber) r.receiverid, r.serialnumber, rd.timestamp, r.time_interval, a.username\r\nFROM public.receiver r \r\nLeft JOIN public.receiverdata rd \r\nON rd.receiverid = r.receiverid\r\nLeft JOIN public.account a \r\nON r.accountid = a.accountid\r\norder by r.serialnumber, rd.timestamp desc";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        receivers.Add(new Receiver
                        {
                            SerialNumber = reader["serialnumber"].ToString(),
                            ReceiverId = int.Parse(reader["receiverid"].ToString()),
                            ReceiverLatestData = string.IsNullOrEmpty(reader["timestamp"].ToString()) ? null : new ReceiverData() {Timestamp = DateTime.Parse(reader["timestamp"].ToString()).ToLocalTime() },
                            Description = reader["username"].ToString(),
							TimeInterval = int.Parse(reader["time_interval"].ToString())
                        });
					}
			}

            foreach (var receiver in receivers)
            {
                List<Sensor> sensors = new List<Sensor>();

                string command2 = $"SELECT s.sensorid, s.tagnumber FROM public.sensor s LEFT JOIN sensormeasurement sm ON sm.sensorid = s.sensorid where s.receiverId = @ReceiverId order by timestamp desc limit (select count(*) FROM sensor where receiverid = @ReceiverId)";

                await using (NpgsqlCommand cmd = new NpgsqlCommand(command2, con))
                {
                    cmd.Parameters.AddWithValue("@ReceiverId", NpgsqlTypes.NpgsqlDbType.Integer, receiver.ReceiverId);

                    await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        while (await reader.ReadAsync())
                        {
                            sensors.Add(
                                    new Sensor
                                    {
                                        SensorId = int.Parse(reader["sensorid"].ToString()),
                                        TagNumber = reader["tagnumber"].ToString(),               
                                    });
                        }
                }
                receiver.Sensors = sensors;
            }

            con.Close();
            return new WebContent(WebResponse.ContentRetrievalSuccess, receivers);
        }
        catch (Exception e)
        {
            return new WebContent(WebResponse.ContentRetrievalFailure, null);
        }
    }

	public async Task<WebResponse> UpdateReceiverTimeInterval(int timeInterval, string serialNumber)
	{
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"UPDATE public.receiver SET  time_interval= @TimeInterval WHERE serialnumber = @SerialNumber;";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
			{
                cmd.Parameters.AddWithValue("@TimeInterval", timeInterval);
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

	public async Task<WebContent> GetReceiverBySerialNumber(string serialNumber)
	{
		Receiver receiver = new Receiver();
        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command = $"SELECT * FROM public.Receiver WHERE serialNumber = @serialNumber;";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
            {
                cmd.Parameters.AddWithValue("@serialNumber", serialNumber);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        receiver = new Receiver
                        {
                            SerialNumber = reader["serialnumber"].ToString(),
							AccountId = int.Parse(reader["accountid"].ToString()),
                            ReceiverId = int.Parse(reader["receiverid"].ToString()),
                            FieldId = reader["fieldid"] as int?,
                            TimeInterval = int.Parse(reader["time_interval"].ToString())
                        };
                    }
            }

                List<Sensor> sensors = new List<Sensor>();
                SensorMeasurement measurement;

                string command2 = "SELECT s.sensorid, s.tagnumber, sm.temperature, sm.humidity, s.batterylow, s.description, sm.timestamp" +
                    " FROM public.sensor s LEFT JOIN sensormeasurement sm ON sm.sensorid = s.sensorid where s.receiverId = @ReceiverId " +
                    "order by timestamp desc limit (select count(*) FROM sensor where receiverid = @ReceiverId)";

                await using (NpgsqlCommand cmd = new NpgsqlCommand(command2, con))
                {
                    cmd.Parameters.AddWithValue("@ReceiverId", NpgsqlTypes.NpgsqlDbType.Integer, receiver.ReceiverId);
                    await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        while (await reader.ReadAsync())
                        {
                            measurement = new SensorMeasurement
                            {
                                SensorId = int.Parse(reader["sensorid"].ToString()),
                                Temperature = float.Parse(reader["temperature"].ToString()),
                                Humidity = float.Parse(reader["humidity"].ToString()),
                                Timestamp = DateTime.Parse(reader["timestamp"].ToString()),
                            };

                            sensors.Add(
                                    new Sensor
                                    {
                                        SensorId = int.Parse(reader["sensorid"].ToString()),
                                        ReceiverId = receiver.ReceiverId,
                                        TagNumber = reader["tagnumber"].ToString(),
                                        BatteryLow = bool.Parse(reader["batterylow"].ToString()),
                                        Description = reader["description"].ToString(),
                                        LatestSensorMeasurement = measurement
                                    });
                        }
                }
			receiver.Sensors = sensors;

            con.Close();
            return new WebContent(WebResponse.ContentRetrievalSuccess, receiver);
        }
        catch (Exception e)
        {
            return new WebContent(WebResponse.ContentRetrievalFailure, null);
        }
    }
}