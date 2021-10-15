using System;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class PatientHospitalizedProfileResponseDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public DateTime? RevisitDate { get; set; }
        public string RevisitDateDisplayed { get; set; }


        public string CreatedAt { get; set; }

        public PatientDto PatientInformation { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }

        public string PatientDetailedInformation { get; set; }
    }
}