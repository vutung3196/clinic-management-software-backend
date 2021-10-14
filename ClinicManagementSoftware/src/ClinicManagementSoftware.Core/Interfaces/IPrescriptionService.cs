using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Prescription;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IPrescriptionService
    {
        Task<long> CreatePrescription(CreatePrescriptionDto request);
        Task<PrescriptionInformation> EditPrescription(long prescriptionId, CreatePrescriptionDto prescriptionRequest);
        Task<ICollection<PrescriptionInformation>> GetPrescriptionsByPatientId(long patientId);
        Task<IEnumerable<PatientPrescriptionResponse>> GetPrescriptionsByClinicId();
        Task<PrescriptionInformation> GetPrescriptionById(long prescriptionId);
        Task DeleteAsync(long id);
    }
}
