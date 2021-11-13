using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Exceptions.Clinic;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Web.ApiModels.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSoftware.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize]
        [Authorize(Roles = "Doctor,TestSpecialist")]
        public async Task<IActionResult> Get(long labTestId)
        {
            try
            {
                var result = await _patientMedicalImageService.GetMedicalImageFilesOfALabTest(labTestId);
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

        [HttpGet("byvisitingform")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetByVisitingForm(long visitingFormId)
        {
            try
            {
                var result = await _patientMedicalImageService.GetMedicalImageFilesByVisitingFormId(visitingFormId);
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
        [Authorize]
        [Authorize(Roles = "TestSpecialist")]
        public async Task<IActionResult> Post([FromBody] CreateFileRequest request)
        {
            try
            {
                var result =
                    await _patientMedicalImageService.CreateFileImagesForLabTest(request.LabTestId,
                        request.CloudinaryFiles);
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

        [HttpPost("cliniclogo")]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadLogo([FromBody] CreateClinicLogoRequest request)
        {
            try
            {
                var result = await _patientMedicalImageService.CreateImageLogoForClinic(request.CloudinaryFile);
                var response = new ImageFileResponse
                {
                    PublicId = result.PublicId,
                    CreatedAt = result.CreatedAt.Format(),
                    Name = result.FileName,
                    Url = result.Url,
                    SecureUrl = result.SecureUrl
                };
                return Ok(new Response<ImageFileResponse>(response));
            }
            catch (ClinicNotFoundException exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status401Unauthorized, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<FilesController>/5
        // delete file 
        [HttpDelete("{request}")]
        [Authorize(Roles = "TestSpecialist")]
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

        [HttpDelete("cloudinary")]
        public async Task<IActionResult> DeleteCloudinaryFile([FromBody] DeleteCloudinaryFileRequest request)
        {
            try
            {
                await _patientMedicalImageService.DeleteCloudinaryFile(request.PublicId);
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