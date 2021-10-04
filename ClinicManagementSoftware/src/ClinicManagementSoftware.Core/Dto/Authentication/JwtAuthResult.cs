using System;
using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Core.Dto.Authentication
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")] public string AccessToken { get; set; }
        [JsonPropertyName("expiredAt")] public DateTime AccessTokenExpireAt { get; set; }
    }
}