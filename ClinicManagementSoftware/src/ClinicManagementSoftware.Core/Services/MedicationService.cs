using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Medication;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IRepository<Medication> _medicationSpecification;

        public MedicationService(IRepository<Medication> medicationSpecification)
        {
            _medicationSpecification = medicationSpecification;
        }

        public async Task<IEnumerable<MedicationDto>> GetAllMedications()
        {
            var medications =  await _medicationSpecification.ListAsync();
            return medications.Select(x => new MedicationDto(x.Id, x.Name, x.Description, x.Usage));
        }
    }
}