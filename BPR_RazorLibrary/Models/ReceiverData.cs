using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class ReceiverData
{
    [JsonPropertyName("receiverDataId")]
    public int ReceiverDataId { get; set; }
    [JsonPropertyName("receiverId")]
    public int ReceiverId { get; set; }
    [JsonPropertyName("timeStamp")]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }
    [JsonPropertyName("latitude")]
    public float Latitude { get; set; }
}
