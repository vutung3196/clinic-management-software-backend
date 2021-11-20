using System;
using System.ComponentModel.DataAnnotations;
using ClinicManagementSoftware.Core.Helpers;

namespace ClinicManagementSoftware.Core.Dto.MedicalService
{
    public class MedicalServiceDto
    {
        public MedicalServiceDto(long id, string name, string description, double price, string groupName, long groupId,
            DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            GroupName = groupName;
            GroupId = groupId;
            CreatedDate = createdAt;
        }

        public long Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên của xét nghiệm không vượt quá 100 ký tự")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Tên của xét nghiệm không vượt quá 500 ký tự")]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập giá tiền lớn hơn {1}")]
        public double Price { get; set; }

        public double? TotalPrice => Price;
        public string AmountDisplayed => $"{Price:n0}" + " Đồng";
        public string GroupName { get; set; }
        public long GroupId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedAt => CreatedDate.Format();
    }
}