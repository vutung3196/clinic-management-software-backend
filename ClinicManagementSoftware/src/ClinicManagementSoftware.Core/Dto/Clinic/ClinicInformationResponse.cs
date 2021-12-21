using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Helpers;

namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class ClinicInformationResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }

        public string AddressStreet { get; set; }
        public string AddressDistrict { get; set; }
        public string AddressCity { get; set; }

        public string AddressDetailInformation =>
            ClinicAddressDetailExtension.GetDetailedAddress(AddressDetail, AddressStreet, AddressDistrict, AddressCity);

        public string AddressDetail { get; set; }
        public ImageFileResponse ImageFile { get; set; }
    }
}