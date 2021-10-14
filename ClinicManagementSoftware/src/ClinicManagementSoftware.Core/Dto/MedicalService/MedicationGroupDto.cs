using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Prescription;

namespace ClinicManagementSoftware.Core.Dto.MedicalService
{
    public class MedicationGroupDto
    {
        public string GroupName { get; set; }
        public IEnumerable<MedicationInformation> Medications { get; set; }
    }
}