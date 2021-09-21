using Ardalis.HttpClientTestExtensions;
using ClinicManagementSoftware.Web;
using ClinicManagementSoftware.Web.Endpoints.ProjectEndpoints;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ClinicManagementSoftware.FunctionalTests.ApiEndpoints
{
    [Collection("Sequential")]
    public class ProjectList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectList(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsOneProject()
        {
            var result = await _client.GetAndDeserialize<ProjectListResponse>("/Projects");

            Assert.Single(result.Projects);
            Assert.Contains(result.Projects, i => i.Name == SeedData.TestProject1.Name);
        }
    }
}
