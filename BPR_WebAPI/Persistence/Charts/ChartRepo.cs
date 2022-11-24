using BPR_RazorLibrary.Models;
using BPR_RazorLibrary.Pages;
using Npgsql;

namespace BPR_WebAPI.Persistence.Charts;

public class ChartRepo : IChartRepo
{
    private readonly IConfiguration configuration;
    string connectionString;

    public ChartRepo(IConfiguration iConfig)
    {
        configuration = iConfig;
        connectionString = configuration["ConnectionStrings:DefaultConnection"];
    }

    public async Task<WebContent> GetChartDataByFieldId(int fieldId, DateTime startDate, DateTime endDate)
    {
        List<DateTime> dates = new List<DateTime>();
        List<string> tagNumbers = new List<string>();
        List<ChartCompareMeasurement> chartCompareMeasurements = new List<ChartCompareMeasurement>();
        List<ChartData> chartDataList = new List<ChartData>();

        try
        {
            using var con = new NpgsqlConnection(connectionString);
            con.Open();

            string command1 = "select f.fieldid, sm.timestamp, s.sensorid, s.tagnumber, sm.temperature, sm.humidity\r\nfrom field f\r\njoin receiver r\r\non r.fieldid = f.fieldid\r\njoin sensor s\r\non s.receiverid = r.receiverid\r\njoin sensormeasurement sm\r\non sm.sensorid = s.sensorid\r\nwhere f.fieldid = @FieldId and timestamp > @StartDate and sm.timestamp < @EndDate\r\norder by sm.timestamp ";

            List<SensorMeasurement> chartMeasurements = new List<SensorMeasurement>();

            await using (NpgsqlCommand cmd = new NpgsqlCommand(command1, con))
            {
                cmd.Parameters.AddWithValue("@FieldId", NpgsqlTypes.NpgsqlDbType.Integer, fieldId);
                cmd.Parameters.AddWithValue("@StartDate", NpgsqlTypes.NpgsqlDbType.Date, startDate);
                cmd.Parameters.AddWithValue("@EndDate", NpgsqlTypes.NpgsqlDbType.Date, endDate);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        if (!dates.Contains(DateTime.Parse(reader["timestamp"].ToString()).Date))
                        {
                            dates.Add(DateTime.Parse(reader["timestamp"].ToString()).Date);
                        }

                        if (!tagNumbers.Contains(reader["tagnumber"].ToString()))
                        {
                            tagNumbers.Add(reader["tagnumber"].ToString());
                        }

                        chartCompareMeasurements.Add(new ChartCompareMeasurement
                        {
                            Time = DateTime.Parse(reader["timestamp"].ToString()).ToLocalTime(),
                            Temperature = float.Parse(reader["temperature"].ToString()),
                            Humidity = float.Parse(reader["humidity"].ToString()),
                            TagNumber = reader["tagnumber"].ToString()
                        });

                    }

                foreach (var date in dates)
                {
                    foreach (var tagNumber in tagNumbers)
                    {
                        ChartData chartData = new ChartData
                        {
                            Date = date,
                            TagNumber = tagNumber

                        };

                        foreach (var chartMeasurement in chartCompareMeasurements)
                        {
                            if (chartMeasurement.Time.Date.Equals(date) && chartMeasurement.TagNumber.Equals(tagNumber))
                            {
                                chartMeasurements.Add(new SensorMeasurement
                                {
                                    TagNumber = chartMeasurement.TagNumber,
                                    Timestamp = chartMeasurement.Time,
                                    Temperature = chartMeasurement.Temperature,
                                    Humidity = chartMeasurement.Humidity
                                });
                            }

                        }
                        /*chartData.Measurements = chartMeasurements;
                        chartDataList.Add(chartData);*/
                    }
                }
            }
            con.Close();

            return new WebContent(WebResponse.ContentRetrievalSuccess, chartMeasurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new WebContent(WebResponse.ContentRetrievalFailure, null);
        }
    }

}