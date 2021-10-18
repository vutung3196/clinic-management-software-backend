using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.LabOrderForm;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;
using ClinicManagementSoftware.Core.Exceptions.Patient;
using ClinicManagementSoftware.Core.Exceptions.Prescription;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using ClinicManagementSoftware.Web.Filters;
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
    public class LabOrderFormController : ControllerBase
    {
        private readonly ILabOrderFormService _labOrderFormService;
        private readonly ILogger<LabOrderFormController> _logger;

        public LabOrderFormController(ILabOrderFormService labOrderFormService,
            ILogger<LabOrderFormController> logger)
        {
            _labOrderFormService = labOrderFormService;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetByClinicId()
        //{
        //    try
        //    {
        //        var result = await _labOrderFormService.GetByStatus();
        //        var response = new Response<IEnumerable<PatientPrescriptionResponse>>(result);

        //        return Ok(response);
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _labOrderFormService.GetLabOrderFormById(id);
                return Ok(new Response<LabOrderFormDto>(result));
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

        [HttpGet("byrole")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _labOrderFormService.GetAllByRole();
                return Ok(new Response<IEnumerable<LabOrderFormDto>>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<PrescriptionController>
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Post([FromBody] CreateOrEditLabOrderFormDto request)
        {
            try
            {
                var result = await _labOrderFormService.CreateLabOrderForm(request);
                return Ok(new Response<long>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (PatientNotFoundException exception)
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

        [HttpPost("pay/{id}")]
        [ValidateModel]
        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> CreatePayment(long id, [FromBody] CreatePaymentForLabOrderFormDto request)
        {
            try
            {
                var result = await _labOrderFormService.PayLabOrderForm(id, request);
                return Ok(new Response<long>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (PatientNotFoundException exception)
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


        // PUT api/<PrescriptionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CreateOrEditLabOrderFormDto request)
        {
            try
            {
                await _labOrderFormService.EditLabOrderForm(id, request);
                return Ok("Update successfully");
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

        // DELETE api/<PrescriptionController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _labOrderFormService.DeleteLabOrderForm(id);
                return Ok("Delete successfully");
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