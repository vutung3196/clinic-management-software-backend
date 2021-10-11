using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class LabOrderFormDto
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DoctorVisitingFormCode { get; set; }
        public string Status { get; set; }
        public long DoctorVisitingFormId { get; set; }

        public string PatientDetailedInformation => PatientInformation.FullName + "-" + PatientInformation.Gender +
                                                    "-" + PatientInformation.Age + " tuổi";

        public IEnumerable<LabTestInformation> LabTests { get; set; }
        public PatientDto PatientInformation { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }
        public string DoctorName { get; set; }
    }
}