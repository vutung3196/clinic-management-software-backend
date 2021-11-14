using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class CreateReceiptDto
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public long PatientId { get; set; }
        public long? PatientDoctorVisitingFormId { get; set; }
        public long? LabOrderFormId { get; set; }
        public double Total { get; set; }
        public IEnumerable<ReceiptMedicalServiceDto> MedicalServices { get; set; }
    }
}