using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPatientDoctorVisitingFormService
    {
        Task<PatientDoctorVisitingFormDto> GetAll(string byRole);
    }
}