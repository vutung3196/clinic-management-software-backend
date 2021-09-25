using System;

namespace ClinicManagementSoftware.Core.Exceptions.Patient
{
    public class InvalidGenderException : Exception
    {
        public InvalidGenderException(string message) : base(message)
        {
        }

        public InvalidGenderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
