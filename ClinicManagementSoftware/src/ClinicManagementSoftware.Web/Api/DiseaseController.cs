using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
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
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseService _diseaseService;
        private readonly ILogger<DiseaseController> _logger;

        public DiseaseController(ILogger<DiseaseController> logger, IDiseaseService diseaseService)
        {
            _logger = logger;
            _diseaseService = diseaseService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _diseaseService.GetAll();
                return Ok(new Response<IEnumerable<DiseaseResponseDto>>(result));
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}