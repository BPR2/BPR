using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class Sensor
{
    [JsonPropertyName("SensorId")]
    public int SensorId { get; set; }
    [JsonPropertyName("ReceiverId")]
    public int ReceiverId { get; set; }
    [JsonPropertyName("TagNumber")]
    public string? TagNumber { get; set; }
    [JsonPropertyName("BatteryLow")]
    public bool BatteryLow { get; set; }
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

}
