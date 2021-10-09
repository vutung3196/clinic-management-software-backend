using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.Prescription
{
    public class PrescriptionInformation
    {
        public long Id { get; set; }
        public string DoctorVisitingFormCode { get; set; }
        public long DoctorVisitingFormId { get; set; }
        public string VisitReason { get; set; }
        public string DiagnosedDescription { get; set; }
        public string RevisitDate { get; set; }
        public string DoctorSuggestion { get; set; }
        public string PatientPrescriptionCode { get; set; }
        public string CreatedAt { get; set; }
        public string DoctorName { get; set; }
        public long DoctorId { get; set; }
        public IEnumerable<MedicationInformation> MedicationInformation { get; set; }
    }
}
