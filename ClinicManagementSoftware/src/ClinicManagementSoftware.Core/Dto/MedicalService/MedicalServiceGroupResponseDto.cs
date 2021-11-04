using System.ComponentModel.DataAnnotations;

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

        [MaxLength(100, ErrorMessage = "Tên của nhóm chỉ định xét nghiệm không vượt quá 100 ký tự")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả của nhóm chỉ định xét nghiệm không vượt quá 500 ký tự")]
        public string Description { get; set; }

        public string CreatedAt { get; set; }
    }
}