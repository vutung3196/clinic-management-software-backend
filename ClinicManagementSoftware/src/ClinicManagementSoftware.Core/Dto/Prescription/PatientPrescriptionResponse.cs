using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.Prescription
{
    public class PatientPrescriptionResponse
    {
        public PatientDto Patient { get; set; }
        public string PatientPrescriptionCode => Prescription.PatientPrescriptionCode;
        public long Id => Prescription.Id;
        public string PatientName => Patient.FullName;
        public string PhoneNumber => Patient.PhoneNumber;
        public string DiagnosedDescription => Prescription.DiagnosedDescription;
        public string RevisitDate => Prescription.RevisitDate;
        public string DoctorSuggestion => Prescription.DoctorSuggestion;
        public string CreatedAt => Prescription.CreatedAt;
        public PrescriptionInformation Prescription { get; set; }
    }
}