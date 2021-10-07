using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientDoctorVisitingMedicalServiceSpec : Specification<MedicalService>, ISingleResultSpecification
    {
        public GetPatientDoctorVisitingMedicalServiceSpec(long clinicId)
        {
            Query.Where(clinicServiceGroup => clinicServiceGroup.ClinicId == clinicId)
                .Where(x => x.IsVisitingDoctorService);
        }
    }
}