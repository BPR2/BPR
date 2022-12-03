using System.Text.Json.Serialization;

namespace BPR_RazorLibrary.Models;

public class Field
{
    [JsonPropertyName("fieldid")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("pawLevelLimit")]
    public int? PawLevelLimit { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("receiver")]
    public Receiver? Receiver { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Field field &&
               Name == field.Name &&
               PawLevelLimit == field.PawLevelLimit &&
               Description == field.Description;
    }
}
