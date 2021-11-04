using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class CreateOrEditLabOrderFormDto
    {
        [MaxLength(100, ErrorMessage = "Miêu tả không được vượt quá 100 ký tự")]
        public string Description { get; set; }
        public IList<CreateLabTestInformation> LabTests { get; set; }
        public long PatientHospitalizedProfileId { get; set; }
        public string Code { get; set; }
        public long PatientDoctorVisitingFormId { get; set; }
    }
}