using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class CreateLabTestInformation
    {
        [Required]
        public long MedicalServiceId { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
    }
}