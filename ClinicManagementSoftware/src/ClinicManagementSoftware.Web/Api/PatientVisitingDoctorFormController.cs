﻿using System;
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

        // POST api/<PatientController>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] CreateOrUpdatePatientDoctorVisitingFormDto request)
        {
            try
            {
                await _patientDoctorVisitingFormService.CreateVisitingForm(request);
                var response = new Response<string>("Success");
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message + request);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

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
        //            AddressDetail = request.AddressDetail,
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