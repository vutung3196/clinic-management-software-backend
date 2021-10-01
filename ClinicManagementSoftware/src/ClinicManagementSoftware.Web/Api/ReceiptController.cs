using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Receipt;
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
    [Authorize(Roles="Accountant")]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        private readonly ILogger<ReceiptController> _logger;

        public ReceiptController(ILogger<ReceiptController> logger,
            IReceiptService receiptService)
        {
            _logger = logger;
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _receiptService.GetAll();
                return Ok(new Response<IEnumerable<ReceiptResponse>>(result));
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
                var result = await _receiptService.GetReceiptById(id);
                return Ok(new Response<ReceiptResponse>(result));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // DELETE api/<ClinicServiceController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _receiptService.Delete(id);
                return Ok("Delete successfully");
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}