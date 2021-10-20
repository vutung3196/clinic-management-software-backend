using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicationsSpec : Specification<Medication>
    {
        public GetAllMedicationsSpec()
        {
            Query.Include(x => x.MedicationGroup);
        }
    }
}