using System;
using System.Text.Json.Serialization;

namespace ClinicManagementSoftware.Web.Authentication.Model
{
    public class RefreshTokenDto
    {
        [JsonPropertyName("username")] public string UserName { get; set; }

        [JsonPropertyName("tokenString")] public string TokenString { get; set; }

        [JsonPropertyName("expireAt")] public DateTime ExpireAt { get; set; }
    }
}