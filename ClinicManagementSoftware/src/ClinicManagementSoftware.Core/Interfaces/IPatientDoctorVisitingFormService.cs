using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientDoctorVisitingFormService
    {
        Task<PatientDoctorVisitingFormDto> GetAll(string byRole);
        Task CreateVisitingForm(CreateOrUpdatePatientDoctorVisitingFormDto request);
        Task<IEnumerable<DoctorAvailabilityDto>> GetCurrentDoctorAvailabilities();
    }
}