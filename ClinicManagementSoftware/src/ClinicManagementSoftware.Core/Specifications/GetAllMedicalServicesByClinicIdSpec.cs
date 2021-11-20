using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicalServicesByClinicIdSpec : Specification<MedicalService>
    {
        public GetAllMedicalServicesByClinicIdSpec(long clinicId)
        {
            Query.Where(medicalService => medicalService.ClinicId == clinicId)
                .Where(medicalService => !medicalService.IsVisitingDoctorService)
                .Include(medicalService => medicalService.MedicalServiceGroup)
                .Where(medicalService => medicalService.IsDeleted == false);
        }
    }
}