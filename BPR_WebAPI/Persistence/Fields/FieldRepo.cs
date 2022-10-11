using BPR_RazorLibrary.Models;
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

                throw new NotImplementedException();
            }
        }
    }
}
