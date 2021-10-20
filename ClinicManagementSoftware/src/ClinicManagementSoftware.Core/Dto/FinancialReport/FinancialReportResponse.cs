using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class FinancialReportResponse
    {
        public double TotalReceiptAmount { get; set; }
        public string TotalReceiptAmountDisplayed => $"{TotalReceiptAmount:n0}";
        public int TotalNumberDoctorVisitingForms { get; set; }
        public int TotalNumberNewPatients { get; set; }
        public int TotalNumberPrescriptions { get; set; }
        public int TotalNumberTestPerformed { get; set; }
        public IEnumerable<PatientPaymentInformation> PatientPaymentInformation { get; set; }
        public IEnumerable<ReceiptByDayInformation> ReceiptByDayInformations { get; set; }
        public IEnumerable<int> NumberOfPatientsByMonth { get; set; }
    }
}