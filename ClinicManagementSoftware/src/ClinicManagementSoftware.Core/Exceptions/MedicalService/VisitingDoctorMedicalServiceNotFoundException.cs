using System;

namespace ClinicManagementSoftware.Core.Exceptions.MedicalService
{
    public class VisitingDoctorMedicalServiceNotFoundException : Exception
    {
        public VisitingDoctorMedicalServiceNotFoundException(string message) : base(message)
        {
        }

        public VisitingDoctorMedicalServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
