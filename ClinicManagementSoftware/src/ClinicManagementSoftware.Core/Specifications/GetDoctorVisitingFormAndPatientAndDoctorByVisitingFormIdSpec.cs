using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetReceiptAndPatientAndClinicByReceiptIdSpec : Specification<Receipt>,
        ISingleResultSpecification
    {
        public GetReceiptAndPatientAndClinicByReceiptIdSpec(long id)
        {
            Query.Include(receipt => receipt.Patient)
                .ThenInclude(patient => patient.Clinic)
                .Where(receipt => receipt.Id == id);
        }
    }
}