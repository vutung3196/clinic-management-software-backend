using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.LabOrderForm;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class LabOrderFormService : ILabOrderFormService
    {
        private readonly IRepository<LabOrderForm> _labOrderFormRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly IRepository<PatientDoctorVisitForm> _patientDoctorVisitingFormRepository;
        private readonly ILabTestQueueService _labTestQueueService;
        private readonly IReceiptService _receiptService;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public LabOrderFormService(IRepository<LabOrderForm> labOrderFormRepository,
            IRepository<LabTest> labTestRepository, IUserContext userContext,
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository, IMapper mapper,
            IReceiptService receiptService, ILabTestQueueService labTestQueueService)
        {
            _labOrderFormRepository = labOrderFormRepository;
            _labTestRepository = labTestRepository;
            _userContext = userContext;
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _mapper = mapper;
            _receiptService = receiptService;
            _labTestQueueService = labTestQueueService;
        }

        // edit
        // new api for payment
        // maybe not needed
        public async Task EditLabOrderForm(long id, CreateOrEditLabOrderFormDto request)
        {
            var @spec = new GetLabOrderFormAndLabTestsAndPatientInformationSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            labOrderForm.CreatedAt = DateTime.Now;
            labOrderForm.Description = request.Description;
            labOrderForm = await _labOrderFormRepository.AddAsync(labOrderForm);
            foreach (var test in labOrderForm.LabTests)
            {
                await _labTestRepository.DeleteAsync(test);
            }

            foreach (var test in request.LabTests)
            {
                var labTest = new LabTest
                {
                    CreatedAt = DateTime.Now,
                    Description = test.Description,
                    LabOrderFormId = labOrderForm.Id,
                    MedicalServiceId = test.MedicalServiceId,
                    Status = (byte) EnumLabTestStatus.NotPaid,
                };
                await _labTestRepository.AddAsync(labTest);
            }
        }

        public async Task<long> PayLabOrderForm(long id, CreatePaymentForLabOrderFormDto request)
        {
            var @spec = new GetLabOrderFormAndLabTestsAndPatientInformationSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            var receiptRequest = new CreateReceiptDto
            {
                Code = request.PaymentCode,
                Description = request.PaymentDescription,
                MedicalServices = request.LabTests.Select(x => new ReceiptMedicalServiceDto
                {
                    BasePrice = x.BasePrice,
                    Name = x.Name,
                    Quantity = x.Quantity
                }),
                Total = request.Total,
                PatientId = labOrderForm.PatientHospitalizedProfile.PatientId,
                LabOrderFormId = id
            };

            var receiptId = await _receiptService.CreateReceipt(receiptRequest);
            labOrderForm.Status = (byte) EnumLabOrderFormStatus.Paid;
            foreach (var labTest in labOrderForm.LabTests)
            {
                labTest.Status = (byte) EnumLabTestStatus.WaitingForTesting;
                await _labTestRepository.UpdateAsync(labTest);
            }

            var medicalServiceGroupIdToLabTestIds = new Dictionary<long, List<long>>();
            foreach (var labTest in labOrderForm.LabTests)
            {
                var medicalServiceGroupId = labTest.MedicalService.MedicalServiceGroupId;
                if (!medicalServiceGroupIdToLabTestIds.ContainsKey(medicalServiceGroupId))
                {
                    medicalServiceGroupIdToLabTestIds[medicalServiceGroupId] = new List<long>
                        {labTest.Id};
                }
                else
                {
                    medicalServiceGroupIdToLabTestIds[medicalServiceGroupId]
                        .Add(labTest.Id);
                }
            }

            // update queue for each lab medical service group
            var currentContext = await _userContext.GetCurrentContext();
            foreach (var medicalServiceGroupId in medicalServiceGroupIdToLabTestIds.Keys)
            {
                var labTestIds = medicalServiceGroupIdToLabTestIds[medicalServiceGroupId].ToArray();
                await _labTestQueueService.EnqueueNewLabTestForMedicalServiceGroup(labTestIds,
                    medicalServiceGroupId, currentContext.ClinicId);
            }

            return receiptId;
        }

        public async Task PayLabOrderForm(long id)
        {
            var @spec = new GetLabOrderFormAndLabTestsAndPatientInformationSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            labOrderForm.Status = (byte) EnumLabOrderFormStatus.Paid;
            foreach (var test in labOrderForm.LabTests)
            {
                await _labTestRepository.DeleteAsync(test);
            }

            foreach (var labTest in labOrderForm.LabTests)
            {
                labTest.Status = (byte) EnumLabTestStatus.WaitingForTesting;
                await _labTestRepository.UpdateAsync(labTest);
            }
        }

        public async Task<IEnumerable<LabOrderFormDto>> GetAllByRole()
        {
            // validate role
            var currentContext = await _userContext.GetCurrentContext();
            if (currentContext.Role == null)
            {
                throw new ArgumentException("Current context should not be null");
            }

            var result = await GetLabOrderFormsByRole(currentContext);
            return result;
        }

        private async Task<IEnumerable<LabOrderFormDto>> GetLabOrderFormsByRole(
            CurrentUserContext currentContext)
        {
            var labOrderForms = new List<LabOrderForm>();
            if (currentContext.Role.RoleName.Equals(ConfigurationConstant.ReceptionistRole))
            {
                var @spec =
                    new GetLabOrderFormsForReceptionistSpec(currentContext.ClinicId);
                labOrderForms =
                    (await _labOrderFormRepository.ListAsync(@spec)).OrderByDescending(
                        x => x.CreatedAt).ToList();
            }
            else if (currentContext.Role.RoleName.Equals(ConfigurationConstant.DoctorRole))
            {
                var @spec =
                    new GetLabOrderFormsForDoctorSpec(currentContext.ClinicId);
                labOrderForms =
                    (await _labOrderFormRepository.ListAsync(@spec)).OrderByDescending(
                        x => x.CreatedAt).ToList();
            }
            else if (currentContext.Role.RoleName.Equals(ConfigurationConstant.TestSpecialistRole))
            {
                var @spec =
                    new GetLabOrderFormsForTestSpecialistSpec(currentContext.ClinicId);
                labOrderForms =
                    (await _labOrderFormRepository.ListAsync(@spec)).OrderByDescending(
                        x => x.CreatedAt).ToList();
            }


            return labOrderForms.Select(labOrderForm => new LabOrderFormDto
            {
                Id = labOrderForm.Id,
                PatientInformation = _mapper.Map<PatientDto>(labOrderForm.PatientHospitalizedProfile.Patient),
                Code = labOrderForm.Code,
                DoctorName = labOrderForm.Doctor.FullName,
                CreatedAt = labOrderForm.CreatedAt.Format(),
                Description = labOrderForm.Description,
                Status = GetLabOrderFormStatus(labOrderForm.Status),
                LabTests = labOrderForm.LabTests.Select(x => new LabTestInformation
                {
                    Id = x.Id,
                    Result = x.Result,
                    Name = x.MedicalService.Name,
                    Price = x.MedicalService.Price,
                    Description = x.Description,
                }),
            });
        }

        public async Task<LabOrderFormDto> GetLabOrderFormById(long id)
        {
            var @spec = new GetLabOrderFormAndLabTestsAndPatientInformationSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            return new LabOrderFormDto
            {
                PatientInformation = _mapper.Map<PatientDto>(labOrderForm.PatientHospitalizedProfile.Patient),
                Code = labOrderForm.Code,
                CreatedAt = labOrderForm.CreatedAt.Format(),
                Description = labOrderForm.Description,
                DoctorName = labOrderForm.Doctor.FullName,
                Status = GetLabOrderFormStatus(labOrderForm.Status),
                DoctorVisitingFormCode = labOrderForm.PatientDoctorVisitForm.Code,
                LabTests = labOrderForm.LabTests.Select(x => new LabTestInformation
                {
                    Id = x.Id,
                    Result = x.Result,
                    Name = x.MedicalService.Name,
                    Description = x.Description,
                }),
                ClinicInformation = new ClinicInformationResponse
                {
                    AddressCity = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressCity,
                    AddressDistrict = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressDistrict,
                    AddressStreet = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressStreet,
                    AddressDetail = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.AddressDetail,
                    Name = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.Name,
                    PhoneNumber = labOrderForm.PatientHospitalizedProfile.Patient.Clinic.PhoneNumber
                }
            };
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

        public Task<ClinicInformationResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateLabOrderForm(CreateOrEditLabOrderFormDto request)
        {
            var currentUser = await _userContext.GetCurrentContext();
            var labOrderForm = new LabOrderForm
            {
                CreatedAt = DateTime.Now,
                DoctorId = currentUser.UserId,
                Code = request.Code,
                Status = (byte) EnumLabOrderFormStatus.NotPaid,
                PatientDoctorVisitFormId = request.PatientDoctorVisitingFormId,
                PatientHospitalizedProfileId = request.PatientHospitalizedProfileId,
                Description = request.Description
            };
            labOrderForm = await _labOrderFormRepository.AddAsync(labOrderForm);
            foreach (var test in request.LabTests)
            {
                var labTest = new LabTest
                {
                    CreatedAt = DateTime.Now,
                    Description = test.Description,
                    LabOrderFormId = labOrderForm.Id,
                    MedicalServiceId = test.MedicalServiceId,
                    Status = (byte) EnumLabTestStatus.NotPaid,
                };
                await _labTestRepository.AddAsync(labTest);
            }

            // doctor visiting form status
            var doctorVisitingForm =
                await _patientDoctorVisitingFormRepository.GetByIdAsync(request.PatientDoctorVisitingFormId);
            if (doctorVisitingForm == null)
            {
                throw new ArgumentException(
                    $"Cannot find doctor visiting form with id: {request.PatientDoctorVisitingFormId}");
            }

            doctorVisitingForm.VisitingStatus = (byte) EnumDoctorVisitingFormStatus.HavingTesting;
            await _patientDoctorVisitingFormRepository.UpdateAsync(doctorVisitingForm);
            return labOrderForm.Id;
        }

        public async Task DeleteLabOrderForm(long id)
        {
            var labOrderForm = await _labOrderFormRepository.GetByIdAsync(id);
            if (labOrderForm == null)
                throw new ArgumentException(
                    $"Cannot find lab order form with id: {id}");
            await _labOrderFormRepository.DeleteAsync(labOrderForm);
        }
    }

    public class A
    {
        public long MedicalServiceGroupId { get; set; }
        public long[] LabTestId { get; set; }
    }
}