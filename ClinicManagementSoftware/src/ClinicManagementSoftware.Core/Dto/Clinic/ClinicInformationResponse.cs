namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class ClinicInformationResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}