using System.Linq;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabTestHavingDeletingMedicalServiceGroupSpec : Specification<LabTest>,
        ISingleResultSpecification
    {
        public GetLabTestHavingDeletingMedicalServiceGroupSpec(long medicalServiceGroupId)
        {
            Query.Where(x => x.MedicalService.MedicalServiceGroupId == medicalServiceGroupId)
                .Where(x => x.Status == (byte) EnumLabTestStatus.NotPaid)
                .Where(x => x.IsDeleted == false);
        }
    }
}