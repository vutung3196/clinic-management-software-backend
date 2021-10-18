using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class UserResultResponse
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CreatedAt { get; set; }
        public byte Enabled { get; set; }
        public long ClinicId { get; set; }
        public string Role { get; set; }
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }

        public string RoleDescription => Role switch
        {
            "Admin" => "Admin",
            "Receptionist" => "Lễ tân",
            "Doctor" => "Bác sĩ",
            "TestSpecialist" => "Nhân viên xét nghiệm",
            _ => ""
        };

        public string Status => Enabled == (byte)EnumEnabled.Active ? "Kích hoạt" : "Khóa";
    }
}