using System;

namespace ClinicManagementSoftware.Core.Exceptions.Prescription
{
    public class InvalidMedicalInsuranceCodeException : Exception
    {
        public InvalidMedicalInsuranceCodeException(string message) : base(message)
        {
        }

        public InvalidMedicalInsuranceCodeException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}