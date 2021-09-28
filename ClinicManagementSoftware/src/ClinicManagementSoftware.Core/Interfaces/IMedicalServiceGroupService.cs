using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicationService;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalServiceGroupService
    {
        Task<IEnumerable<MedicalServiceGroupDto>> GetAllMedicalServiceGroups();
        Task<MedicalServiceGroupDto> CreateMedicalServiceGroup(MedicalServiceGroupDto request);
        Task<MedicalServiceGroupDto> EditMedicalServiceGroup(long id, MedicalServiceGroupDto request);
        Task DeleteMedicalServiceGroup(long id);
    }
}