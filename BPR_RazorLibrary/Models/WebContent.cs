using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class WebContent
{
    [JsonPropertyName("Response")]
    public WebResponse response { get; set; }
    [JsonPropertyName("Content")]
    public object content { get; set; }

    public WebContent(WebResponse response, object content)
    {
        this.response = response;
        this.content = content;
    }
}
