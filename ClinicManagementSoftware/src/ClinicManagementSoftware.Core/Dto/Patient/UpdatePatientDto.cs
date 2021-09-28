namespace ClinicManagementSoftware.Core.Dto.Patient
{
    public class UpdatePatientDto : CreatePatientDto
    {
        public UpdatePatientDto()
        {
        }

        public long Id { get; set; }
    }
}
