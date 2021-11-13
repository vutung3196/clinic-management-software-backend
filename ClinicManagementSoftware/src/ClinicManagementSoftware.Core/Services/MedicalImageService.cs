using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Cloudinary;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Exceptions.Clinic;
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
        private readonly IRepository<PatientDoctorVisitForm> _visitingFormRepository;
        private readonly IUserContext _userContext;
        private readonly IRepository<Clinic> _clinicRepository;

        public MedicalImageService(IRepository<CloudinaryFile> cloudinaryFileRepository,
            IRepository<MedicalImageFile> medicalImageFileRepository, IRepository<LabTest> labTestRepository,
            ICloudinaryService cloudinaryService, IRepository<PatientDoctorVisitForm> visitingFormRepository,
            IUserContext userContext, IRepository<Clinic> clinicRepository)
        {
            _cloudinaryFileRepository = cloudinaryFileRepository;
            _medicalImageFileRepository = medicalImageFileRepository;
            _labTestRepository = labTestRepository;
            _cloudinaryService = cloudinaryService;
            _visitingFormRepository = visitingFormRepository;
            _userContext = userContext;
            _clinicRepository = clinicRepository;
        }

        public async Task<IEnumerable<CloudinaryFile>> GetMedicalImageFilesOfALabTest(long labTestId)
        {
            var @spec = new GetDetailedLabTestByIdSpec(labTestId);
            var labTest = await _labTestRepository.GetBySpecAsync(@spec);
            if (labTest == null)
            {
                throw new ArgumentException($"Cannot find lab test with id: {labTestId}");
            }

            return labTest.MedicalImageFiles.Select(x => x.CloudinaryFile);
        }

        public async Task<IEnumerable<CloudinaryFile>> GetMedicalImageFilesByVisitingFormId(long visitingFormId)
        {
            var @spec = new GetMedicalImageFilesByVisitingFormIdSpec(visitingFormId);
            var visitingForm = await _visitingFormRepository.GetBySpecAsync(@spec);
            if (visitingForm == null)
            {
                throw new ArgumentException($"Cannot find lab test with id: {visitingFormId}");
            }

            var medicalImageFiles = visitingForm.LabOrderForms.SelectMany(x => x.LabTests)
                .SelectMany(x => x.MedicalImageFiles);
            return medicalImageFiles.Select(imageFile => imageFile.CloudinaryFile);
        }

        public async Task<List<CloudinaryFile>> CreateFileImagesForLabTest(long labTestId,
            IList<CloudinaryFieldDto> cloudinaryFields)
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
                    CreatedAt = DateTime.Now,
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
                    CreatedAt = DateTime.Now,
                    FilePath = string.Empty,
                    LabTestId = labTest.Id,
                };
                // save to medical image file in database
                await _medicalImageFileRepository.AddAsync(medicalImageFile);
                result.Add(cloudinaryFile);
            }

            return result;
        }

        public async Task<CloudinaryFile> CreateImageLogoForClinic(CloudinaryFieldDto cloudinaryField)
        {
            var currentUser = await _userContext.GetCurrentContext();
            var cloudinaryFile = new CloudinaryFile
            {
                PublicId = cloudinaryField.PublicId,
                CreatedAt = DateTime.Now,
                Bytes = cloudinaryField.Bytes,
                FileName = cloudinaryField.OriginalFilename,
                Height = cloudinaryField.Height,
                Width = cloudinaryField.Width,
                Url = cloudinaryField.Url,
                SecureUrl = cloudinaryField.SecureUrl,
            };

            var currentClinic = await _clinicRepository.GetByIdAsync(currentUser.ClinicId);
            if (currentClinic == null)
            {
                throw new ClinicNotFoundException($"Cannot find clinic with id {currentUser.ClinicId}");
            }


            // save to cloudinary file table in database
            cloudinaryFile = await _cloudinaryFileRepository.AddAsync(cloudinaryFile);
            // update clinic
            currentClinic.CloudinaryFileId = cloudinaryFile.Id;
            await _clinicRepository.UpdateAsync(currentClinic);
            return cloudinaryFile;
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

        public async Task DeleteCloudinaryFile(string publicId)
        {
            await _cloudinaryService.DeleteImage(publicId);
        }
    }
}