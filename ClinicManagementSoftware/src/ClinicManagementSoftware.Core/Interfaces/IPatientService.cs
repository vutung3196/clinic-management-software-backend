using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto> GetByIdAsync(long? id);
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto> AddAsync(CreatePatientDto request);
        Task<PatientDto> UpdateAsync(UpdatePatientDto patient);
        Task DeleteAsync(long? id);
    }
}
