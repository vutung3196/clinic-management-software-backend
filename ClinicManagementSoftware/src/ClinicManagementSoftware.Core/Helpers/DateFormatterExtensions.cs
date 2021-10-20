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

        public static DateTime ResetTimeToStartOfDay(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                0, 0, 0, 0);
        }

        public static DateTime ResetTimeToEndOfDay(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                23, 59, 59, 999);
        }
    }
}