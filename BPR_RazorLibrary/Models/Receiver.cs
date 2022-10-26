using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class Receiver
{
    [JsonPropertyName("ReceiverId")]
    public int ReceiverId { get; set; }

    [JsonPropertyName("AccountId")]
    public int? AccountId { get; set; }

    [JsonPropertyName("FieldId")]
    public int? FieldId { get; set; }

    [JsonPropertyName("SerialNumber")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }
    [JsonPropertyName("receiverLatestData")]
    public ReceiverData? ReceiverLatestData { get; set; }
    [JsonPropertyName("sensors")]
    public List<Sensor>? Sensors { get; set; }

    public bool IsRowExpanded { get; set; } = false;
}
