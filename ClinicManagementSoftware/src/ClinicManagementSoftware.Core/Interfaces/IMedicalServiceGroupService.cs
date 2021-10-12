using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalServiceGroupService
    {
        Task<IEnumerable<MedicalServiceGroupResponseDto>> GetAllMedicalServiceGroups();
        Task<MedicalServiceGroupResponseDto> CreateMedicalServiceGroup(MedicalServiceGroupResponseDto request);
        Task<MedicalServiceGroupResponseDto> EditMedicalServiceGroup(long id, MedicalServiceGroupResponseDto request);
        Task DeleteMedicalServiceGroup(long id);
    }
}