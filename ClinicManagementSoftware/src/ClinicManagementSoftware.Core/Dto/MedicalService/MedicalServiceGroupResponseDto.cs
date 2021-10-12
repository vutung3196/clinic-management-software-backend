namespace ClinicManagementSoftware.Core.Dto.MedicalService
{
    public class MedicalServiceGroupResponseDto
    {
        public MedicalServiceGroupResponseDto(long id, string name, string description, string createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
    }
}