using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.MedicalService;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Services
{
    public class PatientDoctorVisitingFormService : IPatientDoctorVisitingFormService
    {
        private readonly IRepository<PatientDoctorVisitForm> _patientDoctorVisitingFormRepository;
        private readonly IRepository<MedicalService> _medicalServiceRepository;
        private readonly IDoctorQueueService _doctorQueueService;
        private readonly IReceiptService _receiptService;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public PatientDoctorVisitingFormService(
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository,
            IUserContext userContext,
            IDoctorQueueService doctorQueueService, IReceiptService receiptService,
            IRepository<MedicalService> medicalServiceRepository, IMapper mapper)
        {
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _userContext = userContext;
            _doctorQueueService = doctorQueueService;
            _receiptService = receiptService;
            _medicalServiceRepository = medicalServiceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDoctorVisitingFormDto>> GetAllByRole()
        {
            // validate role
            var currentContext = await _userContext.GetCurrentContext();
            if (currentContext.Role == null)
            {
                throw new ArgumentException("Current context should not be null");
            }

            var result = await GetDoctorVisitingFormByRole(currentContext);
            return result;
        }

        private async Task<IEnumerable<PatientDoctorVisitingFormDto>> GetDoctorVisitingFormByRole(
            CurrentUserContext currentContext)
        {
            List<PatientDoctorVisitForm> doctorVisitForms;
            switch (currentContext.Role.RoleName)
            {
                case "Receptionist":
                {
                    var @spec =
                        new GetPatientDoctorVisitingFormsForReceptionistFromClinicIdSpec(currentContext.ClinicId);
                    doctorVisitForms =
                        (await _patientDoctorVisitingFormRepository.ListAsync(@spec)).OrderByDescending(
                            x => x.UpdatedAt).ToList();
                    break;
                }
                case "Doctor":
                    // get from queue, order by id
                    doctorVisitForms = await ProcessDoctorVisitForms(currentContext.UserId);
                    break;
                default:
                    return new List<PatientDoctorVisitingFormDto>();
            }

            return doctorVisitForms.Select(ConvertToPatientDoctorVisitingFormDto);
        }

        private async Task<List<PatientDoctorVisitForm>> ProcessDoctorVisitForms(long doctorId)
        {
            var currentDoctorVisitingQueue = await _doctorQueueService.GetCurrentDoctorQueue(doctorId);
            var spec =
                new GetPatientDoctorVisitingFormsForDoctorFromClinicIdSpec(doctorId,
                    currentDoctorVisitingQueue.ToArray());
            var doctorVisitForms = await _patientDoctorVisitingFormRepository.ListAsync(spec);
            var visitFormIdToDoctorVisitForm =
                doctorVisitForms.ToDictionary(doctorVisitForm => doctorVisitForm.Id, x => x);

            return currentDoctorVisitingQueue.Select(visitingFormId => visitFormIdToDoctorVisitForm[visitingFormId])
                .ToList();
        }


        public async Task<PatientDoctorVisitingFormDto> GetById(long id)
        {
            var @spec = new GetDoctorVisitingFormAndPatientAndDoctorByVisitingFormIdSpec(id);
            var patientDoctorVisitForm = await _patientDoctorVisitingFormRepository.GetBySpecAsync(@spec);
            if (patientDoctorVisitForm == null)
            {
                throw new ArgumentException($"Visiting form is not found with id: {id}");
            }

            var result = ConvertToPatientDoctorVisitingFormDto(patientDoctorVisitForm);

            return result;
        }

        private PatientDoctorVisitingFormDto ConvertToPatientDoctorVisitingFormDto(
            PatientDoctorVisitForm patientDoctorVisitForm)
        {
            var result = new PatientDoctorVisitingFormDto
            {
                Description = patientDoctorVisitForm.Description,
                Code = patientDoctorVisitForm.Code,
                CreatedAt = patientDoctorVisitForm.CreatedAt.Format(),
                DoctorName = patientDoctorVisitForm.Doctor.FullName,
                PatientInformation = _mapper.Map<PatientDto>(patientDoctorVisitForm.Patient),
                VisitingStatus = patientDoctorVisitForm.VisitingStatus,
                DoctorId = patientDoctorVisitForm.Doctor.Id,
                Id = patientDoctorVisitForm.Id,
                ClinicInformation = new ClinicInformationResponse
                {
                    Address = patientDoctorVisitForm.Patient.Clinic.Address,
                    Name = patientDoctorVisitForm.Patient.Clinic.Name,
                    PhoneNumber = patientDoctorVisitForm.Patient.Clinic.PhoneNumber
                },
                UpdatedAt = patientDoctorVisitForm.UpdatedAt.HasValue
                    ? patientDoctorVisitForm.UpdatedAt.Value.ToString("MM/dd/yyyy hh:mm tt")
                    : "",
                VisitingStatusDisplayed = GetDoctorVisitingFormStatus(patientDoctorVisitForm.VisitingStatus)
            };
            return result;
        }

        private static string GetDoctorVisitingFormStatus(byte status)
        {
            var result = status switch
            {
                (byte) EnumDoctorVisitingFormStatus.WaitingForDoctor => "Đang chờ khám",
                (byte) EnumDoctorVisitingFormStatus.Done => "Đã khám xong",
                (byte) EnumDoctorVisitingFormStatus.HavingTesting => "Đang làm xét nghiệm",
                _ => ""
            };

            return result;
        }

        public async Task<CreateVisitingFormResponse> CreateVisitingForm(
            CreateOrUpdatePatientDoctorVisitingFormDto request)
        {
            var currentContext = await _userContext.GetCurrentContext();
            var visitingForm = new PatientDoctorVisitForm
            {
                Code = request.VisitingFormCode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Description = request.Description,
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                VisitingStatus = (byte) EnumDoctorVisitingFormStatus.WaitingForDoctor
            };

            visitingForm = await _patientDoctorVisitingFormRepository.AddAsync(visitingForm);
            await _doctorQueueService.EnqueueNewPatient(visitingForm.Id, request.DoctorId);

            var createReceiptRequest =
                await GetDoctorVisitingFormMedicalServiceReceiptRequest(request, visitingForm.Id,
                    currentContext.ClinicId);
            var receiptId = await _receiptService.CreateReceipt(createReceiptRequest);
            var result = new CreateVisitingFormResponse {ReceiptId = receiptId, DoctorVisitingFormId = visitingForm.Id};
            return result;
        }

        public async Task<IEnumerable<DoctorAvailabilityDto>> GetCurrentDoctorAvailabilities()
        {
            var currentContext = await _userContext.GetCurrentContext();
            var doctorQueues = await _doctorQueueService.GetAllDoctorQueues(currentContext.ClinicId);
            return doctorQueues.Select(x => new DoctorAvailabilityDto()
            {
                DoctorId = x.DoctorId,
                DoctorName = x.Doctor.FullName,
                PatientNumber = JsonConvert.DeserializeObject<VisitingDoctorQueueData>(x.Queue).Data.Count,
            }).OrderBy(x => x.PatientNumber);
        }

        public async Task MoveATopPatientToTheEndOfADoctorQueue()
        {
            var currentContext = await _userContext.GetCurrentContext();
            await _doctorQueueService.MoveAFirstPatientToTheEndOfTheQueue(currentContext.UserId);
        }

        private async Task<CreateReceiptDto> GetDoctorVisitingFormMedicalServiceReceiptRequest(
            CreateOrUpdatePatientDoctorVisitingFormDto request,
            long patientDoctorVisitingFormId, long clinicId)
        {
            var medicalServices = new List<ReceiptMedicalServiceDto>();
            var @spec = new GetPatientDoctorVisitingMedicalServiceSpec(clinicId);
            var medicalService = await _medicalServiceRepository.GetBySpecAsync(@spec);
            if (medicalService == null)
            {
                throw new VisitingDoctorMedicalServiceNotFoundException(
                    "Visiting doctor medical service is not created");
            }

            medicalServices.Add(new ReceiptMedicalServiceDto
            {
                Name = medicalService.Name,
                BasePrice = medicalService.Price,
                Quantity = 1
            });

            var createReceiptRequest = new CreateReceiptDto
            {
                Code = request.PaymentCode,
                Description = request.PaymentDescription,
                PatientDoctorVisitingFormId = patientDoctorVisitingFormId,
                Total = medicalServices.Sum(x => x.Total),
                PatientId = request.PatientId,
                MedicalServices = medicalServices
            };
            return createReceiptRequest;
        }
    }
}