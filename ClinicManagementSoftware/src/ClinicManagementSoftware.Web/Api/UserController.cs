using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.User;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(new Response<IEnumerable<UserResultResponse>>(result));
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (UserNotFoundException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
        {
            try
            {
                var result = await _userService.CreateUser(request);
                return Ok(new Response<UserResultResponse>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden, exception.Message);
            }
            catch (UserNotFoundException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(long id, [FromBody] EditUserDto request)
        {
            try
            {
                var result = await _userService.EditUser(id, request);
                return Ok(new Response<UserResultResponse>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(long id)
        {
            try
            {
                await _userService.DeactivateUser(id);
                return Ok("Deactivate user successfully");
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationUserModel model)
        {
            var userDto = _mapper.Map<UserDto>(model);
            try
            {
                // create user
                var user = await _userService.CreateAsync(userDto);
                _logger.LogInformation($"Create user {user.UserName} successfully");
                return Ok($"Create user {model.UserName} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}