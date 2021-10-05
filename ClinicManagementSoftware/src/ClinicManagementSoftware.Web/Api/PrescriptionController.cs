using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Prescription;
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
    [Authorize(Roles = "Doctor")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly ILogger<PrescriptionController> _logger;

        public PrescriptionController(IPrescriptionService prescriptionService,
            ILogger<PrescriptionController> logger)
        {
            _prescriptionService = prescriptionService;
            _logger = logger;
        }

        [HttpGet("getbypatient")]
        public async Task<IActionResult> GetByPatientId(long patientId)
        {
            try
            {
                var result = await _prescriptionService.GetPrescriptionsByPatientId(patientId);
                var response = new Response<ICollection<PrescriptionInformation>>(result);
                
                return Ok(response);
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

        [HttpGet]
        public async Task<IActionResult> GetByClinicId()
        {
            try
            {
                var result = await _prescriptionService.GetPrescriptionsByClinicId();
                var response = new Response<IEnumerable<PatientPrescriptionResponse>>(result);

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _prescriptionService.GetPatientPrescriptionById(id);
                return Ok(new Response<PrescriptionInformation>(result));
            }
            catch (PrescriptionNotFoundException exception)
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

        // POST api/<PrescriptionController>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] CreatePrescriptionDto request)
        {
            try
            {
                await _prescriptionService.CreatePrescription(request);
                return Ok("Create prescription successfully");
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
        public async Task<IActionResult> Put(long id, [FromBody] CreatePrescriptionDto request)
        {
            try
            {
                var result =  await _prescriptionService.EditPrescription(id, request);
                return Ok(new Response<PrescriptionInformation>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception.Message);
            }
            catch (PrescriptionNotFoundException exception)
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
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _prescriptionService.DeleteAsync(id);
                return Ok("Delete successfully");
            }
            catch (PrescriptionNotFoundException exception)
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