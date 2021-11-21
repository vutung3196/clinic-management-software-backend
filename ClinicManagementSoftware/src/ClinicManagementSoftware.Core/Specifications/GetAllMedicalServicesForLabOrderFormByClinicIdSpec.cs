using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicalServicesForLabOrderFormByClinicIdSpec : Specification<MedicalService>
    {
        public GetAllMedicalServicesForLabOrderFormByClinicIdSpec(long clinicId)
        {
            Query.Where(medicalService => medicalService.ClinicId == clinicId)
                .Where(medicalService => !medicalService.IsVisitingDoctorService)
                .Include(medicalService => medicalService.MedicalServiceGroup)
                .Where(medicalService => medicalService.IsDeleted == false);
        }
    }
}