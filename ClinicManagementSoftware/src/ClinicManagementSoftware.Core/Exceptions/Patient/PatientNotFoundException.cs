using System;

namespace ClinicManagementSoftware.Core.Exceptions.Patient
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
        }

        public PatientNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
