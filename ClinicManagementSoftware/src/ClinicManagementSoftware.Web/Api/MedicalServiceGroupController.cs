using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicManagementSoftware.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MedicalServiceGroupController : ControllerBase
    {
        private readonly IMedicalServiceGroupService _medicalGroupService;
        private readonly ILogger<MedicalServiceController> _logger;

        public MedicalServiceGroupController(IMedicalServiceGroupService medicalGroupService,
            ILogger<MedicalServiceController> logger)
        {
            _medicalGroupService = medicalGroupService;
            _logger = logger;
        }

        // GET api/<ClinicServiceController>/5
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _medicalGroupService.GetAllMedicalServiceGroups();
                var response = new Response<IEnumerable<MedicalServiceGroupResponseDto>>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<ClinicServiceController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MedicalServiceGroupResponseDto request)
        {
            try
            {
                var result = await _medicalGroupService.CreateMedicalServiceGroup(request);
                var response = new Response<MedicalServiceGroupResponseDto>(result);
                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<ClinicServiceController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] MedicalServiceGroupResponseDto request)
        {
            try
            {
                var result = await _medicalGroupService.EditMedicalServiceGroup(id, request);
                var response = new Response<MedicalServiceGroupResponseDto>(result);
                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<ClinicServiceController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _medicalGroupService.DeleteMedicalServiceGroup(id);
                var response = new Response<string>("Delete successfully");
                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}