using System;

namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class ClinicInformationForAdminResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CreatedAt { get; set; }
        public bool Enabled { get; set; }
        public string Status => Enabled ? "Kích hoạt" : "Khóa";
        public string AddressStreet { get; set; }
        public string AddressDistrict { get; set; }
        public string AddressCity { get; set; }
        public string AddressDetail { get; set; }
    }
}