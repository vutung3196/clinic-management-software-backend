using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Medication;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicationService
    {
        Task<IEnumerable<MedicationDto>> GetAllMedications();
    }
}