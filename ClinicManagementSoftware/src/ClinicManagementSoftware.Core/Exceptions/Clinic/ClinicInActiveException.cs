using System;

namespace ClinicManagementSoftware.Core.Exceptions.Clinic
{
    public class ClinicNotFoundException : Exception
    {
        public ClinicNotFoundException(string message) : base(message)
        {
        }

        public ClinicNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}