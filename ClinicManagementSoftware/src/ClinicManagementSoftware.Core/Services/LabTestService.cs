using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Dto.LabTest;
using ClinicManagementSoftware.Core.Dto.Patient;
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
        private readonly ILabOrderFormService _labOrderFormService;
        private readonly IUserContext _userContext;
        private readonly ILabTestQueueService _labTestQueueService;
        private readonly IMapper _mapper;

        public LabTestService(IRepository<LabTest> labTestRepository,
            ILabOrderFormService labOrderFormService, IUserContext userContext,
            ILabTestQueueService labTestQueueService, IMapper mapper)
        {
            _labTestRepository = labTestRepository;
            _labOrderFormService = labOrderFormService;
            _userContext = userContext;
            _labTestQueueService = labTestQueueService;
            _mapper = mapper;
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
                // TODO implement image files later 
                ImageFiles = GetCurrentImageFilesForLabTest(),
                Id = labTest.Id,
                Result = labTest.Result,
                Status = GetLabTestStatus(labTest.Status),
            };
        }

        public async Task<IEnumerable<LabTestDto>> GetAllByRole()
        {
            var labOrderForms = await _labOrderFormService.GetAllByRole();
            var a = labOrderForms.Select(x => x.LabTests).ToList();
            var labTests = labOrderForms.Select(x => new LabTestDto()
            {
                //Description = x.
            });
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<LabTestDto>> GetCurrentLabTestsNeedToBePerformed()
        {
            var currentContext = await _userContext.GetCurrentContext();
            var currentLabTestIds = await _labTestQueueService.GetCurrentLabTestQueue(currentContext.ClinicId);
            var ids = currentLabTestIds.ToArray();
            var @spec = new GetDetailedLabTestsInIdsSpec(ids);
            var labTests = await _labTestRepository.ListAsync(@spec);
            var index = 1;
            var result = labTests.Select(x => new LabTestDto()
            {
                Index = index++,
                Id = x.Id,
                Result = x.Result,
                Status = GetLabTestStatus(x.Status),
                Description = x.Description,
                LabOrderFormCode = x.LabOrderForm.Code,
                CreatedAt = x.CreatedAt.Format(),
                DoctorName = x.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = x.LabOrderForm.PatientDoctorVisitFormId,
                PatientInformation = _mapper.Map<PatientDto>(x.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = x.LabOrderFormId,
                // TODO implement image files later 
                ImageFiles = GetCurrentImageFilesForLabTest(),
            });
            return result;
        }

        private static IEnumerable<ImageFileResponse> GetCurrentImageFilesForLabTest()
        {
            return null;
        }

        public async Task<IEnumerable<LabTestDto>> GetLabTestsByStatus(byte status)
        {
            var currentContext = await _userContext.GetCurrentContext();
            if (status == (byte) EnumLabTestStatus.WaitingForTesting)
            {
                return await GetCurrentLabTestsNeedToBePerformed();
            }

            var @spec = new GetDetailedLabTestsByStatusAndClinicSpec(status, currentContext.ClinicId);
            var labTests = await _labTestRepository.ListAsync(@spec);
            var result = labTests.Select(x => new LabTestDto()
            {
                Description = x.Description,
                LabOrderFormCode = x.LabOrderForm.Code,
                CreatedAt = x.CreatedAt.Format(),
                DoctorName = x.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = x.LabOrderForm.PatientDoctorVisitFormId,
                PatientInformation = _mapper.Map<PatientDto>(x.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = x.LabOrderFormId,
                // TODO implement image files later 
                ImageFiles = GetCurrentImageFilesForLabTest(),
                Id = x.Id,
                Result = x.Result,
                Status = GetLabTestStatus(x.Status),
            });
            return result;
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

        public async Task<LabTestDto> Edit(long id, EditLabTestDto request)
        {
            var @spec = new GetDetailedLabTestByIdSpec(id);
            var labTest = await _labTestRepository.GetBySpecAsync(@spec);

            if (labTest == null)
            {
                throw new ArgumentException($"Cannot find a lab test with id: {id}");
            }

            // update lab test
            labTest.Result = request.Result;
            labTest.Status = request.Status;

            await _labTestRepository.UpdateAsync(labTest);
            return new LabTestDto
            {
                Description = labTest.Description,
                LabOrderFormCode = labTest.LabOrderForm.Code,
                CreatedAt = labTest.CreatedAt.Format(),
                DoctorName = labTest.LabOrderForm.Doctor.FullName,
                DoctorVisitingFormId = labTest.LabOrderForm.PatientDoctorVisitFormId,
                PatientInformation = _mapper.Map<PatientDto>(labTest.LabOrderForm.PatientHospitalizedProfile.Patient),
                LabOrderFormId = labTest.LabOrderFormId,
                // TODO implement image files later 
                ImageFiles = GetCurrentImageFilesForLabTest(),
                Id = labTest.Id,
                Result = labTest.Result,
                Status = GetLabTestStatus(labTest.Status),
            };
        }
    }
}