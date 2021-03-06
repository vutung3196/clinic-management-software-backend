using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.Prescription;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.Prescription;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;
using SendGrid;

namespace ClinicManagementSoftware.Core.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<PatientHospitalizedProfile> _patientHospitalizedProfileRepository;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<PatientDoctorVisitForm> _patientDoctorVisitingFormRepository;
        private readonly IRepository<MailTemplate> _mailTemplateRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly ISendGridService _sendGridService;
        private readonly IDoctorQueueService _doctorQueueService;

        public PrescriptionService(
            IRepository<PatientHospitalizedProfile> patientHospitalizedProfileRepository,
            IMapper mapper,
            IRepository<Prescription> prescriptionSpecificationRepository, IUserContext userContext,
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository,
            IDoctorQueueService doctorQueueService, ISendGridService sendGridService,
            IRepository<MailTemplate> mailTemplateRepository)
        {
            _prescriptionRepository = prescriptionSpecificationRepository;
            _userContext = userContext;
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _doctorQueueService = doctorQueueService;
            _sendGridService = sendGridService;
            _mailTemplateRepository = mailTemplateRepository;
            _patientHospitalizedProfileRepository = patientHospitalizedProfileRepository;
            _mapper = mapper;
        }

        public async Task<long> CreatePrescription(CreatePrescriptionDto request)
        {
            string medicationInformation = null;
            if (request.MedicationInformation != null && request.MedicationInformation.Any())
            {
                if (request.MedicationInformation.Any(x => x.Usage.Length > 100))
                {
                    throw new ArgumentException("Cách dùng của thuốc không vượt quá 100 ký tự");
                }

                medicationInformation = JsonConvert.SerializeObject(request.MedicationInformation);
            }

            if (request.RevisitDate < DateTime.Now)
            {
                throw new ArgumentException("Ngày tái khám cần là một ngày trong tương laix");
            }

            var currentUser = await _userContext.GetCurrentContext();
            var currentPrescription = new Prescription
            {
                PatientHospitalizedProfileId = request.PatientHospitalizedProfileId,
                MedicalInsuranceCode = request.MedicalInsuranceCode,
                CreatedAt = DateTime.Now,
                DiagnosedDescription = request.DiagnosedDescription,
                DoctorSuggestion = request.DoctorSuggestion,
                MedicationInformation = medicationInformation,
                Code = request.Code,
                PatientDoctorVisitFormId = request.PatientDoctorVisitingFormId,
                DoctorId = currentUser.UserId,
                DiseaseNote = request.DiseaseNote,
                RevisitDate = request.RevisitDate,
                SupervisorName = request.SupervisorName,
                Weight = request.Weight
            };

            // update status of doctor visiting form
            currentPrescription = await _prescriptionRepository.AddAsync(currentPrescription);
            var visitingForm =
                await _patientDoctorVisitingFormRepository.GetByIdAsync(request.PatientDoctorVisitingFormId);
            if (visitingForm == null)
            {
                throw new ArgumentException(
                    $"Cannot find doctor visiting form with id : {request.PatientDoctorVisitingFormId}");
            }

            visitingForm.UpdatedAt = DateTime.Now;
            visitingForm.VisitingStatus = (byte) EnumDoctorVisitingFormStatus.Done;
            await _patientDoctorVisitingFormRepository.UpdateAsync(visitingForm);

            // update queue
            await _doctorQueueService.DeleteAVisitingFormInDoctorQueue(visitingForm.Id, currentUser.UserId);

            // sending email here
            var prescription = await GetPrescriptionById(currentPrescription.Id);

            if (!string.IsNullOrEmpty(prescription.PatientInformation.EmailAddress))
            {
                await SendPrescriptionEmail(prescription, prescription.PatientInformation);
            }

            // update revisit date of hospitalized profile
            var patientHospitalizedProfile = await _patientHospitalizedProfileRepository
                .GetByIdAsync(request.PatientHospitalizedProfileId);
            if (patientHospitalizedProfile == null)
            {
                throw new ArgumentException(
                    $"Cannot find patient hospitalized profile with id: {request.PatientHospitalizedProfileId}");
            }

            patientHospitalizedProfile.RevisitDate = request.RevisitDate;
            await _patientHospitalizedProfileRepository.UpdateAsync(patientHospitalizedProfile);

            return currentPrescription.Id;
        }

        public async Task SendEmail(long prescriptionId)
        {
            var prescription = await GetPrescriptionById(prescriptionId);

            if (!string.IsNullOrEmpty(prescription.PatientInformation.EmailAddress))
            {
                await SendPrescriptionEmail(prescription, prescription.PatientInformation);
            }
        }

        private async Task SendPrescriptionEmail(PrescriptionInformation prescription, PatientDto patientInformation)
        {
            var getPrescriptionMailTemplateSpec =
                new GetMailTemplateByNameSpec(ConfigurationConstant.PrescriptionMailTemplate);
            var prescriptionEmailTemplate =
                await _mailTemplateRepository.GetBySpecAsync(getPrescriptionMailTemplateSpec);
            if (prescriptionEmailTemplate == null)
            {
                throw new ArgumentException("Cannot find prescription EmailAddress Template");
            }

            var prescriptionEmailContent =
                ProcessPrescriptionEmailContent(prescriptionEmailTemplate.Template, prescription,
                    patientInformation);

            await _sendGridService.Send(prescriptionEmailContent, "Đơn thuốc của bạn", MimeType.Html,
                patientInformation.EmailAddress, prescription.ClinicInformation.Name);
        }

        private static string ProcessPrescriptionEmailContent(string mailTemplate,
            PrescriptionInformation prescriptionInformation,
            PatientDto patientInformation)
        {
            if (string.IsNullOrWhiteSpace(mailTemplate))
            {
                return string.Empty;
            }

            // clinic
            mailTemplate = mailTemplate.Replace("{prescription.clinicInformation.name}",
                prescriptionInformation.ClinicInformation.Name);
            mailTemplate = mailTemplate.Replace("{prescription.clinicInformation.address}",
                prescriptionInformation.ClinicInformation.AddressDetailInformation);
            mailTemplate = mailTemplate.Replace("{prescription.clinicInformation.phoneNumber}",
                prescriptionInformation.ClinicInformation.PhoneNumber);

            // prescription
            mailTemplate = mailTemplate.Replace("{prescription.code}",
                prescriptionInformation.Code);
            mailTemplate =
                mailTemplate.Replace("{prescription.patientInformation.fullName}", patientInformation.FullName);
            mailTemplate = mailTemplate.Replace("{prescription.patientInformation.phoneNumber}",
                patientInformation.PhoneNumber);
            mailTemplate = mailTemplate.Replace("{prescription.patientInformation.addressCity}",
                patientInformation.AddressCity);
            mailTemplate = mailTemplate.Replace("{prescription.patientInformation.gender}", patientInformation.Gender);

            mailTemplate = mailTemplate.Replace("{prescription.patientInformation.medicalInsuranceCode}",
                patientInformation.MedicalInsuranceCode);
            mailTemplate = mailTemplate.Replace("{prescription.doctorName}",
                prescriptionInformation.DoctorName);

            mailTemplate = mailTemplate.Replace("{prescription.diagnosedDescription}",
                prescriptionInformation.DiagnosedDescription);
            mailTemplate = mailTemplate.Replace("{prescription.diseaseNote}",
                prescriptionInformation.DiseaseNote);

            mailTemplate = mailTemplate.Replace("{prescription.doctorSuggestion}",
                prescriptionInformation.DoctorSuggestion);
            if (prescriptionInformation.PatientInformation.Age <= 6)
            {
                mailTemplate =
                    mailTemplate.Replace("{prescription.weight}", prescriptionInformation.Weight + "kg");
                mailTemplate = mailTemplate.Replace("{prescription.patientInformation.age}",
                    patientInformation.Month + " tháng");
            }
            else
            {
                mailTemplate =
                    mailTemplate.Replace("{prescription.weight}", string.Empty);
                mailTemplate = mailTemplate.Replace("{prescription.patientInformation.age}",
                    patientInformation.Age.ToString());
            }

            mailTemplate =
                mailTemplate.Replace("{prescription.supervisorName}", prescriptionInformation.SupervisorName);
            mailTemplate =
                mailTemplate.Replace("{prescription.revisitDate}", prescriptionInformation.RevisitDateDisplayed);
            var arrayTime = prescriptionInformation.CreatedAt.Split("/");
            if (arrayTime.Length == 3)
            {
                var day = arrayTime[1];
                var month = arrayTime[0];
                var year = arrayTime[2];

                mailTemplate = mailTemplate.Replace("{date.day}", day);
                mailTemplate = mailTemplate.Replace("{date.month}", month);
                mailTemplate = mailTemplate.Replace("{date.year}", year);
            }

            var medicationsHtml = GetMedications(prescriptionInformation.MedicationInformation.ToList());
            mailTemplate = mailTemplate.Replace("{medications}", medicationsHtml);

            return mailTemplate;
        }

        private static string GetMedications(IReadOnlyList<MedicationInformation> medicationInformation)
        {
            var result = "";
            for (var i = 0; i < medicationInformation.Count; i++)
            {
                var medicationHtml =
                    " <tr> <td align=\"center\" valign=\"top\"> {index}. </td> <td align=\"left\" valign=\"top\"> <div style={style6}> <strong>{name}</strong> <div>{usage}</div> </div> <div style={style11}> Số lượng: <strong>{number}</strong> viên </div> <div class=\"clear\"></div> </td> </tr>";
                medicationHtml = medicationHtml.Replace("{index}", (i + 1).ToString());
                medicationHtml = medicationHtml.Replace("{name}", medicationInformation[i].Name);
                medicationHtml = medicationHtml.Replace("{number}", medicationInformation[i].Quantity.ToString());
                medicationHtml = medicationHtml.Replace("{usage}", medicationInformation[i].Usage);
                result += medicationHtml;
            }

            return result;
        }

        public async Task<IEnumerable<PatientPrescriptionResponse>> GetAllPrescriptions()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var spec = new GetPrescriptionsByClinicIdSpec(currentUser.ClinicId);
            var profiles = await _patientHospitalizedProfileRepository.ListAsync(spec);
            var prescriptions = profiles
                .SelectMany(x => x.Prescriptions)
                .OrderByDescending(x => x.UpdatedAt)
                .ThenByDescending(x => x.CreatedAt);
            var result = prescriptions.Select(x => new PatientPrescriptionResponse
            {
                PatientInformation = _mapper.Map<PatientDto>(x.PatientHospitalizedProfile.Patient),
                Prescription = _mapper.Map<PrescriptionInformation>(x),
                DoctorName = x.Doctor.FullName,
            }).ToList();

            return result;
        }

        public async Task<PrescriptionInformation> GetPrescriptionById(long prescriptionId)
        {
            var @spec = new GetDetailedPrescriptionByIdSpec(prescriptionId);
            var prescription = await _prescriptionRepository.GetBySpecAsync(@spec);
            if (prescription == null)
                throw new PrescriptionNotFoundException(
                    $"Cannot find patient prescription with patientId: {prescriptionId}");

            var result = _mapper.Map<PrescriptionInformation>(prescription);
            result.PatientInformation = _mapper.Map<PatientDto>(prescription.PatientHospitalizedProfile.Patient);
            result.ClinicInformation = new ClinicInformationResponse()
            {
                AddressCity = prescription.PatientHospitalizedProfile.Patient.Clinic.AddressCity,
                AddressDistrict = prescription.PatientHospitalizedProfile.Patient.Clinic.AddressDistrict,
                AddressStreet = prescription.PatientHospitalizedProfile.Patient.Clinic.AddressStreet,
                AddressDetail = prescription.PatientHospitalizedProfile.Patient.Clinic.AddressDetail,
                Name = prescription.PatientHospitalizedProfile.Patient.Clinic.Name,
                PhoneNumber = prescription.PatientHospitalizedProfile.Patient.Clinic.PhoneNumber
            };
            result.DoctorName = prescription.Doctor.FullName;
            return result;
        }
    }
}