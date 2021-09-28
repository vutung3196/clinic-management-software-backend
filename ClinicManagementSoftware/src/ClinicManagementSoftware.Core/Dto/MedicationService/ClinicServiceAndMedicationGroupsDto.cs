using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.MedicationService
{
    public class ClinicServiceAndMedicationGroupsDto
    {
        public string GroupName { get; set; }
        public ICollection<MedicalServiceDto> ClinicServicePrices { get; set; }
        public ICollection<MedicationPriceDto> MedicationPrices { get; set; }
        public bool IsMedication { get; set; }
    }
}