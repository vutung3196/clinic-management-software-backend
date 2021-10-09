using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.Prescription;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Exceptions.Patient;
using ClinicManagementSoftware.Core.Exceptions.Prescription;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<PatientHospitalizedProfile> _patientHospitalizedProfileRepository;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public PrescriptionService(IRepository<Patient> patientPrescriptionRepository,
            IRepository<PatientHospitalizedProfile> patientHospitalizedProfileRepository,
            IMapper mapper,
            IRepository<Prescription> prescriptionSpecificationRepository, IUserContext userContext)
        {
            _patientRepository = patientPrescriptionRepository;
            _prescriptionRepository = prescriptionSpecificationRepository;
            _userContext = userContext;
            _patientHospitalizedProfileRepository = patientHospitalizedProfileRepository;
            _mapper = mapper;
        }

        public async Task CreatePrescription(CreatePrescriptionDto request)
        {
            var currentPatient = await _patientRepository.GetByIdAsync(request.PatientId);
            if (currentPatient == null)
                throw new PatientNotFoundException($"Patient not found with patientId {request.PatientId}");

            string medicationInformation = null;
            if (request.MedicationInformation != null && request.MedicationInformation.Any())
                medicationInformation = JsonConvert.SerializeObject(request.MedicationInformation);
            //var prescriptionCode = ConfigurationConstant.BasePrescriptionName + DateTime.Now.ToString("yyyyMdHHmmss");

            var currentUser = await _userContext.GetCurrentContext();
            var currentPrescription = new Prescription
            {
                PatientHospitalizedProfileId = request.PatientHospitalizedProfileId,
                VisitReason = request.VisitReason,
                PatientPrescriptionCode = request.PrescriptionCode,
                CreatedAt = DateTime.UtcNow,
                DiagnosedDescription = request.DiagnosedDescription,
                DoctorSuggestion = request.DoctorSuggestion,
                MedicationInformation = medicationInformation,
                MedicalInsuranceCode = request.MedicalInsuranceCode,
                PatientDoctorVisitFormId = request.PatientDoctorVisitingFormId,
                DoctorId = currentUser.UserId,
                RevisitDate = request.RevisitDate
            };

            await _prescriptionRepository.AddAsync(currentPrescription);
        }

        public async Task<PrescriptionInformation> EditPrescription(long prescriptionId,
            CreatePrescriptionDto prescriptionRequest)
        {
            var currentPrescription = await _prescriptionRepository.GetByIdAsync(prescriptionId);
            var currentUser = await _userContext.GetCurrentContext();
            if (currentPrescription == null)
                throw new PrescriptionNotFoundException(
                    $"Prescriptions not found with Id: {prescriptionRequest.PatientId}");
            string medicineInformation = null;
            if (prescriptionRequest.MedicationInformation != null && prescriptionRequest.MedicationInformation.Any())
                medicineInformation = JsonConvert.SerializeObject(prescriptionRequest.MedicationInformation);

            currentPrescription.VisitReason = prescriptionRequest.VisitReason;
            currentPrescription.UpdatedAt = DateTime.UtcNow;
            currentPrescription.DiagnosedDescription = prescriptionRequest.DiagnosedDescription;
            currentPrescription.DoctorSuggestion = prescriptionRequest.DoctorSuggestion;
            currentPrescription.MedicationInformation = medicineInformation;
            currentPrescription.RevisitDate = prescriptionRequest.RevisitDate;
            currentPrescription.DoctorId = currentUser.UserId;

            await _prescriptionRepository.UpdateAsync(currentPrescription);
            return _mapper.Map<PrescriptionInformation>(currentPrescription);
        }

        public async Task<ICollection<PrescriptionInformation>> GetPrescriptionsByPatientId(long patientId)
        {
            var currentPatient = await _patientRepository.GetByIdAsync(patientId);
            if (currentPatient == null)
                throw new PatientNotFoundException($"Patient not found with patientId {patientId}");

            var currentPatientPrescriptions = await GetPrescriptionsFromPatientId(patientId);
            return currentPatientPrescriptions.Select(x => _mapper.Map<PrescriptionInformation>(x)).ToList();
        }

        public async Task<IEnumerable<PatientPrescriptionResponse>> GetPrescriptionsByClinicId()
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
                Patient = _mapper.Map<PatientDto>(x.PatientHospitalizedProfile.Patient),
                Prescription = _mapper.Map<PrescriptionInformation>(x)
            }).ToList();
            return result;
        }

        public async Task<PrescriptionInformation> GetPatientPrescriptionById(long prescriptionId)
        {
            var patientPrescription = await _prescriptionRepository.GetByIdAsync(prescriptionId);
            if (patientPrescription == null)
                throw new PrescriptionNotFoundException(
                    $"Cannot find patient prescription with patientId: {prescriptionId}");

            return _mapper.Map<PrescriptionInformation>(patientPrescription);
        }

        public async Task DeleteAsync(long id)
        {
            var patientPrescription = await _prescriptionRepository.GetByIdAsync(id);
            if (patientPrescription == null)
                throw new PrescriptionNotFoundException(
                    $"Cannot find patient prescription with id: {id}");
            await _prescriptionRepository.DeleteAsync(patientPrescription);
        }

        private async Task<List<Prescription>> GetPrescriptionsFromPatientId(long patientId)
        {
            var currentPatientHospitalizedProfiles =
                await _patientHospitalizedProfileRepository.ListAsync(
                    new PatientHospitalizedProfileSpec(patientId));
            return currentPatientHospitalizedProfiles.SelectMany(x => x.Prescriptions)
                .OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}