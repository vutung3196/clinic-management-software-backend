using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientDoctorVisitingFormService
    {
        Task<IEnumerable<PatientDoctorVisitingFormDto>> GetAllByRole();
        Task<PatientDoctorVisitingFormDto> GetById(long id);
        Task DeleteById(long id);
        Task<CreateVisitingFormResponse> CreateVisitingForm(CreateOrUpdatePatientDoctorVisitingFormDto request);
        Task<PatientDoctorVisitingFormDto> EditVisitingForm(long id, CreateOrUpdatePatientDoctorVisitingFormDto request);
        Task<IEnumerable<DoctorAvailabilityDto>> GetCurrentDoctorAvailabilities();
        Task<PatientDoctorVisitingFormDto> MoveATopPatientToTheEndOfADoctorQueue();
        Task DeleteVisitingFormsByPatientId(long patientId);
    }
}