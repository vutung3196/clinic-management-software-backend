using System;

namespace ClinicManagementSoftware.Core.Exceptions.User
{
    public class UserInActiveException : Exception
    {
        public UserInActiveException(string message) : base(message)
        {
        }

        public UserInActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}