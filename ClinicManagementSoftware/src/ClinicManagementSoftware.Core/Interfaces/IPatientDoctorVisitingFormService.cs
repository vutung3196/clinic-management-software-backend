using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientDoctorVisitingFormService
    {
        Task<IEnumerable<PatientDoctorVisitingFormDto>> GetAllByRole();
        Task<PatientDoctorVisitingFormDto> GetById(long id);
        Task<CreateVisitingFormResponse> CreateVisitingForm(CreateOrUpdatePatientDoctorVisitingFormDto request);
        Task<IEnumerable<DoctorAvailabilityDto>> GetCurrentDoctorAvailabilities();
        Task MoveATopPatientToTheEndOfADoctorQueue();
    }
}