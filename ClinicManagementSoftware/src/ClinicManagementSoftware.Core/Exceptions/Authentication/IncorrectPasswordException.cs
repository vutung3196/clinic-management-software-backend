using System;

namespace ClinicManagementSoftware.Core.Exceptions.Authentication
{
    public class InCorrectPasswordException : Exception
    {
        public InCorrectPasswordException(string message) : base(message)
        {
        }

        public InCorrectPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
