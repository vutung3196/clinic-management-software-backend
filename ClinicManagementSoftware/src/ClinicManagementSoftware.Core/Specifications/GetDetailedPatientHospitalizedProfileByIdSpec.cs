﻿using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedPatientHospitalizedProfileByIdSpec : Specification<PatientHospitalizedProfile>,
        ISingleResultSpecification
    {
        public GetDetailedPatientHospitalizedProfileByIdSpec(long id)
        {
            Query.Include(profile => profile.Patient)
                .Include(profile => profile.Prescriptions)
                .Include(profile => profile.LabOrderForms)
                .ThenInclude(labOrderForm => labOrderForm.LabTests)
                .ThenInclude(x => x.MedicalService)
                .Include(labTest => labTest.MedicalImageFiles)
                .ThenInclude(file => file.CloudinaryFile)
                .Where(profile => profile.Id == id);
        }
    }
}