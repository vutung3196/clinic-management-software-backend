using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.FinancialReport;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class ClinicReportService : IClinicReportService
    {
        private readonly IRepository<Receipt> _receiptSpecificationRepository;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly IRepository<PatientDoctorVisitForm> _patientDoctorVisitingFormRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IUserContext _userContext;

        public ClinicReportService(IRepository<Receipt> receiptSpecificationRepository,
            IRepository<Prescription> prescriptionRepository,
            IUserContext userContext, IRepository<LabTest> labTestRepository,
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository,
            IRepository<Patient> patientRepository)
        {
            _receiptSpecificationRepository = receiptSpecificationRepository;
            _prescriptionRepository = prescriptionRepository;
            _userContext = userContext;
            _labTestRepository = labTestRepository;
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _patientRepository = patientRepository;
        }

        public async Task<FinancialReportResponse> Get(DateTime startDate, DateTime endDate)
        {
            var currentUser = await _userContext.GetCurrentContext();
            var result = new FinancialReportResponse();
            var @receiptSpec = new GetAllReceiptsFromDateSpec(currentUser.ClinicId, startDate, endDate);
            var receipts = await _receiptSpecificationRepository.ListAsync(@receiptSpec);
            if (receipts.Count == 0)
            {
                return result;
            }

            var totalReceiptAmount = receipts.Sum(x => x.Total);
            if (receipts.Any())
            {
                var topPatients = GetTop10PatientPaymentInformation(receipts);
                result.PatientPaymentInformation = topPatients;
            }

            result.TotalReceiptAmount = totalReceiptAmount;

            // calculate revenue by day information
            var receiptByDayInformations = CalculateReceiptsByDayInformation(receipts);
            result.ReceiptByDayInformations = receiptByDayInformations;
            var patientSpec = new GetPatientsOfClinicFromDateSpec(currentUser.ClinicId, startDate, endDate);
            var patients = await _patientRepository.ListAsync(patientSpec);
            result.TotalNumberNewPatients = patients.Count;

            var labTestSpec = new GetLabTestsPerformedOfClinicFromDateSpec(currentUser.ClinicId, startDate, endDate);
            var labTests = await _labTestRepository.ListAsync(labTestSpec);
            result.TotalNumberTestPerformed = labTests.Count;

            var doctorVisitingFormSpec =
                new GetPaidPatientDoctorVisitingFormOfClinicFromDateSpec(currentUser.ClinicId, startDate, endDate);
            var doctorVisitingForms = await _patientDoctorVisitingFormRepository.ListAsync(doctorVisitingFormSpec);
            result.TotalNumberDoctorVisitingForms = doctorVisitingForms.Count;

            var prescriptions =
                await _prescriptionRepository.ListAsync(
                    new GetPrescriptionsOfClinicFromDateSpec(currentUser.ClinicId, startDate, endDate));
            result.TotalNumberPrescriptions = prescriptions.Count;

            // diagram
            var numberOfPatientsByMonth = GetPatientCountByMonth(doctorVisitingForms);
            result.NumberOfPatientsByMonth = numberOfPatientsByMonth;
            return result;
        }

        public async Task<DashboardReportResponse> GetDashBoardReport(long clinicId, DateTime startDate,
            DateTime endDate)
        {
            var result = new DashboardReportResponse();

            var @receiptSpec = new GetAllReceiptsFromDateSpec(clinicId, startDate, endDate);
            var receipts = await _receiptSpecificationRepository.ListAsync(@receiptSpec);
            if (receipts.Count == 0)
            {
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<MedicalServiceReport>> GetDashBoardReport(DateTime startDate, DateTime endDate)
        {
            var result = new List<MedicalServiceReport>();

            return result;
        }

        private static IEnumerable<int> GetPatientCountByMonth(IReadOnlyCollection<PatientDoctorVisitForm> forms)
        {
            var result = new List<int>();
            for (var i = 1; i <= 12; i++)
            {
                var month = i;
                var patientCount = forms.Count(x => x.CreatedAt.Month == month);
                result.Add(patientCount);
            }

            return result;
        }

        private static IEnumerable<PatientPaymentInformation> GetTop10PatientPaymentInformation(
            IEnumerable<Receipt> receipts)
        {
            var topPatients = receipts
                .GroupBy(x => x.Patient)
                .ToDictionary(x => x.Key,
                    x => x.Sum(receipt => receipt.Total));

            var result = topPatients.OrderByDescending(x => x.Value).Take(10)
                .Select(x => new PatientPaymentInformation
                {
                    Name = x.Key.FullName,
                    DateOfBirth = x.Key.DateOfBirth.HasValue ? x.Key.DateOfBirth.Format() : "",
                    Amount = x.Value,
                    Gender = x.Key.Gender == (byte) EnumGender.Nam ? "Nam" : "Nữ",
                    Address = x.Key.AddressDetail + ", " + x.Key.AddressStreet + ", " + x.Key.AddressDistrict + ", " +
                              x.Key.AddressCity,
                    EmailAddress = x.Key.EmailAddress,
                    PhoneNumber = x.Key.PhoneNumber
                });
            return result;
        }

        private static IEnumerable<ReceiptByDayInformation> CalculateReceiptsByDayInformation(
            IEnumerable<Receipt> receipts)
        {
            var result = new List<ReceiptByDayInformation>();
            var dateSet = new SortedSet<DateTime>(new DateTimeReportComparer());

            var dateToReceipts = new Dictionary<string, List<Receipt>>();

            foreach (var receipt in receipts)
            {
                dateSet.Add(receipt.CreatedAt);
                if (!dateToReceipts.ContainsKey(receipt.CreatedAt.Date.Format()))
                {
                    var receiptList = new List<Receipt> {receipt};
                    dateToReceipts[receipt.CreatedAt.Date.Format()] = receiptList;
                }
                else
                {
                    dateToReceipts[receipt.CreatedAt.Date.Format()].Add(receipt);
                }
            }

            var a = dateToReceipts.Select(x => new ReceiptByDayInformation()
            {
                Date = x.Key,
                TotalReceiptAmount = x.Value.Sum(x => x.Total),
            });
            return a;
            //foreach (var date in dateSet)
            //{
            //    var resultElement = new ReceiptByDayInformation();
            //    if (dateToReceipts.ContainsKey(date.Format()))
            //    {
            //        var totalReceipts = dateToReceipts[date.Format()];
            //        var totalReceiptAmount = totalReceipts.Sum(x => x.Total);
            //        resultElement.TotalReceiptAmount = totalReceiptAmount;
            //    }

            //    resultElement.Date = date.Format();
            //    result.Add(resultElement);
            //}
        }
    }
}