using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Dto.Medication;
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
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;
        private readonly ILogger<MedicalServiceController> _logger;

        public MedicationController(IMedicationService medicationService,
            ILogger<MedicalServiceController> logger)
        {
            _medicationService = medicationService;
            _logger = logger;
        }


        // GET api/<ClinicServiceController>/5
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _medicationService.GetAllMedications();
                var response = new Response<IEnumerable<MedicationGroupDto>>(result);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}