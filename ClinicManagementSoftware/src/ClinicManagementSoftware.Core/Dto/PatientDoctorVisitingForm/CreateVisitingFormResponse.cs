using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class CreateVisitingFormResponse
    {
        public long ReceiptId { get; set; }
        public long DoctorVisitingFormId { get; set; }
    }
}