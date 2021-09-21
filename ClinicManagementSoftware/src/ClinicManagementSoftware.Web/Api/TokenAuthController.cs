using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Exceptions.Authentication;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using ClinicManagementSoftware.Web.Authentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManagerService _jwtAuthManagerService;
        private readonly ILogger<TokenAuthController> _logger;
        private readonly IUserContext _userContext;

        public TokenAuthController(IUserService userService, IJwtAuthManagerService jwtAuthManagerService,
            ILogger<TokenAuthController> logger, IUserContext userContext)
        {
            _userService = userService;
            _jwtAuthManagerService = jwtAuthManagerService;
            _logger = logger;
            _userContext = userContext;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel request)
        {
            try
            {
                var loginResult = await _userService.LoginAsync(request.UserName, request.Password);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, loginResult.UserName),
                    new Claim(ClaimTypes.Role, loginResult.Role.RoleName ?? string.Empty)
                };

                var jwtResult = _jwtAuthManagerService.GenerateTokens(request.UserName, claims, DateTime.Now);
                var userContext = await _userContext.GetUserContextByUserName(request.UserName);
                return Ok(new Response<AuthenticateResultModel>(new AuthenticateResultModel
                {
                    AccessToken = jwtResult.AccessToken,
                    //RefreshToken = jwtResult.RefreshToken.TokenString,
                    AccessTokenExpiredAt = jwtResult.AccessTokenExpireAt,
                    UserName = request.UserName,
                    ClinicId = userContext.ClinicId,
                    Role = loginResult.Role.RoleName ?? string.Empty
                }));
            }
            catch (UserNotFoundException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Incorrect username or password");
            }
            catch (InCorrectPasswordException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Incorrect username or password");
            }
            catch (UserInActiveException exception)
            {
                _logger.LogInformation(exception.Message);
                return BadRequest("Inactive user");
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