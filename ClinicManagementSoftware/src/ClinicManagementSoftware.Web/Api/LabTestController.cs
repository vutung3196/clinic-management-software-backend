using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.LabTest;
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
    [Authorize]
    public class LabTestController : ControllerBase
    {
        private readonly ILabTestService _labTestService;
        private readonly ILogger<LabTestController> _logger;

        public LabTestController(ILabTestService labTestService, ILogger<LabTestController> logger)
        {
            _labTestService = labTestService;
            _logger = logger;
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Doctor,TestSpecialist,Receptionist")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _labTestService.GetLabTestById(id);
                return Ok(new Response<LabTestDto>(result));
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

        [HttpGet("bystatus")]
        [Authorize(Roles = "Doctor,TestSpecialist")]
        public async Task<IActionResult> GetByStatus(byte status)
        {
            try
            {
                var result = await _labTestService.GetLabTestsByStatus(status);
                return Ok(new Response<IEnumerable<LabTestDto>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // PUT api/<PrescriptionController>/5
        // update image
        [HttpPut("{id}")]
        [Authorize(Roles = "TestSpecialist")]
        public async Task<IActionResult> Put(long id, [FromBody] EditLabTestDto request)
        {
            try
            {
                var result = await _labTestService.Edit(id, request);
                return Ok(new Response<UpdateLabTestResponse>(result));
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


        [HttpPut("movetoend")]
        [Authorize(Roles = "TestSpecialist")]
        public async Task<IActionResult> MoveALabTestToTheEndOfAQueue([FromBody] QueueMoveToElementDto request)
        {
            try
            {
                await _labTestService.MoveALabTestToTheEndOfAQueue(request.Id);
                var result = new Response<string>("Move a lab test to the end successfully");
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("movetobeginning")]
        [Authorize(Roles = "TestSpecialist")]
        public async Task<IActionResult> MoveALabTestToTheBeginningOfAQueue([FromBody] QueueMoveToElementDto request)
        {
            try
            {
                await _labTestService.MoveALabTestToTheBeginningOfAQueue(request.Id);
                var result = new Response<string>("Move a lab test to the beginning successfully");
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}