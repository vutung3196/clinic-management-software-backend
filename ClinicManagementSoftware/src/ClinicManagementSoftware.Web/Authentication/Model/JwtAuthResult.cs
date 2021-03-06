using System;
using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Web.Authentication.Model
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")] public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")] public RefreshTokenDto RefreshToken { get; set; }

        [JsonPropertyName("expiredAt")] public DateTime AccessTokenExpireAt { get; set; }
    }
}