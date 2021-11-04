using System;
using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Helpers
{
    public class DateTimeReportComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return x.CompareTo(y);
        }
    }
}