using System.Linq;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabTestHavingDeletingMedicalServiceSpec : Specification<LabTest>, ISingleResultSpecification
    {
        public GetLabTestHavingDeletingMedicalServiceSpec(long medicalServiceId)
        {
            Query.Where(x => x.MedicalService.Id == medicalServiceId)
                .Where(x => x.Status == (byte) EnumLabTestStatus.NotPaid)
                .Where(x => x.IsDeleted == false);
        }
    }
}