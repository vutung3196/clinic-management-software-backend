using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabOrderFormAndLabTestsByIdSpec : Specification<LabOrderForm>, ISingleResultSpecification
    {
        public GetLabOrderFormAndLabTestsByIdSpec(long id)
        {
            Query.Include(x => x.LabTests)
                .Include(x => x.PatientDoctorVisitForm)
                .Where(x => x.Id == id)
                .Where(x => x.IsDeleted == false);
        }
    }
}