using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.Authentication;
using ClinicManagementSoftware.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ClinicManagementSoftware.Core.Services
{
    public class JwtAuthManagerService : IJwtAuthManagerService
    {
        private readonly byte[] _secret;

        public JwtAuthManagerService()
        {
            _secret = Encoding.ASCII.GetBytes(ConfigurationConstant.JwtTokenSecret);
        }

        public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrEmpty(claims?.FirstOrDefault(x
                => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                ConfigurationConstant.JwtTokenIssuer,
                shouldAddAudienceClaim ? ConfigurationConstant.JwtTokenAudience : string.Empty,
                claims,
                expires: now.AddMinutes(ConfigurationConstant.JwtTokenAccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                AccessTokenExpireAt = now.AddMinutes(ConfigurationConstant.JwtTokenAccessTokenExpiration)
            };
        }
    }
}