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
        Task<PatientDoctorVisitingFormDto> EditVisitingForm(long id, CreateOrUpdatePatientDoctorVisitingFormDto request);
        Task<IEnumerable<DoctorAvailabilityDto>> GetCurrentDoctorAvailabilities();
        Task<PatientDoctorVisitingFormDto> MoveAVisitingFormToTheEndOfADoctorQueue(long doctorVisitingFormId);
        //Task<PatientDoctorVisitingFormDto> MoveAVisitingFormToTheEndOfADoctorQueue(long doctorVisitingFormId);
        Task DeleteVisitingFormsByPatientId(long patientId);
        Task<PatientDoctorVisitingFormDto> MoveAVisitingFormToTheBeginningOfADoctorQueue(long doctorVisitingFormId);
    }
}