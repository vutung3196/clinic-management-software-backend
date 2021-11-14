using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Dto.Patient;
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
        private readonly IRepository<LabOrderForm> _labOrderFromRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public PatientHospitalizedProfileService(
            IRepository<PatientHospitalizedProfile> patientHospitalizedProfileRepository, IMapper mapper,
            IUserContext userContext, IRepository<LabOrderForm> labOrderFromRepository,
            IRepository<LabTest> labTestRepository)
        {
            _patientHospitalizedProfileRepository = patientHospitalizedProfileRepository;
            _mapper = mapper;
            _userContext = userContext;
            _labOrderFromRepository = labOrderFromRepository;
            _labTestRepository = labTestRepository;
        }

        public async Task<PatientHospitalizedProfileResponseDto> CreatePatientProfile(
            CreatePatientHospitalizedProfileDto request)
        {
            var patientHospitalizedProfile = new PatientHospitalizedProfile
            {
                CreatedAt = DateTime.Now,
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

            patientHospitalizedProfile.UpdatedAt = DateTime.Now;
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

            var result = new DetailedPatientHospitalizedProfileResponseDto
            {
                DiseaseName = detailedProfile.DiseaseName,
                Code = detailedProfile.Code,
                Description = detailedProfile.Description,
                Prescriptions = detailedProfile.Prescriptions.Select(x =>
                    new PrescriptionInformation
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CreatedAt = x.CreatedAt.Format(),
                        DiagnosedDescription = x.DiagnosedDescription,
                        DoctorSuggestion = x.DoctorSuggestion,
                        DiseaseNote = x.DiseaseNote,
                        DoctorVisitingFormCode = x.PatientDoctorVisitForm?.Code,
                        RevisitDateDisplayed = x.RevisitDate.HasValue ? x.RevisitDate.Format() : null
                    }),
                LabOrderForms = detailedProfile.LabOrderForms.Select(x => new LabOrderFormInformation
                {
                    Id = x.Id,
                    Code = x.Code,
                    CreatedAt = x.CreatedAt.Format(),
                    Description = x.Description,
                    DoctorVisitingFormCode = x.PatientDoctorVisitForm?.Code,
                    DoctorVisitingFormId = x.PatientDoctorVisitFormId,
                    Status = GetLabOrderFormStatus(x.Status),
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

        private static string GetLabOrderFormStatus(byte status)
        {
            return status switch
            {
                (byte) EnumLabOrderFormStatus.NotPaid => "Chưa thanh toán",
                (byte) EnumLabOrderFormStatus.Paid => "Đã thanh toán",
                _ => "Hoàn thành"
            };
        }

        public async Task DeletePatientProfilesByPatientId(long patientId)
        {
            var @spec = new GetPatientHospitalizedProfilesByPatientId(patientId);
            var patientHospitalizedProfiles = await _patientHospitalizedProfileRepository.ListAsync(@spec);
            foreach (var profile in patientHospitalizedProfiles)
            {
                profile.IsDeleted = true;
                profile.DeletedAt = DateTime.Now;
                await _patientHospitalizedProfileRepository.UpdateAsync(profile);
                foreach (var labOrderForm in profile.LabOrderForms)
                {
                    labOrderForm.IsDeleted = true;
                    labOrderForm.DeletedAt = DateTime.Now;
                    await _labOrderFromRepository.UpdateAsync(labOrderForm);
                    foreach (var labTest in labOrderForm.LabTests)
                    {
                        labTest.IsDeleted = true;
                        labTest.DeletedAt = DateTime.Now;
                        await _labTestRepository.UpdateAsync(labTest);
                    }
                }
            }
        }

        public async Task<IEnumerable<PatientHospitalizedProfileResponseDto>> GetAll()
        {
            var currentContext = await _userContext.GetCurrentContext();
            var @spec = new GetPatientHospitalizedProfilesByClinicIdSpec(currentContext.ClinicId);
            var profiles = await _patientHospitalizedProfileRepository.ListAsync(@spec);
            var result = profiles.Select(x => new PatientHospitalizedProfileResponseDto()
            {
                Description = x.Description,
                Code = x.Code,
                CreatedAt = x.CreatedAt.Format(),
                DiseaseName = x.DiseaseName,
                Id = x.Id,

                PatientInformation = _mapper.Map<PatientDto>(x.Patient),
                PatientDetailedInformation = x.Patient.FullName,
                RevisitDate = x.RevisitDate,
                ClinicInformation = new ClinicInformationResponse
                {
                    AddressCity = x.Patient.Clinic.AddressCity,
                    AddressDistrict = x.Patient.Clinic.AddressDistrict,
                    AddressStreet = x.Patient.Clinic.AddressStreet,
                    AddressDetail = x.Patient.Clinic.AddressDetail,
                    Name = x.Patient.Clinic.Name,
                    PhoneNumber = x.Patient.Clinic.PhoneNumber
                }
            });
            return result;
        }
    }
}