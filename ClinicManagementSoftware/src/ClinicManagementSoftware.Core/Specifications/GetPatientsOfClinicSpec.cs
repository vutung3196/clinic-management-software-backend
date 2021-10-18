using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientsOfClinicSpec : Specification<Patient>
    {
        public GetPatientsOfClinicSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(patient => patient.ClinicId == clinicId && patient.IsDeleted == 0)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);
        }
    }
}