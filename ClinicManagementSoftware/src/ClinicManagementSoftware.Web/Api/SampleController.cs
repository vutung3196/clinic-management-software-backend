using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSoftware.Web.Api
{
    /// <summary>
    /// If your API controllers will use a consistent route convention and the [ApiController] attribute (they should)
    /// then it's a good idea to define and use a common base controller class like this one.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : Controller
    {
        private readonly IRepository<Clinic> _clinicSpecificationRepository;

        public SampleController(IRepository<Clinic> clinicSpecificationRepository)
        {
            _clinicSpecificationRepository = clinicSpecificationRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllClinics()
        {
            var a = await _clinicSpecificationRepository.ListAsync();
            return Ok(a);
        }
    }
}