using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Cloudinary;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalImageService : IMedicalImageService
    {
        private readonly IRepository<MedicalImageFile> _medicalImageFileRepository;
        private readonly IRepository<CloudinaryFile> _cloudinaryFileRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public MedicalImageService(IRepository<CloudinaryFile> cloudinaryFileRepository,
            IRepository<MedicalImageFile> medicalImageFileRepository, IRepository<LabTest> labTestRepository,
            ICloudinaryService cloudinaryService)
        {
            _cloudinaryFileRepository = cloudinaryFileRepository;
            _medicalImageFileRepository = medicalImageFileRepository;
            _labTestRepository = labTestRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<CloudinaryFile>> GetMedicalImageFiles(long labTestId)
        {
            var @spec = new GetDetailedLabTestByIdSpec(labTestId);
            var labTest = await _labTestRepository.GetBySpecAsync(@spec);
            if (labTest == null)
            {
                throw new ArgumentException($"Cannot find lab test with id: {labTestId}");
            }

            return labTest.MedicalImageFiles.Select(x => x.CloudinaryFile);
        }

        public async Task<List<CloudinaryFile>> SaveChanges(long labTestId, IList<CloudinaryFieldDto> cloudinaryFields)
        {
            var labTest = await _labTestRepository.GetByIdAsync(labTestId);
            if (labTest == null)
            {
                throw new ArgumentException(
                    $"Cannot find lab test with id: {labTestId}");
            }

            // transformation
            // TODO
            var result = new List<CloudinaryFile>();

            foreach (var cloudinaryField in cloudinaryFields)
            {
                var cloudinaryFile = new CloudinaryFile
                {
                    PublicId = cloudinaryField.PublicId,
                    CreatedAt = DateTime.UtcNow,
                    Bytes = cloudinaryField.Bytes,
                    FileName = cloudinaryField.OriginalFilename,
                    Height = cloudinaryField.Height,
                    Width = cloudinaryField.Width,
                    Url = cloudinaryField.Url,
                    SecureUrl = cloudinaryField.SecureUrl,
                };

                // save to cloudinary file table in database
                cloudinaryFile = await _cloudinaryFileRepository.AddAsync(cloudinaryFile);
                var medicalImageFile = new MedicalImageFile
                {
                    CloudinaryFileId = cloudinaryFile.Id,
                    FileName = cloudinaryField.OriginalFilename,
                    CreatedAt = DateTime.UtcNow,
                    FilePath = string.Empty,
                    LabTestId = labTest.Id,
                };
                // save to medical image file in database
                await _medicalImageFileRepository.AddAsync(medicalImageFile);
                result.Add(cloudinaryFile);
            }

            return result;
        }

        public async Task<MedicalImageFile> EditMedicalImageFile(long id, string name, string description)
        {
            var currentMedicalImageFile = await _medicalImageFileRepository.GetByIdAsync(id);
            if (currentMedicalImageFile == null)
            {
                throw new ArgumentException("Cannot find medical image file with id: {id}");
            }

            currentMedicalImageFile.FileName = name;
            currentMedicalImageFile.Description = description;
            await _medicalImageFileRepository.UpdateAsync(currentMedicalImageFile);
            return currentMedicalImageFile;
        }

        public async Task Delete(long id)
        {
            var spec = new GetImageAndCloudinaryFileSpec(id);
            var currentMedicalImageFile = await _medicalImageFileRepository.GetBySpecAsync(spec);
            if (currentMedicalImageFile == null)
            {
                throw new ArgumentException($"Cannot find medical image file with id: {id}");
            }

            await _medicalImageFileRepository.DeleteAsync(currentMedicalImageFile);
            await _cloudinaryService.DeleteImage(currentMedicalImageFile.CloudinaryFile.PublicId);
        }
    }
}