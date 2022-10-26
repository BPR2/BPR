using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class User
{
    [JsonPropertyName("accountId")]
    public int AccountId { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("name")]
    public string? FullName { get; set; }
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }
    [JsonPropertyName("email")]
    [EmailAddress]
    public string? Email { get; set; }
    [JsonPropertyName("location")]
    public string? Address { get; set; }

    public bool IsRowExpanded { get; set; } = false;
}
