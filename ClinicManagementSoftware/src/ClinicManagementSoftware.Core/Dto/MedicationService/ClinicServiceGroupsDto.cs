using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.MedicationService
{
    public class ClinicServiceGroupsDto
    {
        public string GroupName { get; set; }
        public ICollection<MedicalServiceDto> ClinicServices { get; set; }
    }
}