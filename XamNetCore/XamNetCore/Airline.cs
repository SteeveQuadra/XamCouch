using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace XamNetCore
{
    public partial class Airline
    {
        [JsonProperty("callsign")]
        public string Callsign { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("iata")]
        public string Iata { get; set; }

        [JsonProperty("icao")]
        public string Icao { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonIgnore]
        public string Infos { get => $"{Id} - {Country} - {Name}"; }
    }

    public partial class Airline
    {
        public static Airline FromJson(string json) => JsonConvert.DeserializeObject<Airline>(json, Converter.Settings);
    }
	public class Converter : JsonConverter
	{
		public override bool CanConvert(Type t) => true;

		public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
		{
			throw new Exception("Unknown type");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var t = value.GetType();

			throw new Exception("Unknown type");
		}

		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			DateParseHandling = DateParseHandling.None,
			Converters = { new Converter() },
		};
	}
}

