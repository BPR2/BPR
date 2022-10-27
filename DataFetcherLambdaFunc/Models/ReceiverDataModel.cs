using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataFetcherLambdaFunc.Models;

public class ReceiverDataModel
{
    [JsonPropertyName("SerialNumber")]
    public string? SerialNumber { get; set; }
    [JsonPropertyName("IntervalSeconds")]
    public int? IntervalSeconds { get; set; }

    [JsonPropertyName("Position")]
    public Position? position { get; set; }

public class Position
{
    [JsonPropertyName("Timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("Latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("Longitude")]
    public double Longitude { get; set; }
}
}

public class IntervalSecond
{
    [JsonPropertyName("IntervalSeconds")]
    public int? IntervalSeconds { get; set; }
}