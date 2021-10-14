using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Dto.Medication;
using ClinicManagementSoftware.Core.Dto.Prescription;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IRepository<Medication> _medicationRepository;

        public MedicationService(IRepository<Medication> medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<IEnumerable<MedicationGroupDto>> GetAllMedications()
        {
            var @spec = new GetAllMedicationsSpec();
            var medications = await _medicationRepository.ListAsync(@spec);
            var result = medications.GroupBy(x => x.MedicationGroup)
                .Select(x => new MedicationGroupDto
                {
                    GroupName = x.Key.Name,
                    Medications = x.Select(medication => new MedicationInformation()
                    {
                        Name = medication.Name,
                        Id = medication.Id,
                        Quantity = 1,
                        Usage = medication.Usage
                    }),
                });
            return result;
        }
    }
}