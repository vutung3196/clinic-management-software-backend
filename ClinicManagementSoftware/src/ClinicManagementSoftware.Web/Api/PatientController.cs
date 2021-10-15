using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Exceptions.Patient;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Patient;
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
    [Authorize(Roles = "Receptionist")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        //GET: api/<PatientController>
        [HttpGet]
        public async Task<IActionResult> GetAll(string searchName)
        {
            try
            {
                var result = await _patientService.GetAllAsync(searchName);
                return Ok(new Response<IEnumerable<PatientDto>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<PatientController>/5
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _patientService.GetByIdAsync(id);
                return Ok(new Response<PatientDto>(result));
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest("Id should not be null");
            }
            catch (PatientNotFoundException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest($"Cannot find patient with id {id}");
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
        public async Task<IActionResult> Post([FromBody] CreateOrUpdatePatientModel request)
        {
            try
            {
                var createPatientDto = new CreatePatientDto
                {
                    EmailAddress = request.EmailAddress.Trim(),
                    Gender = request.Gender.Trim(),
                    FullName = request.FullName.Trim(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    DateOfBirth = request.DateOfBirth,
                    AddressDetail = request.AddressDetail.Trim(),
                    MedicalInsuranceCode = request.MedicalInsuranceCode.Trim(),
                    AddressCity = request.AddressCity.Trim(),
                    AddressDistrict = request.AddressDistrict.Trim(),
                    AddressStreet = request.AddressStreet.Trim()
                };

                var result = await _patientService.AddAsync(createPatientDto);
                var response = new Response<PatientDto>(result);
                return Ok(response);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest("Create patient model should not be null");
            }
            catch (InvalidGenderException exception)
            {
                _logger.LogError(exception.Message + request);
                return BadRequest("Invalid gender");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message + request);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id:long}")]
        [ValidateModel]
        public async Task<IActionResult> Put([FromRoute] long id, [FromBody] CreateOrUpdatePatientModel request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogError("Request is null");
                    return BadRequest("Create patient model should not be null");
                }

                var updatePatientDto = new UpdatePatientDto
                {
                    Id = id,
                    EmailAddress = request.EmailAddress.Trim(),
                    Gender = request.Gender.Trim(),
                    FullName = request.FullName.Trim(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    DateOfBirth = request.DateOfBirth,
                    AddressDetail = request.AddressDetail.Trim(),
                    MedicalInsuranceCode = request.MedicalInsuranceCode.Trim(),
                    AddressCity = request.AddressCity.Trim(),
                    AddressDistrict = request.AddressDistrict.Trim(),
                    AddressStreet = request.AddressStreet.Trim()
                };
                var result = await _patientService.UpdateAsync(updatePatientDto);
                var response = new Response<PatientDto>(result);
                return Ok(response);
            }
            catch (InvalidGenderException exception)
            {
                _logger.LogError(exception.Message + request);
                return BadRequest("Invalid gender");
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
                await _patientService.DeleteAsync(id);
                return Ok($"Patient with id {id} has been deleted successfully");
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest("id should not be null");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}