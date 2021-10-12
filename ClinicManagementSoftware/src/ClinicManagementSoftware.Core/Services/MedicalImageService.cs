using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalImageService : IMedicalImageService
    {
        private readonly IRepository<MedicalImageFile> _medicalImageFileRepository;
        private readonly IRepository<CloudinaryFile> _cloudinaryFileRepository;
        private readonly IRepository<LabTest> _labTestRepository;

        public MedicalImageService(IRepository<CloudinaryFile> cloudinaryFileRepository,
            IRepository<MedicalImageFile> medicalImageFileRepository, IRepository<LabTest> labTestRepository)
        {
            _cloudinaryFileRepository = cloudinaryFileRepository;
            _medicalImageFileRepository = medicalImageFileRepository;
            _labTestRepository = labTestRepository;
        }


        //public async Task ProcessMedicalImageFile(long patientId,
        //    IList<MedicalImageFile> medicalImageFiles)
        //{
        //    var spec = new GetPatientHospitalizedProfileBasedOnPatientIdSpec(patientId);
        //    var patientHospitalizedProfile =
        //        await _patientHospitalizedProfileService.GetPatientHospitalizedProfile(patientId);
        //    if (patientHospitalizedProfile == null)
        //    {
        //        throw new PatientHospitalizedProfileNotFoundException(
        //            $"Cannot find hospitalized profile with {patientId}");
        //    }

        //    // save to local dish
        //    var paths = medicalImageFiles
        //        .Select(image => SaveFileToLocalDish(image.MedicalImageFileBase64)).ToList();

        //    // send paths and patient prescription id to rabbit mq
        //    var data = new CloudinaryFileData
        //    {
        //        PatientHospitalizedProfileId = patientHospitalizedProfile.Id,
        //        FilePaths = paths,
        //        ClinicName = "hapham"
        //    };

        //    // create event
        //    var @event = new CreateCloudinaryFileEvent(data);
        //    // publish event
        //    _eventBus.Publish(@event);
        //}

        public async Task<List<CloudinaryFile>> GetMedicalImageFiles(long patientId)
        {
            throw new NotImplementedException();
            //var patientHospitalizedProfile =
            //    await _patientHospitalizedProfileService.GetPatientHospitalizedProfile(patientId);
            //if (patientHospitalizedProfile == null)
            //{
            //    throw new PatientHospitalizedProfileNotFoundException(
            //        $"Cannot find hospitalized profile with {patientId}");
            //}

            //return patientHospitalizedProfile.PatientMedicalImageFiles
            //    .Select(x => x.CloudinaryFile)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .ThenByDescending(x => x.CreatedAt)
            //    .ToList();
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
                    PatientHospitalizedProfileId = labTest.Id
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
            throw new NotImplementedException();
            //var spec = new GetImageAndCloudinaryFileSpec(id);
            //var currentMedicalImageFile = await _medicalImageFileRepository.GetBySpecAsync(spec);
            //if (currentMedicalImageFile == null)
            //{
            //    throw new PatientMedicalImageFileNotFoundException("Cannot find medical image file with id: {id}");
            //}

            //await _medicalImageFileRepository.DeleteAsync(currentMedicalImageFile);
            //if (currentMedicalImageFile.CloudinaryFile != null)
            //{
            //    var @event = new DeleteCloudinaryFileEvent(new DeleteCloudinaryFileData()
            //    {
            //        PublicId = currentMedicalImageFile.CloudinaryFile.PublicId
            //    });
            //    _eventBus.Publish(@event);
            //}
        }
    }
}