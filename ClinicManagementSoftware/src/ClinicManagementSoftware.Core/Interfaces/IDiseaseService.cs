using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IDiseaseService
    {
        Task<IEnumerable<DiseaseResponseDto>> GetAll();
    }

    public class DiseaseResponseDto
    {
        public string DiseaseGroupName { get; set; }
        public IEnumerable<string> DiseaseNames { get; set; }
    }
}