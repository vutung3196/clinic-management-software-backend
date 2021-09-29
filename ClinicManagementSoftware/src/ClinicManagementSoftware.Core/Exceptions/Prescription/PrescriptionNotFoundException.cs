using System;

namespace ClinicManagementSoftware.Core.Exceptions.Prescription
{
    public class PrescriptionNotFoundException : Exception
    {
        public PrescriptionNotFoundException(string message) : base(message)
        {
        }

        public PrescriptionNotFoundException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}