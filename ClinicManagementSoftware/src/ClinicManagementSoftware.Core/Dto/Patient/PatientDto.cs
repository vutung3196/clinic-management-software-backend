﻿using System;
using ClinicManagementSoftware.Core.Helpers;

namespace ClinicManagementSoftware.Core.Dto.Patient
{
    public class PatientDto
    {
        public long Id { get; set; }
        public long ClinicId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string AddressDetail { get; set; }
        public string AddressCity { get; set; }
        public string AddressStreet { get; set; }
        public string AddressDistrict { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthDetail => DateOfBirth != null ? DateOfBirth.Format() : string.Empty;
        public int? Age => DateOfBirth != null ? DateTime.Now.Year - DateOfBirth.Value.Year : null;

        public string MedicalInsuranceCode { get; set; }
        //public int? Age => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth : null;
    }
}