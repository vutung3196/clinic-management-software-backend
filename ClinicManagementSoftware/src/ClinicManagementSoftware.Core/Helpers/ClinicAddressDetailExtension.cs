namespace ClinicManagementSoftware.Core.Helpers
{
    public static class ClinicAddressDetailExtension
    {
        public static string GetDetailedAddress(string addressDetail, string addressStreet, string addressDistrict,
            string addressCity)
        {
            var clinicAddressDetail = "";
            clinicAddressDetail = addressDetail;
            if (!string.IsNullOrEmpty(addressStreet))
            {
                clinicAddressDetail += ", " + addressStreet;
            }

            if (!string.IsNullOrEmpty(addressDistrict))
            {
                clinicAddressDetail += ", " + addressDistrict;
            }

            if (!string.IsNullOrEmpty(addressCity))
            {
                clinicAddressDetail += ", " + addressCity;
            }

            return clinicAddressDetail;
        }
    }
}