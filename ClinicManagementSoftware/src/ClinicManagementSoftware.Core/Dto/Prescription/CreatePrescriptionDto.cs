using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Prescription
{
    public class CreatePrescriptionDto
    {
        [Required(ErrorMessage = "Cần điền thông tin chẩn đoán")]
        public string DiagnosedDescription { get; set; }
        public DateTime RevisitDate { get; set; }
        public string DoctorSuggestion { get; set; }
        public IList<MedicationInformation> MedicationInformation { get; set; }
        public long PatientHospitalizedProfileId { get; set; }
        public string MedicalInsuranceCode { get; set; }
        public string Code { get; set; }
        public string DiseaseNote { get; set; }
        public long PatientDoctorVisitingFormId { get; set; }
    }
}
