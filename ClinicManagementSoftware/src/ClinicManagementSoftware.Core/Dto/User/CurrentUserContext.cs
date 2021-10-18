using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class CurrentUserContext
    {
        public CurrentUserContext(long userId, long clinicId, string userName, string fullName, Role role, long? medicalServiceGroupForTestSpecialistId)
        {
            UserId = userId;
            ClinicId = clinicId;
            UserName = userName;
            FullName = fullName;
            Role = role;
            MedicalServiceGroupForTestSpecialistId = medicalServiceGroupForTestSpecialistId;
        }

        public CurrentUserContext(long? medicalServiceGroupForTestSpecialistId)
        {
            MedicalServiceGroupForTestSpecialistId = medicalServiceGroupForTestSpecialistId;
        }

        public long UserId { get; set; }
        public long ClinicId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }
        public Role Role { get; set; }
    }
}