﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataFetcherLambdaFunc.Models
{
    public class SensorDataModel
    {
        [JsonPropertyName("SerialNumber")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("TagNumber")]
        public string? TagNumber { get; set; }

        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("BatteryLow")]
        public object? BatteryLow { get; set; }

        [JsonPropertyName("Temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("Humidity")]
        public double Humidity { get; set; }

        public override string ToString()
        {
            return "SerialNumber: " + SerialNumber + " TagNumber: " + TagNumber;
        }
    }
}
