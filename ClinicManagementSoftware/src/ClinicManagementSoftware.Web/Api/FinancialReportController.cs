using System;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.FinancialReport;
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
    public class FinancialReportController : ControllerBase
    {
        private readonly ILogger<FinancialReportController> _logger;
        private readonly IClinicReportService _clinicReportService;

        public FinancialReportController(ILogger<FinancialReportController> logger,
            IClinicReportService clinicReportService)
        {
            _logger = logger;
            _clinicReportService = clinicReportService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _clinicReportService.Get(startDate, endDate);
                return Ok(new Response<FinancialReportResponse>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}