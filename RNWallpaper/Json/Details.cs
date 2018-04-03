using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RNWallpaper.Json
{

    public partial class Details
    {
        [JsonProperty("ImageID")] public long ImageId { get; set; }

        [JsonProperty("URL")] public string Url { get; set; }

        [JsonProperty("UploadedOn")] public DateTimeOffset UploadedOn { get; set; }

        [JsonProperty("Link")] public string Link { get; set; }

        [JsonProperty("Uploader")] public Uploader Uploader { get; set; }

        [JsonProperty("Tags")] public List<Tag> Tags { get; set; }

        [JsonProperty("Colors")] public List<ImageColors> Colors { get; set; }
    }

    public partial class ImageColors
    {
        [JsonProperty("HEX")] public string Hex { get; set; }

        [JsonProperty("RGB")] public string Rgb { get; set; }

        [JsonProperty("Link")] public string Link { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("TagID")] public long TagId { get; set; }

        [JsonProperty("Name")] public string Name { get; set; }

        [JsonProperty("Purity")] public string Purity { get; set; }

        [JsonProperty("Link")] public string Link { get; set; }
    }

    public partial class Uploader
    {
        [JsonProperty("Name")] public string Name { get; set; }

        [JsonProperty("Profile")] public string Profile { get; set; }

        [JsonProperty("ProfilePicture")] public string ProfilePicture { get; set; }
    }

    public partial class Details
    {
        public static Details FromJson(string json) =>
            JsonConvert.DeserializeObject<Details>(json, DetailsConvert.Settings);
    }

    internal class DetailsConvert
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
    }
}
