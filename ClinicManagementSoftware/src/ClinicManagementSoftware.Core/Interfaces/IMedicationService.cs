using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicationService
    {
        Task<IEnumerable<MedicationGroupDto>> GetAllMedications();
    }
}