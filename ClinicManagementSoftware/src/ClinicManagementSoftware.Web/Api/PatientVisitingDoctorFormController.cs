using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
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
    [Authorize]
    public class PatientVisitingDoctorFormController : ControllerBase
    {
        private readonly ILogger<PatientVisitingDoctorFormController> _logger;
        private readonly IPatientDoctorVisitingFormService _patientDoctorVisitingFormService;

        public PatientVisitingDoctorFormController(ILogger<PatientVisitingDoctorFormController> logger,
            IPatientDoctorVisitingFormService patientDoctorVisitingFormService)
        {
            _logger = logger;
            _patientDoctorVisitingFormService = patientDoctorVisitingFormService;
        }

        [HttpGet("byrole")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _patientDoctorVisitingFormService.GetAllByRole();
                return Ok(new Response<IEnumerable<PatientDoctorVisitingFormDto>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<PatientController>/5
        [HttpGet("doctorAvailability")]
        public async Task<IActionResult> GetDoctorAvailability()
        {
            try
            {
                var result = await _patientDoctorVisitingFormService.GetCurrentDoctorAvailabilities();
                return Ok(new Response<IEnumerable<DoctorAvailabilityDto>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _patientDoctorVisitingFormService.GetById(id);
                return Ok(new Response<PatientDoctorVisitingFormDto>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status404NotFound, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("movetoend")]
        public async Task<IActionResult> MoveAPatientToTheEndOfADoctorQueue([FromBody] QueueMoveToElementDto dto)
        {
            try
            {
                var result =
                    await _patientDoctorVisitingFormService.MoveAVisitingFormToTheEndOfADoctorQueue(dto.Id);
                var response = new Response<PatientDoctorVisitingFormDto>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("movetobeginning")]
        public async Task<IActionResult> MoveAPatientToTheBeginningOfADoctorQueue([FromBody] QueueMoveToElementDto dto)
        {
            try
            {
                var result =
                    await _patientDoctorVisitingFormService.MoveAVisitingFormToTheBeginningOfADoctorQueue(dto.Id);
                var response = new Response<PatientDoctorVisitingFormDto>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<PatientController>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] CreateOrUpdatePatientDoctorVisitingFormDto request)
        {
            try
            {
                var result = await _patientDoctorVisitingFormService.CreateVisitingForm(request);
                var response = new Response<CreateVisitingFormResponse>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message + request);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// PUT api/<PatientController>/5
        [HttpPut("{id:long}")]
        [ValidateModel]
        public async Task<IActionResult> Put([FromRoute] long id,
            [FromBody] CreateOrUpdatePatientDoctorVisitingFormDto request)
        {
            try
            {
                var result = await _patientDoctorVisitingFormService.EditVisitingForm(id, request);
                var response = new Response<PatientDoctorVisitingFormDto>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message + request);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _patientDoctorVisitingFormService.DeleteById(id);
                return Ok($"Visiting form with id {id} has been deleted successfully");
            }
            catch (ArgumentNullException exception)
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