using System;

namespace ClinicManagementSoftware.Core.Exceptions.Clinic
{
    public class ClinicInActiveException : Exception
    {
        public ClinicInActiveException(string message) : base(message)
        {
        }

        public ClinicInActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}