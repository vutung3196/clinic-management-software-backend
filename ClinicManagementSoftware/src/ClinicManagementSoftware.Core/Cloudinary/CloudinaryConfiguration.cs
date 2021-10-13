using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public class CloudinaryConfiguration
    {
        [JsonPropertyName("cloudName")] 
        public string CloudName { get; set; }

        [JsonPropertyName("apiKey")] 
        public string ApiKey { get; set; }

        [JsonPropertyName("apiSecret")] 
        public string ApiSecret { get; set; }
    }
}