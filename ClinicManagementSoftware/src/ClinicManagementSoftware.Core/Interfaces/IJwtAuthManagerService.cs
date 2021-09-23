using System;
using System.Security.Claims;
using ClinicManagementSoftware.Core.Dto.Authentication;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IJwtAuthManagerService
    {
        JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);
    }
}