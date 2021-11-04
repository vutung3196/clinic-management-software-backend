using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientsOfClinicFromDateSpec : Specification<Patient>
    {
        public GetPatientsOfClinicFromDateSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(patient => patient.ClinicId == clinicId && patient.IsDeleted == 0)
                .Where(x => x.ActiveDate >= startDate && x.ActiveDate <= endDate);
        }
    }
}