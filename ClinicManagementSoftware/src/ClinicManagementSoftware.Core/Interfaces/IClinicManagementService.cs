using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IClinicManagementService
    {
        Task UpdateClinicInformation(long id, CreateUpdateClinicRequestDto request);
        Task DeactivateClinic(long id);
        Task<ClinicInformationResponse> GetClinicInformation(long id);
        Task<ClinicInformationForAdminResponse> CreateClinic(CreateUpdateClinicRequestDto request);
        Task<IEnumerable<ClinicInformationForAdminResponse>> GetClinics();
    }
}