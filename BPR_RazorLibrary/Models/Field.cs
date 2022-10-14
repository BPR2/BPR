using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BPR_RazorLibrary.Models
{
    public class Field
    {
        [JsonPropertyName("fieldid")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("pawLevelLimit")]
        public int? PawLevelLimit { get; set; }
        [JsonPropertyName("location")]
        public string? Location { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("receiver")]
        public Receiver? Receiver { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Field field &&
                   Name == field.Name &&
                   PawLevelLimit == field.PawLevelLimit &&
                   Location == field.Location &&
                   Description == field.Description;
        }
    }
}
