using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalServiceService
    {
        Task<IEnumerable<MedicalServiceDto>> GetAllMedicalServices();
        Task<MedicalServiceDto> CreateMedicalService(MedicalServiceDto request);
        Task<MedicalServiceDto> EditMedicalService(long id, MedicalServiceDto request);
        Task DeleteMedicalService(long id);
    }
}