using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class CreatePatientHospitalizedProfileDto
    {
        public long PatientId { get; set; }

        [MaxLength(50, ErrorMessage = "Tên bệnh không vượt quá 50 ký tự")]
        public string DiseaseName { get; set; }

        [MaxLength(50, ErrorMessage = "Mô tả bệnh không vượt quá 50 ký tự")]
        public string Description { get; set; }

        public string Code { get; set; }
        public DateTime? RevisitDate { get; set; }
    }
}