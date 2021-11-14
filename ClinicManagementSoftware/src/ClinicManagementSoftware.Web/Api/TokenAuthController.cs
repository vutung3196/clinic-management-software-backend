using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Exceptions.Authentication;
using ClinicManagementSoftware.Core.Exceptions.Clinic;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using ClinicManagementSoftware.Web.Authentication.Model;
using ClinicManagementSoftware.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenAuthController : ControllerBase
    {
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly IJwtAuthManagerService _jwtAuthManagerService;
        private readonly ILogger<TokenAuthController> _logger;

        public TokenAuthController(IJwtAuthManagerService jwtAuthManagerService,
            ILogger<TokenAuthController> logger,
            ITokenAuthenticationService tokenAuthenticationService)
        {
            _jwtAuthManagerService = jwtAuthManagerService;
            _logger = logger;
            _tokenAuthenticationService = tokenAuthenticationService;
        }

        [HttpPost("auth")]
        [ValidateModel]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel request)
        {
            try
            {
                var userResult = await _tokenAuthenticationService.LoginAsync(request.UserName, request.Password);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userResult.UserName),
                    new Claim(ClaimTypes.Role, userResult.Role.RoleName)
                };

                var jwtResult = _jwtAuthManagerService.GenerateTokens(request.UserName, claims, DateTime.Now);
                return Ok(new Response<AuthenticateResultModel>(new AuthenticateResultModel
                {
                    AccessToken = jwtResult.AccessToken,
                    AccessTokenExpiredAt = jwtResult.AccessTokenExpireAt,
                    UserName = request.UserName,
                    ClinicId = userResult.ClinicId,
                    Id = userResult.Id,
                    FullName = userResult.FullName,
                    Role = userResult.Role.RoleName
                }));
            }
            catch (UserNotFoundException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Tài khoản hoặc mật khẩu chưa chính xác");
            }
            catch (InCorrectPasswordException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Tài khoản hoặc mật khẩu chưa chính xác");
            }
            catch (UserInActiveException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Tài khoản đang bị khóa");
            }
            catch (ClinicInActiveException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Phòng khám đang bị khóa");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Getting this exception: {exception.Message} " +
                                 $"when User [{request.UserName}] authenticates");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}