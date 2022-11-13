using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class ChartData
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("tagNumber")]
    public string? TagNumber { get; set; }
    [JsonPropertyName("measurements")]
    public List<ChartMeasurement>? Measurements { get; set; }

    public override string ToString()
    {
        string result = "Date: " + Date + " TagNumber: " + TagNumber;
        foreach (var item in Measurements)
        {
            result += "\n" + " Measurements: " + item.ToString();
        }

        return result;
    }
}

public class ChartMeasurement
{
    [JsonPropertyName("time")]
    public TimeSpan Time { get; set; }
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }
    [JsonPropertyName("humidity")]
    public float Humidity { get; set; }

    public override string ToString() {
        return "Time: " + Time + " Temperature: " + Temperature + " Humidity: " + Humidity;
    }
}

public class ChartCompareMeasurement
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    [JsonPropertyName("tagNumber")]
    public string? TagNumber { get; set; }
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }
    [JsonPropertyName("humidity")]
    public float Humidity { get; set; }

    public override string ToString()
    {
        return "Time: " + Time + " Temperature: " + Temperature + " Humidity: " + Humidity;
    }
}
