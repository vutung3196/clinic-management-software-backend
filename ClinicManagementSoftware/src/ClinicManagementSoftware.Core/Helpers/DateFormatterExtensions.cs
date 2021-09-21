using System;

namespace ClinicManagementSoftware.Core.Helpers
{
    public static class DateFormatterExtensions
    {
        public static string Format(this DateTime? dateTime)
        {
            return dateTime?.ToString("MM/dd/yyyy");
        }

        public static string Format(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy");
        }
    }
}
