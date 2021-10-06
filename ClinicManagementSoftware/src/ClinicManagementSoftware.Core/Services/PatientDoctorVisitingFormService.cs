using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.MedicalService;
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

        public PatientDoctorVisitingFormService(
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository,
            IUserContext userContext,
            IDoctorQueueService doctorQueueService, IReceiptService receiptService,
            IRepository<MedicalService> medicalServiceRepository)
        {
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _userContext = userContext;
            _doctorQueueService = doctorQueueService;
            _receiptService = receiptService;
            _medicalServiceRepository = medicalServiceRepository;
        }

        public async Task<PatientDoctorVisitingFormDto> GetAll(string byRole)
        {
            // validate role
            if (!ConfigurationConstant.PatientVisitingDoctorFormRoles.Contains(byRole))
            {
                throw new ArgumentException("Invalid role");
            }

            var currentContext = await _userContext.GetCurrentContext();
            var visitingForms = new List<PatientDoctorVisitForm>();
            // get by each role
            if (byRole.Equals("Accountant"))
            {
                // get thanh toán, chưa thanh toán và đang chờ khám
                var accountantSpec = new GetPatientDoctorVisitingFormsByAccountantSpec(currentContext.ClinicId);
                visitingForms = await _patientDoctorVisitingFormRepository.ListAsync(accountantSpec);
            }
            else
            {
            }

            // return result;
            //return visitingForms.Select(x => _)
            return null;
        }

        public async Task CreateVisitingForm(CreateOrUpdatePatientDoctorVisitingFormDto request)
        {
            var currentContext = await _userContext.GetCurrentContext();
            var visitingForm = new PatientDoctorVisitForm
            {
                Code = request.VisitingFormCode,
                CreatedAt = DateTime.UtcNow,
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
            await _receiptService.CreateReceipt(createReceiptRequest);
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