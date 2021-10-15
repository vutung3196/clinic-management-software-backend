using Ardalis.Specification;
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
                .ThenInclude(x => x.PatientDoctorVisitForm)
                .Include(profile => profile.LabOrderForms)
                .ThenInclude(x => x.PatientDoctorVisitForm)
                .Include(x => x.LabOrderForms)
                .ThenInclude(labOrderForm => labOrderForm.LabTests)
                .ThenInclude(x => x.MedicalService)
                .Include(labTest => labTest.LabOrderForms)
                .ThenInclude(x => x.LabTests)
                .ThenInclude(x => x.MedicalImageFiles)
                .ThenInclude(file => file.CloudinaryFile)
                .Where(profile => profile.Id == id);
        }
    }
}