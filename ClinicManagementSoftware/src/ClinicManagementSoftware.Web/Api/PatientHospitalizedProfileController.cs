using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;
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
    [Authorize(Roles = "Doctor")]
    public class PatientHospitalizedProfileController : ControllerBase
    {
        private readonly IPatientHospitalizedProfileService _patientHospitalizedProfileService;
        private readonly ILogger<PatientHospitalizedProfileController> _logger;

        public PatientHospitalizedProfileController(
            IPatientHospitalizedProfileService patientHospitalizedProfileService,
            ILogger<PatientHospitalizedProfileController> logger)
        {
            _patientHospitalizedProfileService = patientHospitalizedProfileService;
            _logger = logger;
        }


        // GET api/<ClinicServiceController>/5
        [HttpGet("getbypatient")]
        public async Task<IActionResult> GetByPatientId(long patientId)
        {
            try
            {
                var result =
                    await _patientHospitalizedProfileService.GetPatientHospitalizedProfilesForPatient(patientId);
                var response = new Response<IEnumerable<PatientHospitalizedProfileResponseDto>>(result);
                return Ok(response);
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
                var result =
                    await _patientHospitalizedProfileService.GetDetailedPatientHospitalizedProfile(id);
                var response = new Response<DetailedPatientHospitalizedProfileResponseDto>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result =
                    await _patientHospitalizedProfileService.GetAll();
                var response = new Response<IEnumerable<PatientHospitalizedProfileResponseDto>>(result);
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
        public async Task<IActionResult> Post([FromBody] CreatePatientHospitalizedProfileDto request)
        {
            try
            {
                var result = await _patientHospitalizedProfileService.CreatePatientProfile(request);
                var response = new Response<PatientHospitalizedProfileResponseDto>(result);
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
        public async Task<IActionResult> Put(long id, [FromBody] CreatePatientHospitalizedProfileDto request)
        {
            try
            {
                var result = await _patientHospitalizedProfileService.EditPatientProfile(id, request);
                var response = new Response<PatientHospitalizedProfileResponseDto>(result);
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

        //// DELETE api/<ClinicServiceController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    try
        //    {
        //        await _patientHospitalizedProfileService.DeletePatientProfile(id);
        //        var response = new Response<string>("Delete successfully");
        //        return Ok(response);
        //    }
        //    catch (ArgumentException exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return BadRequest(exception.Message);
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}