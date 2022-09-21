using System.Text.Json.Serialization;

namespace FuncBearerToken
{
    public class Response
    {
        [JsonPropertyName("access_token")]
        public string? access_token { get; set; }
    }
}
