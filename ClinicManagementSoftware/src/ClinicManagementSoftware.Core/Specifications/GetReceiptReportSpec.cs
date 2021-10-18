using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDoctorVisitingFormAndPatientAndDoctorByVisitingFormIdSpec :
        Specification<PatientDoctorVisitForm>,
        ISingleResultSpecification
    {
        public GetDoctorVisitingFormAndPatientAndDoctorByVisitingFormIdSpec(long id)
        {
            Query.Include(patientDoctorVisitForm => patientDoctorVisitForm.Patient)
                .ThenInclude(patient => patient.Clinic)
                .Include(patientDoctorVisitForm => patientDoctorVisitForm.Doctor)
                .Where(patientDoctorVisitForm => patientDoctorVisitForm.Id == id);
        }
    }
}