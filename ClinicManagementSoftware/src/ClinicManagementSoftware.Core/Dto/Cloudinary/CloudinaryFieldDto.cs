using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Dto.Cloudinary
{
    public class CloudinaryFieldDto
    {
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }

        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("original_filename")]
        public string OriginalFilename { get; set; }

        [JsonProperty("placeholder")]
        public bool Placeholder { get; set; }

        [JsonProperty("public_id")]
        public string PublicId { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("secure_url")]
        public string SecureUrl { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("tags")]
        public List<object> Tags { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("version_id")]
        public string VersionId { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}