using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Core.Dto.Authentication
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }
    }
}