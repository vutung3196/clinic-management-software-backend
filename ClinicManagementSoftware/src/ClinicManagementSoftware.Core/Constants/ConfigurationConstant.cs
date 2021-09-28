using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Constants
{
    public static class ConfigurationConstant
    {
        public static string ClinicManagementSoftwareDatabase = "MySQLConnection";
        public static List<string> PatientVisitingDoctorFormRoles = new() { "Doctor", "Accountant" };

        public static string TokenExpirationInSeconds = "TokenExpiration";
        public static string JwtTokenSecret = "1234567890123456789";
        public static string JwtTokenIssuer = "https://mywebapi.com";
        public static string JwtTokenAudience = "https://mywebapi.com";
        public static int JwtTokenAccessTokenExpiration = 3600000;
        public static int JwtTokenRefreshTokenExpiration = 3600;
    }
}