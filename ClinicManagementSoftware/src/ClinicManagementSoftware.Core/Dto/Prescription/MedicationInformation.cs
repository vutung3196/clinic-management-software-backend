﻿using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Prescription
{
    public class MedicationInformation
    {
        public long Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng viên thuốc cần lớn hơn {1}")]
        public int Number { get; set; }

        public string Usage { get; set; }
        public string Name { get; set; }
    }
}