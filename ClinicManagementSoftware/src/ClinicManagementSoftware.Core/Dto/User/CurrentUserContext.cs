using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class CurrentUserContext
    {
        public CurrentUserContext(long userId, long clinicId, string userName, string fullName, Role role)
        {
            UserId = userId;
            ClinicId = clinicId;
            UserName = userName;
            FullName = fullName;
            Role = role;
        }

        public CurrentUserContext()
        {
        }

        public long UserId { get; set; }
        public long ClinicId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
    }
}