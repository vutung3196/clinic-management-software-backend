using System;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string role)
        {
            try
            {
                return Ok(role);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// GET api/<PatientController>/5
        //[HttpGet("{id:long}")]
        //public async Task<IActionResult> Get(long id)
        //{
        //    try
        //    {
        //        var result = await _patientDoctorVisitingFormService.GetByIdAsync(id);
        //        return Ok(new Response<PatientDto>(result));
        //    }
        //    catch (ArgumentNullException exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return BadRequest("Id should not be null");
        //    }
        //    catch (PatientNotFoundException exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return BadRequest($"Cannot find patient with id {id}");
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //// POST api/<PatientController>
        //[HttpPost]
        //[ValidateModel]
        //public async Task<IActionResult> Post([FromBody] CreateOrUpdatePatientModel request)
        //{
        //    try
        //    {
        //        var createPatientDto = new CreatePatientDto
        //        {
        //            EmailAddress = request.EmailAddress,
        //            Gender = request.Gender,
        //            FullName = request.FullName,
        //            Occupation = request.Occupation,
        //            PhoneNumber = request.PhoneNumber,
        //            DateOfBirth = request.DateOfBirth,
        //            Address = request.Address
        //        };

        //        var result = await _patientDoctorVisitingFormService.AddAsync(createPatientDto);
        //        var response = new Response<PatientDto>(result);
        //        return Ok(response);
        //    }
        //    catch (ArgumentNullException exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return BadRequest("Create patient model should not be null");
        //    }
        //    catch (InvalidGenderException exception)
        //    {
        //        _logger.LogError(exception.Message + request);
        //        return BadRequest("Invalid gender");
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message + request);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //// PUT api/<PatientController>/5
        //[HttpPut("{id:long}")]
        //[ValidateModel]
        //public async Task<IActionResult> Put([FromRoute] long id, [FromBody] CreateOrUpdatePatientModel request)
        //{
        //    try
        //    {
        //        if (request == null)
        //        {
        //            _logger.LogError("Request is null");
        //            return BadRequest("Create patient model should not be null");
        //        }

        //        var updatePatientDto = new UpdatePatientDto
        //        {
        //            Id = id,
        //            EmailAddress = request.EmailAddress,
        //            Gender = request.Gender,
        //            FullName = request.FullName,
        //            Occupation = request.Occupation,
        //            PhoneNumber = request.PhoneNumber,
        //            Address = request.Address,
        //            DateOfBirth = request.DateOfBirth
        //        };
        //        var result = await _patientDoctorVisitingFormService.UpdateAsync(updatePatientDto);
        //        var response = new Response<PatientDto>(result);
        //        return Ok(response);
        //    }
        //    catch (InvalidGenderException exception)
        //    {
        //        _logger.LogError(exception.Message + request);
        //        return BadRequest("Invalid gender");
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message + request);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //// DELETE api/<PatientController>/5
        //[HttpDelete("{id:long}")]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    try
        //    {
        //        await _patientDoctorVisitingFormService.DeleteAsync(id);
        //        return Ok($"Patient with id {id} has been deleted successfully");
        //    }
        //    catch (ArgumentNullException exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return BadRequest("id should not be null");
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}