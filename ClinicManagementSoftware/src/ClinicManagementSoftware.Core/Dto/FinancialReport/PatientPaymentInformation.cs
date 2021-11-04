using System;

namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class PatientPaymentInformation
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double Amount { get; set; }

        public string AmountDisplayed => $"{Amount:n0}";
    }
}