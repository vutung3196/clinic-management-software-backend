using System;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class PatientHospitalizedProfileResponseDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public DateTime? RevisitDate { get; set; }
    }
}
