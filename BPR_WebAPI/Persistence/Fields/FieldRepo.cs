using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using Npgsql;

namespace BPR_WebAPI.Persistence.Fields
{

	public class FieldRepo : IFieldRepo
	{
		private readonly IConfiguration configuration;
		string connectionString;

		public FieldRepo(IConfiguration iConfig)
		{
			configuration = iConfig;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}


		public async Task<WebContent> GetAllFieldsByUserId(int userId)
		{
			List<Field> fields = new List<Field>();
			Receiver receiver;
			ReceiverData receiverData;

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command1 = "SELECT f.fieldid,f.name, f.pawLevelLimit, f.location, f.description as field_description, r.receiverid, " +
							"r.serialnumber, r.description as receiver_description, rd.timestamp, rd.longitude, rd.latitude " +
							"FROM public.field f LEFT JOIN public.receiver r on f.fieldid = r.fieldid " +
							"LEFT JOIN receiverdata rd ON rd.receiverid = r.receiverid where r.accountId = @UserId " +
							"order by timestamp desc limit 1";

				await using (NpgsqlCommand cmd = new NpgsqlCommand(command1, con))
				{
					cmd.Parameters.AddWithValue("@UserId", NpgsqlTypes.NpgsqlDbType.Integer, userId);
					await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
						while (await reader.ReadAsync())
						{

							receiverData = new ReceiverData
							{
								ReceiverId = int.Parse(reader["receiverid"].ToString()),
								Timestamp = DateTime.Parse(reader["timestamp"].ToString()),
								Longitude = float.Parse(reader["longitude"].ToString()),
								Latitude = float.Parse(reader["latitude"].ToString())
							};

							receiver = new Receiver
							{
								ReceiverId = int.Parse(reader["receiverid"].ToString()),
								SerialNumber = reader["serialnumber"].ToString(),
								AccountId = userId,
								FieldId = int.Parse(reader["fieldid"].ToString()),
								Description = reader["receiver_description"].ToString(),
								ReceiverLatestData = receiverData
							};

							fields.Add(
								new Field
								{
									Id = int.Parse(reader["fieldid"].ToString()),
									Name = reader["name"].ToString(),
									PawLevelLimit = int.Parse(reader["pawLevelLimit"].ToString()),
									Location = reader["location"].ToString(),
									Description = reader["field_description"].ToString(),
									Receiver = receiver
								});
						}
				}

				foreach (var field in fields)
				{
					List<Sensor> sensors = new List<Sensor>();
					SensorMeasurement measurement;

					string command2 = "SELECT s.sensorid, s.tagnumber, sm.temperature, sm.humidity, s.batterylow, s.description, sm.timestamp" +
						" FROM public.sensor s LEFT JOIN sensormeasurement sm ON sm.sensorid = s.sensorid where s.receiverId = @ReceiverId " +
						"order by timestamp desc limit (select count(*) FROM sensor where receiverid = @ReceiverId)";

					await using (NpgsqlCommand cmd = new NpgsqlCommand(command2, con))
					{
						cmd.Parameters.AddWithValue("@ReceiverId", NpgsqlTypes.NpgsqlDbType.Integer, field.Receiver.ReceiverId);
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
											ReceiverId = field.Receiver.ReceiverId,
											TagNumber = reader["tagnumber"].ToString(),
											BatteryLow = bool.Parse(reader["batterylow"].ToString()),
											Description = reader["description"].ToString(),
											LatestSensorMeasurement = measurement
										});
							}
					}
					field.Receiver.Sensors = sensors;
				}

				con.Close();

				return new WebContent(WebResponse.ContentRetrievalSuccess, fields);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return new WebContent(WebResponse.ContentRetrievalFailure, null);
			}
		}


		public async Task<WebResponse> CreateFieldAsync(Field field)
		{
			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command = $"INSERT INTO public.Field(Name, Location, Description, PawLevelLimit) VALUES (@Name, @Location, @Description, @PawLevelLimit);";
				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@Name", field.Name);
					cmd.Parameters.AddWithValue("@Location", field.Location);
					cmd.Parameters.AddWithValue("@Description", field.Description);
					cmd.Parameters.AddWithValue("@PawLevelLimit", field.PawLevelLimit);

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

		public async Task<WebContent> GetLatestFieldByUserId(int userId)
		{
			Field field = new Field();

			try
			{
				using var con = new NpgsqlConnection(connectionString);
				con.Open();

				string command = $"select f.fieldid, f.name, f.location, f.description, f.pawlevellimit from field f join receiver r on r.fieldid = r.fieldid where r.accountid = @UserId order by fieldid desc limit 1;";

				await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
				{
					cmd.Parameters.AddWithValue("@UserId", NpgsqlTypes.NpgsqlDbType.Integer, userId);

					await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
						while (await reader.ReadAsync())
						{
							field = new Field
							{
								Id = int.Parse(reader["fieldid"].ToString()),
								Name = reader["name"].ToString(),
								PawLevelLimit = int.Parse(reader["pawLevelLimit"].ToString()),
								Location = reader["location"].ToString(),
								Description = reader["description"].ToString()
							};
						}					

					cmd.ExecuteNonQuery();
				}
				con.Close();
				return new WebContent(WebResponse.ContentCreateSuccess, field);
			}
			catch (Exception e)
			{
				return new WebContent(WebResponse.ContentCreateFailure, null);
			}
		}
    
     public async Task<WebResponse> UnassignReceiver(int fieldId, int receiverId)
        {
            try
            {
                using var con = new NpgsqlConnection(connectionString);
                con.Open();

                string command = $"UPDATE public.receiver SET fieldid = null where receiverId = @receiverId AND fieldid = @fieldid;";
                await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@fieldId", fieldId);
                    cmd.Parameters.AddWithValue("@receiverId", receiverId);
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

        public async Task<WebContent> UpdateField(Field field)
        {
            if (string.IsNullOrEmpty(field.Name)) return new WebContent(WebResponse.ContentDataCorrupted, field);

            try
            {
                using var con = new NpgsqlConnection(connectionString);
                con.Open();

                string command = $"Update public.Field SET name = @name, description = @description, pawLevelLimit = @pawLevelLimit  WHERE fieldid = @fieldId;" +
                    $"UPDATE public.receiver SET fieldid = @fieldId where serialnumber = @serialnumber;";
                await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@fieldId", field.Id);
                    cmd.Parameters.AddWithValue("@name", field.Name);
                    cmd.Parameters.AddWithValue("@name", field.PawLevelLimit);
                    cmd.Parameters.AddWithValue("@description", field.Description);
                    cmd.Parameters.AddWithValue("@pawLevelLimit", field.PawLevelLimit);
                    cmd.Parameters.AddWithValue("@serialnumber", field.Receiver.SerialNumber);
                    cmd.ExecuteNonQuery();
                }

                foreach (var s in field.Receiver.Sensors)
                {
                    string command2 = $"UPDATE public.sensor SET description = @description where tagnumber = @tagnumber;";
                    await using (NpgsqlCommand cmd = new NpgsqlCommand(command2, con))
                    {
                        cmd.Parameters.AddWithValue("@tagnumber", s.TagNumber);
                        cmd.Parameters.AddWithValue("@description", s.Description);
                        cmd.ExecuteNonQuery();
                    }

                }
                con.Close();
                return new WebContent(WebResponse.ContentUpdateSuccess, field);
            }
            catch (Exception e)
            {
                return new WebContent(WebResponse.ContentUpdateFailure, field);
            }
        }
	}
}
