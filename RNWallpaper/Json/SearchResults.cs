using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RNWallpaper.Json
{

    public class JsonRoot
    {
        [JsonProperty("CurrentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("TotalPages")]
        public long TotalPages { get; set; }

        [JsonProperty("End")]
        public bool End { get; set; }

        [JsonProperty("Results")]
        public List<Results> Results { get; set; }
    }

    public class Results
    {
        [JsonProperty("ImageID")]
        public long ImageId { get; set; }

        [JsonProperty("Thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("Purity")]
        public Purity Purity { get; set; }

        [JsonProperty("Category")]
        public Category Category { get; set; }

        [JsonProperty("Width")]
        public long Width { get; set; }

        [JsonProperty("Height")]
        public long Height { get; set; }

        [JsonProperty("Favorites")]
        public long Favorites { get; set; }

        [JsonProperty("Link")]
        public string Link { get; set; }

        public Uri ThumbnailUrl => new Uri(Thumbnail, UriKind.Absolute);
    }

    public enum Category {
        Anime,
        People,
        General
    };

    public enum Purity { Sketchy, Sfw };

    public class SearchResults
    {
        public static JsonRoot FromJson(string json) => JsonConvert.DeserializeObject<JsonRoot>(json, Converter.Settings);
    }

    static class CategoryExtensions
    {
        public static Category? ValueForString(string str)
        {
            switch (str)
            {
                case "anime": return Category.Anime;
                case "people": return Category.People;
                case "general": return Category.General;
                default: return null;
            }
        }

        public static Category ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Category value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case Category.Anime: serializer.Serialize(writer, "anime"); break;
                case Category.People: serializer.Serialize(writer, "people"); break;
                case Category.General: serializer.Serialize(writer, "general"); break;
            }
        }
    }

    static class PurityExtensions
    {
        public static Purity? ValueForString(string str)
        {
            switch (str)
            {
                case "sketchy": return Purity.Sketchy;
                case "sfw": return Purity.Sfw;
                default: return null;
            }
        }

        public static Purity ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Purity value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case Purity.Sketchy: serializer.Serialize(writer, "sketchy"); break;
                case Purity.Sfw: serializer.Serialize(writer, "sfw"); break;
            }
        }
    }

    public static class Serialize
    {
        public static string ToJson(this JsonRoot self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal class Converter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Purity) || t == typeof(Category?) || t == typeof(Purity?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (t == typeof(Category))
                return CategoryExtensions.ReadJson(reader, serializer);
            if (t == typeof(Purity))
                return PurityExtensions.ReadJson(reader, serializer);
            if (t == typeof(Category?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return CategoryExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Purity?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return PurityExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType();
            if (t == typeof(Category))
            {
                ((Category)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Purity))
            {
                ((Purity)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new Converter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
