using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Core.Dto.Authentication
{
    public class JwtTokenConfig
    {
        [JsonPropertyName("secret")] public string Secret => "1234567890123456789";

        [JsonPropertyName("issuer")] public string Issuer => "https://mywebapi.com";

        [JsonPropertyName("audience")] public string Audience => "https://mywebapi.com";

        [JsonPropertyName("accessTokenExpiration")]
        public int AccessTokenExpiration => 3600000;

        [JsonPropertyName("refreshTokenExpiration")]
        public int RefreshTokenExpiration => 3600;
    }
}