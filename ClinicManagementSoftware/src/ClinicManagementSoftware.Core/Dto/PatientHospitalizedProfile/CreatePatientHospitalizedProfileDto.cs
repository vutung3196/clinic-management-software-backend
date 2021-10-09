﻿using System;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class CreatePatientHospitalizedProfileDto
    {
        public long PatientId { get; set; }
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public DateTime? RevisitDate { get; set; }
    }
}