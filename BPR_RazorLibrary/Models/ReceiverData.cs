namespace BPR_RazorLibrary.Models;

public class ReceiverData
{
    public int ReceiverDataId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime TimeStamp { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
}
