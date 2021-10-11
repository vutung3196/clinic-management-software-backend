using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;
using ClinicManagementSoftware.Core.Dto.Prescription;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class PatientHospitalizedProfileService : IPatientHospitalizedProfileService
    {
        private readonly IRepository<PatientHospitalizedProfile> _patientHospitalizedProfileRepository;
        private readonly IMapper _mapper;

        public PatientHospitalizedProfileService(
            IRepository<PatientHospitalizedProfile> patientHospitalizedProfileRepository, IMapper mapper)
        {
            _patientHospitalizedProfileRepository = patientHospitalizedProfileRepository;
            _mapper = mapper;
        }

        public Task<PatientHospitalizedProfileResponseDto> GetPatientProfilesByPatientId(long patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientHospitalizedProfileResponseDto> CreatePatientProfile(
            CreatePatientHospitalizedProfileDto request)
        {
            var patientHospitalizedProfile = new PatientHospitalizedProfile
            {
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Description = request.Description,
                DiseaseName = request.DiseaseName,
                PatientId = request.PatientId,
                RevisitDate = request.RevisitDate,
                Code = request.Code,
            };
            patientHospitalizedProfile =
                await _patientHospitalizedProfileRepository.AddAsync(patientHospitalizedProfile);
            return new PatientHospitalizedProfileResponseDto
            {
                Description = patientHospitalizedProfile.Description,
                DiseaseName = patientHospitalizedProfile.DiseaseName,
                PatientId = patientHospitalizedProfile.PatientId,
                Id = patientHospitalizedProfile.Id,
                RevisitDate = patientHospitalizedProfile.RevisitDate,
                Code = request.Code,
            };
        }

        public async Task<PatientHospitalizedProfileResponseDto> EditPatientProfile(long id,
            CreatePatientHospitalizedProfileDto request)
        {
            var patientHospitalizedProfile = await _patientHospitalizedProfileRepository.GetByIdAsync(id);
            if (patientHospitalizedProfile == null)
            {
                throw new ArgumentException($"Cannot find patient hospitalized profile with id: {id}");
            }

            patientHospitalizedProfile.UpdatedAt = DateTime.UtcNow;
            patientHospitalizedProfile.Description = request.Description;
            patientHospitalizedProfile.DiseaseName = request.DiseaseName;
            patientHospitalizedProfile.RevisitDate = request.RevisitDate;
            await _patientHospitalizedProfileRepository.UpdateAsync(patientHospitalizedProfile);
            return new PatientHospitalizedProfileResponseDto()
            {
                Description = patientHospitalizedProfile.Description,
                DiseaseName = patientHospitalizedProfile.DiseaseName,
                PatientId = patientHospitalizedProfile.PatientId,
                Id = patientHospitalizedProfile.Id,
                RevisitDate = patientHospitalizedProfile.RevisitDate
            };
        }

        public async Task DeletePatientProfile(long id)
        {
            var patientHospitalizedProfile = await _patientHospitalizedProfileRepository.GetByIdAsync(id);
            if (patientHospitalizedProfile == null)
            {
                throw new ArgumentException($"Cannot find patient hospitalized profile with id: {id}");
            }

            await _patientHospitalizedProfileRepository.DeleteAsync(patientHospitalizedProfile);
        }

        public async Task<IEnumerable<PatientHospitalizedProfileResponseDto>> GetPatientHospitalizedProfilesForPatient(
            long patientId)
        {
            var @spec = new GetPatientHospitalizedProfilesByPatientId(patientId);
            var patientHospitalizedProfiles = await _patientHospitalizedProfileRepository.ListAsync(@spec);

            return patientHospitalizedProfiles.OrderByDescending(x => x.RevisitDate).Select(
                patientHospitalizedProfile =>
                    new PatientHospitalizedProfileResponseDto
                    {
                        Description = patientHospitalizedProfile.Description,
                        DiseaseName = patientHospitalizedProfile.DiseaseName,
                        PatientId = patientHospitalizedProfile.PatientId,
                        Id = patientHospitalizedProfile.Id,
                        RevisitDate = patientHospitalizedProfile.RevisitDate,
                        Code = patientHospitalizedProfile.Code,
                        CreatedAt = patientHospitalizedProfile.CreatedAt.Format()
                    });
        }

        public async Task<DetailedPatientHospitalizedProfileResponseDto> GetDetailedPatientHospitalizedProfile(long id)
        {
            var spec = new GetDetailedPatientHospitalizedProfileByIdSpec(id);
            var detailedProfile = await _patientHospitalizedProfileRepository.GetBySpecAsync(spec);
            if (detailedProfile == null)
            {
                throw new ArgumentException($"Cannot find detailed profile with id: {id}");
            }

            var labTests = detailedProfile.LabOrderForms.SelectMany(x => x.LabTests);

            var result = new DetailedPatientHospitalizedProfileResponseDto()
            {
                Prescriptions = detailedProfile.Prescriptions.Select(x => _mapper.Map<PrescriptionInformation>(x)),
                LabOrderForms = detailedProfile.LabOrderForms.Select(x => new LabOrderFormInformation
                {
                    Id = x.Id,
                    Code = x.Code,
                    CreatedAt = x.CreatedAt.Format(),
                    Description = x.Description,
                    DoctorVisitingFormCode = x.PatientDoctorVisitForm?.Code,
                    DoctorVisitingFormId = x.PatientDoctorVisitFormId,
                }),
                LabTests = labTests.Select(x =>
                    new LabTestInformation
                    {
                        Id = x.Id,
                        Result = x.Result,
                        Name = x.MedicalService.Name,
                        CreatedAt = x.CreatedAt.Format(),
                        Description = x.Description,
                        Status = GetLabTestStatus(x.Status),
                        ImageFiles = x.MedicalImageFiles?.Select(medicalImageFile => new ImageFileResponse
                        {
                            Id = medicalImageFile?.Id,
                            PublicId = medicalImageFile?.CloudinaryFile?.PublicId,
                            CreatedAt = medicalImageFile?.CreatedAt.Format(),
                            Name = medicalImageFile?.FileName,
                            Url = medicalImageFile?.CloudinaryFile?.Url,
                            SecureUrl = medicalImageFile?.CloudinaryFile?.SecureUrl,
                            Description = medicalImageFile?.Description
                        }),
                    }),
            };
            return result;
        }

        private static string GetLabTestStatus(byte status)
        {
            return status == (byte) EnumLabTestStatus.Done ? "Đã có kết quả" : "Chưa có kết quả";
        }

        public async Task DeletePatientProfilesByPatientId(long patientId)
        {
            var @spec = new GetPatientHospitalizedProfilesByPatientId(patientId);
            var patientHospitalizedProfiles = await _patientHospitalizedProfileRepository.ListAsync(@spec);
            foreach (var profile in patientHospitalizedProfiles)
            {
                profile.IsDeleted = true;
                profile.DeletedAt = DateTime.UtcNow;
                await _patientHospitalizedProfileRepository.UpdateAsync(profile);
            }
        }
    }
}