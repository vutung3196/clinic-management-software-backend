using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabOrderFormAndLabTestsAndPatientInformationSpec : Specification<LabOrderForm>,
        ISingleResultSpecification
    {
        public GetLabOrderFormAndLabTestsAndPatientInformationSpec(long id)
        {
            Query.Where(labOrderForm => labOrderForm.Id == id)
                .Include(x => x.Doctor)
                .Include(labOrderForm => labOrderForm.PatientDoctorVisitForm)
                .Include(labOrderForm => labOrderForm.PatientHospitalizedProfile.Patient)
                .ThenInclude(x => x.Clinic)
                .Include(labOrderForm => labOrderForm.LabTests)
                .ThenInclude(labTest => labTest.MedicalService);
        }
    }
}