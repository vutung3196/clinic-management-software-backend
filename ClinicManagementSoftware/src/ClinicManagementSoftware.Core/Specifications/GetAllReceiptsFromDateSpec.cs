using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllReceiptsFromDateSpec : Specification<Receipt>
    {
        public GetAllReceiptsFromDateSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(x => x.Patient.ClinicId == clinicId)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Include(x => x.LabOrderForm)
                .ThenInclude(x => x.LabTests)
                .Include(x => x.PatientDoctorVisitForm)
                .Include(x => x.Patient);
        }
    }
}