using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.MedicalService
{
    public class MedicalServiceGroupDto
    {
        public string GroupName { get; set; }
        public IEnumerable<MedicalServiceForLabTest> MedicalServices { get; set; }
    }
}