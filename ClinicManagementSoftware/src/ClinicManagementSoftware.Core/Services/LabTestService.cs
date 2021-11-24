using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Dto.LabTest;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly IUserContext _userContext;
        private readonly ILabTestQueueService _labTestQueueService;
        private readonly IMapper _mapper;
        private readonly IRepository<LabOrderForm> _labOrderFormRepository;
        private readonly IRepository<PatientDoctorVisitForm> _doctorVisitingFormRepository;

        public LabTestService(IRepository<LabTest> labTestRepository,
            IUserContext userContext,
            ILabTestQueueService labTestQueueService, IMapper mapper, IRepository<LabOrderForm> labOrderFormRepository,
            IRepository<PatientDoctorVisitForm> doctorVisitingFormRepository)
        {
            _labTestRepository = labTestRepository;
            _userContext = userContext;
            _labTestQueueService = labTestQueueService;
            _mapper = mapper;
            _labOrderFormRepository = labOrderFormRepository;
            _doctorVisitingFormRepository = doctorVisitingFormRepository;
        }


        public async Task<LabTestDto> GetLabTestById(long id)
        {
            var @spec = new GetDetailedLabTestByIdSpec(id);
            var labTest = await _labTestRepository.GetBySpecAsync(@spec);

            if (labTest == null)
            {
                throw new ArgumentException($"Cannot find a lab test with id: {id}");
            }

            return new LabTestDto
            {
                Description = labTest.Description,
                LabOrderFormCode = labTest.LabOrderForm.Code,
                CreatedAt = labTest.CreatedAt.Format(),
                DoctorName = labTest.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = labTest.LabOrderForm.PatientDoctorVisitFormId,
                PatientInformation = _mapper.Map<PatientDto>(labTest.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = labTest.LabOrderFormId,
                MedicalServiceName = labTest.MedicalService.Name,
                // TODO implement image files later 
                ImageFiles = GetCurrentImageFilesForLabTest(labTest.MedicalImageFiles.Select(X => X.CloudinaryFile)),
                Id = labTest.Id,
                ClinicInformation = new ClinicInformationResponse
                {
                    AddressCity = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressCity,
                    AddressDistrict = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressDistrict,
                    AddressStreet = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressStreet,
                    AddressDetail = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressDetail,
                    Name = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.Name,
                    PhoneNumber = labTest.LabOrderForm.PatientHospitalizedProfile.Patient.Clinic.PhoneNumber
                },
                Result = labTest.Result,
                StatusDisplayed = GetLabTestStatus(labTest.Status),
                Status = labTest.Status
            };
        }

        public async Task<IEnumerable<LabTestDto>> GetCurrentLabTestsNeedToBePerformed(
            CurrentUserContext currentContext)
        {
            if (!currentContext.MedicalServiceGroupForTestSpecialistId.HasValue)
            {
                throw new AuthenticationException("Test specialist should have medical service group");
            }

            var currentLabTestIds = await _labTestQueueService.GetCurrentLabTestQueue(currentContext.ClinicId,
                currentContext.MedicalServiceGroupForTestSpecialistId.Value);
            var ids = currentLabTestIds.ToArray();
            var @spec = new GetDetailedLabTestsInIdsSpec(ids);
            var unOrderLabTests = await _labTestRepository.ListAsync(@spec);

            var idToLabTest =
                unOrderLabTests.ToDictionary(doctorVisitForm => doctorVisitForm.Id, x => x);

            var orderedLabTests = ids.Select(visitingFormId => idToLabTest[visitingFormId])
                .ToList();
            var index = 1;
            var result = orderedLabTests.Select(x => new LabTestDto
            {
                Index = index++,
                Id = x.Id,
                Result = x.Result,
                MedicalServiceName = x.MedicalService.Name,
                StatusDisplayed = GetLabTestStatus(x.Status),
                Description = x.Description,
                LabOrderFormCode = x.LabOrderForm.Code,
                CreatedAt = x.CreatedAt.Format(),
                DoctorName = x.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = x.LabOrderForm.PatientDoctorVisitFormId,
                PatientInformation = _mapper.Map<PatientDto>(x.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = x.LabOrderFormId,
                ImageFiles = GetCurrentImageFilesForLabTest(x.MedicalImageFiles.Select(x => x.CloudinaryFile)),
                Status = x.Status
            });
            return result;
        }

        private static IEnumerable<ImageFileResponse> GetCurrentImageFilesForLabTest(
            IEnumerable<CloudinaryFile> files)
        {
            return files.Select(x => new ImageFileResponse
            {
                Id = x.MedicalImageFile.Id,
                PublicId = x.PublicId,
                CreatedAt = x.CreatedAt.Format(),
                Name = x.MedicalImageFile.FileName,
                Url = x.Url,
                SecureUrl = x.SecureUrl,
                Description = x.MedicalImageFile.Description
            });
        }

        public async Task<IEnumerable<LabTestDto>> GetLabTestsByStatus(byte status)
        {
            var currentContext = await _userContext.GetCurrentContext();
            if (status == (byte) EnumLabTestStatus.WaitingForTesting)
            {
                return await GetCurrentLabTestsNeedToBePerformed(currentContext);
            }

            if (!currentContext.MedicalServiceGroupForTestSpecialistId.HasValue)
            {
                throw new AuthenticationException("Test specialist should have medical service group");
            }

            var @spec = new GetDetailedLabTestsByStatusAndClinicAndMedicalServiceGroupSpec(status,
                currentContext.ClinicId, currentContext.MedicalServiceGroupForTestSpecialistId.Value);
            var labTests = await _labTestRepository.ListAsync(@spec);
            var result = labTests.Select(x => new LabTestDto()
            {
                Description = x.Description,
                LabOrderFormCode = x.LabOrderForm.Code,
                CreatedAt = x.CreatedAt.Format(),
                DoctorName = x.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = x.LabOrderForm.PatientDoctorVisitFormId,
                MedicalServiceName = x.MedicalService.Name,
                PatientInformation = _mapper.Map<PatientDto>(x.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = x.LabOrderFormId,
                ImageFiles = GetCurrentImageFilesForLabTest(x.MedicalImageFiles.Select(x => x.CloudinaryFile)),
                Id = x.Id,
                Result = x.Result,
                StatusDisplayed = GetLabTestStatus(x.Status),
                Status = x.Status
            });
            return result;
        }

        public async Task MoveALabTestToTheEndOfAQueue(long labTestId)
        {
            var currentContext = await _userContext.GetCurrentContext();
            await _labTestQueueService.MoveALabTestToTheEndOfTheQueue(labTestId, currentContext.ClinicId);
            var labTest = await _labTestRepository.GetByIdAsync(labTestId);
            if (labTest == null)
            {
                throw new ArgumentException($"lab test is not found with patientId: {labTestId}");
            }

            labTest.UpdatedAt = DateTime.Now;
            await _labTestRepository.UpdateAsync(labTest);
        }

        public async Task MoveALabTestToTheBeginningOfAQueue(long labTestId)
        {
            var currentContext = await _userContext.GetCurrentContext();
            await _labTestQueueService.MoveALabTestToTheBeginningOfTheQueue(labTestId, currentContext.ClinicId);
            var labTest = await _labTestRepository.GetByIdAsync(labTestId);
            if (labTest == null)
            {
                throw new ArgumentException($"lab test is not found with patientId: {labTestId}");
            }

            labTest.UpdatedAt = DateTime.Now;
            await _labTestRepository.UpdateAsync(labTest);
        }

        private static string GetLabTestStatus(byte status)
        {
            return status switch
            {
                (byte) EnumLabTestStatus.NotPaid => "Chưa thanh toán",
                (byte) EnumLabTestStatus.WaitingForTesting => "Đang chờ xét nghiệm",
                (byte) EnumLabTestStatus.WaitingForResult => "Đang chờ kết quả",
                (byte) EnumLabTestStatus.Done => "Đã có kết quả",
                _ => ""
            };
        }

        public async Task<UpdateLabTestResponse> Edit(long id, EditLabTestDto request)
        {
            var result = new UpdateLabTestResponse();
            var @spec = new GetDetailedLabTestByIdSpec(id);
            var currentLabTest = await _labTestRepository.GetBySpecAsync(@spec);
            var currentContext = await _userContext.GetCurrentContext();

            if (!currentContext.MedicalServiceGroupForTestSpecialistId.HasValue)
            {
                throw new AuthenticationException("Test specialist should have medical service group");
            }

            if (currentLabTest == null)
            {
                throw new ArgumentException($"Cannot find a lab test with id: {id}");
            }

            switch (request.Status)
            {
                case (byte) EnumLabTestStatus.WaitingForResult
                    when currentLabTest.Status == (byte) EnumLabTestStatus.WaitingForTesting:
                    result.IsMovingFromWaitingForTestingStatusToWaitingForResultStatus = true;
                    break;
                case (byte) EnumLabTestStatus.Done
                    when currentLabTest.Status == (byte) EnumLabTestStatus.WaitingForResult:
                    result.IsMovingFromWaitingForResultStatusToDoneStatus = true;
                    break;
                case (byte) EnumLabTestStatus.Done
                    when currentLabTest.Status == (byte) EnumLabTestStatus.WaitingForTesting:
                    result.IsMovingFromWaitingForTestingStatusToDoneStatus = true;
                    break;
            }

            if (result.IsMovingFromWaitingForResultStatusToDoneStatus && string.IsNullOrWhiteSpace(request.Result))
            {
                throw new ArgumentException("Cần ghi kết quả xét nghiệm");
            }

            // update lab test queue for 1 status please
            if (currentLabTest.Status == (byte) EnumLabTestStatus.WaitingForTesting
                && request.Status != (byte) EnumLabTestStatus.WaitingForTesting)
            {
                await _labTestQueueService.DeleteALabTestInQueue(id, currentContext.ClinicId,
                    currentContext.MedicalServiceGroupForTestSpecialistId.Value);
            }

            currentLabTest.Result = request.Result;
            currentLabTest.Status = request.Status;
            await _labTestRepository.UpdateAsync(currentLabTest);

            // update lab test order form status
            var @labOrderFormSpec = new GetLabOrderFormAndLabTestsByIdSpec(currentLabTest.LabOrderFormId);
            var currentLabOrderForm = await _labOrderFormRepository.GetBySpecAsync(@labOrderFormSpec);

            if (currentLabOrderForm == null)
            {
                throw new ArgumentException("Cannot find lab order form of this lab test");
            }

            if (currentLabOrderForm.LabTests.All(x => x.Status == (byte) EnumLabTestStatus.Done))
            {
                // update doctor visiting form status
                currentLabOrderForm.Status = (byte) EnumLabOrderFormStatus.Done;
                await _labOrderFormRepository.UpdateAsync(currentLabOrderForm);
                var currentDoctorVisitingForm = currentLabOrderForm.PatientDoctorVisitForm;
                currentDoctorVisitingForm.VisitingStatus = (byte) EnumDoctorVisitingFormStatus.WaitingForDoctor;
                await _doctorVisitingFormRepository.UpdateAsync(currentDoctorVisitingForm);
                result.IsLabOrderFormDone = true;
            }

            result.LabTests = await GetLabTestsByStatus(request.CurrentPageStatus);
            return result;
        }
    }
}