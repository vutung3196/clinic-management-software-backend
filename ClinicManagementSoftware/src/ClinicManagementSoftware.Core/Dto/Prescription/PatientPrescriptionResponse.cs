using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.Prescription
{
    public class PatientPrescriptionResponse
    {
        public PatientDto PatientInformation { get; set; }
        public string Code => Prescription.Code;
        public long Id => Prescription.Id;
        public string PatientName => PatientInformation.FullName;
        public string PhoneNumber => PatientInformation.PhoneNumber;
        public string DiagnosedDescription => Prescription.DiagnosedDescription;
        public string Note => Prescription.DiseaseNote;
        public string DoctorName { get; set; }
        public string RevisitDateDisplayed => Prescription.RevisitDateDisplayed;
        public string DoctorSuggestion => Prescription.DoctorSuggestion;
        public string CreatedAt => Prescription.CreatedAt;

        public string PatientDetailedInformation => PatientInformation.FullName + "-" + PatientInformation.Gender +
                                                    "-" + PatientInformation.Age + " tuổi";

        public PrescriptionInformation Prescription { get; set; }
    }
}