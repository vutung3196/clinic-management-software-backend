using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetReceiptReportSpec : Specification<Receipt>
    {
        public GetReceiptReportSpec(DateTime startDate, DateTime endDate, long clinicId)
        {
            Query.Where(x => x.Patient.ClinicId == clinicId)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Include(x => x.Patient);
        }
    }
}