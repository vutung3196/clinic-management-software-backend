using System;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPrescriptionsOfClinicFromDateSpec : Specification<Prescription>
    {
        public GetPrescriptionsOfClinicFromDateSpec(long clinicId, DateTime startDate, DateTime endDate)
        {
            Query.Where(prescription =>
                    prescription.PatientHospitalizedProfile.Patient.ClinicId == clinicId)
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);
        }
    }
}