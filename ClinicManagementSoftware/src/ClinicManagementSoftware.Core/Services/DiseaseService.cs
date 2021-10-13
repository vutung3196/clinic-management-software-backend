using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class DiseaseService : IDiseaseService
    {
        private readonly IRepository<Disease> _diseaseRepository;

        public DiseaseService(IRepository<Disease> diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }

        public async Task<IEnumerable<DiseaseResponseDto>> GetAll()
        {
            var diseases = await _diseaseRepository.ListAsync();
            var result = 
                diseases.GroupBy(x => x.DiseaseGroup)
                    .Select(x => new DiseaseResponseDto
                    {
                        DiseaseGroupName = x.Key,
                        DiseaseNames = x.Select(disease => disease.Description)
                    });
            return result;
        }
    }
}