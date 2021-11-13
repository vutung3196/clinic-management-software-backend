using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using ClinicManagementSoftware.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicManagementService _clinicService;
        private readonly ILogger<ClinicController> _logger;

        public ClinicController(ILogger<ClinicController> logger, IClinicManagementService clinicService)
        {
            _logger = logger;
            _clinicService = clinicService;
        }


        [Authorize]
        [HttpGet("{id}")]
        [Authorize(Roles = "MasterAdmin,Admin")]
        public async Task<IActionResult> GetClinic(long id)
        {
            try
            {
                var result = await _clinicService.GetClinicInformation(id);
                return Ok(new Response<ClinicInformationResponse>(result));
            }
            catch (ArgumentException exception)
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

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateClinic([FromBody] CreateUpdateClinicRequestDto request)
        {
            try
            {
                var result = await _clinicService.CreateClinic(request);
                return Ok(new Response<ClinicInformationForAdminResponse>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "MasterAdmin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _clinicService.GetClinics();
                return Ok(new Response<IEnumerable<ClinicInformationForAdminResponse>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "MasterAdmin,Admin")]
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> EditClinic(long id, [FromBody] CreateUpdateClinicRequestDto request)
        {
            try
            {
                await _clinicService.UpdateClinicInformation(id, request);
                return Ok("Update successfully");
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}