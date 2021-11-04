using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabTestsPerformedOfClinicFromDateSpec : Specification<LabTest>
    {
        public GetLabTestsPerformedOfClinicFromDateSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(labTest => labTest.LabOrderForm.Doctor.ClinicId == clinicId && labTest.IsDeleted == false)
                .Where(x => x.Status == (byte) EnumLabTestStatus.Done)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);
        }
    }
}