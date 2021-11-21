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

        [MaxLength(500, ErrorMessage = "Lời dặn của bác sĩ không được vượt quá 500 ký tự")]
        public string DoctorSuggestion { get; set; }
        public IList<MedicationInformation> MedicationInformation { get; set; }
        public long PatientHospitalizedProfileId { get; set; }
        public string MedicalInsuranceCode { get; set; }
        public string Code { get; set; }
        public double? Weight { get; set; }

        [MaxLength(50, ErrorMessage = "Tên người đưa trẻ đến khám không vượt quá 50 ký tự")]
        public string SupervisorName { get; set; }

        [MaxLength(100, ErrorMessage = "Miêu tả bệnh không được vượt quá 100 ký tự")]
        public string DiseaseNote { get; set; }

        public long PatientDoctorVisitingFormId { get; set; }
    }
}