using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class CreateOrEditLabOrderFormDto
    {
        public string Description { get; set; }
        public IList<CreateLabTestInformation> LabTests { get; set; }
        public long PatientHospitalizedProfileId { get; set; }
        public string Code { get; set; }
        public long PatientDoctorVisitingFormId { get; set; }
    }
}
