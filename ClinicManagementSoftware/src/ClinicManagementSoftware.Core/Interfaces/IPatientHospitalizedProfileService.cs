using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientHospitalizedProfileService
    {
        Task<PatientHospitalizedProfileResponseDto> GetPatientProfilesByPatientId(long patientId);
        Task<PatientHospitalizedProfileResponseDto> CreatePatientProfile(CreatePatientHospitalizedProfileDto request);
        Task<PatientHospitalizedProfileResponseDto> EditPatientProfile(long id, CreatePatientHospitalizedProfileDto request);
        Task DeletePatientProfile(long id);
        Task<IEnumerable<PatientHospitalizedProfileResponseDto>> GetPatientHospitalizedProfilesForPatient(long patientId);
        Task<DetailedPatientHospitalizedProfileResponseDto> GetDetailedPatientHospitalizedProfile(long id);

        
    }
}
