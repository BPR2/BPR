using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class SensorMeasurement
{
    [JsonPropertyName("measurementId")]
    public int MeasurementId { get; set; }
    [JsonPropertyName("sensorId")]
    public int SensorId { get; set; }
    [JsonPropertyName("tagNumber")]
    public string TagNumber { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }
    [JsonPropertyName("humidity")]
    public float Humidity { get; set; }
}
