using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPaidPatientDoctorVisitingFormOfClinicFromDateSpec : Specification<PatientDoctorVisitForm>
    {
        public GetPaidPatientDoctorVisitingFormOfClinicFromDateSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(labTest => labTest.Patient.ClinicId == clinicId && labTest.IsDeleted == false)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);
        }
    }
}