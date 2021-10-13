using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Services;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IMedicalImageService _patientMedicalImageService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IMedicalImageService patientMedicalImageService,
            ILogger<FilesController> logger)
        {
            _patientMedicalImageService = patientMedicalImageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long labTestId)
        {
            try
            {
                var result = await _patientMedicalImageService.GetMedicalImageFiles(labTestId);
                var response = result.Select(x => new ImageFileResponse
                {
                    Id = x.MedicalImageFile.Id,
                    PublicId = x.PublicId,
                    CreatedAt = x.CreatedAt.Format(),
                    Name = x.MedicalImageFile.FileName,
                    Url = x.Url,
                    SecureUrl = x.SecureUrl,
                    Description = x.MedicalImageFile.Description
                }).ToList();
                return Ok(new Response<List<ImageFileResponse>>(response));
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


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFileRequest request)
        {
            try
            {
                var result = await _patientMedicalImageService.SaveChanges(request.LabTestId, request.CloudinaryFiles);
                var response = result.Select(x => new ImageFileResponse
                {
                    PublicId = x.PublicId,
                    FilePath = x.MedicalImageFile.FilePath,
                    CreatedAt = x.CreatedAt.Format(),
                    Name = x.FileName,
                    Url = x.Url,
                    SecureUrl = x.SecureUrl
                }).ToList();
                return Ok(new Response<List<ImageFileResponse>>(response));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// PUT api/<FilesController>/5
        //[HttpPut("{id}")]
        //// Update file request
        //public async Task<IActionResult> Put(long id, [FromBody] EditFileRequest request)
        //{
        //    try
        //    {
        //        await _patientMedicalImageService.E(id, request.FileName, request.Description);
        //        return Ok("Updated file successfully");
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        // DELETE api/<FilesController>/5
        // delete file 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _patientMedicalImageService.Delete(id);
                return Ok("Delete file successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}