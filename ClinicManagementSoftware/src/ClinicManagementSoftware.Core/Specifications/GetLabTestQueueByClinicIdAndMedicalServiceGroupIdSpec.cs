using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabTestQueueByClinicIdAndMedicalServiceGroupIdSpec : Specification<LabTestQueue>,
        ISingleResultSpecification
    {
        public GetLabTestQueueByClinicIdAndMedicalServiceGroupIdSpec(long clinicId, long medicalServiceGroupId)
        {
            Query.Where(labTestQueue => labTestQueue.ClinicId == clinicId &&
                                        labTestQueue.MedicalServiceGroupForTestSpecialistId.HasValue)
                .Where(labTestQueue =>
                    labTestQueue.MedicalServiceGroupForTestSpecialistId.Value == medicalServiceGroupId);
        }
    }
}