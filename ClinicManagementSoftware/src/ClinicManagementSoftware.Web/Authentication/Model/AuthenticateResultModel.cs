using System;

namespace ClinicManagementSoftware.Web.Authentication.Model
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public DateTime AccessTokenExpiredAt { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public long ClinicId { get; set; }
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}