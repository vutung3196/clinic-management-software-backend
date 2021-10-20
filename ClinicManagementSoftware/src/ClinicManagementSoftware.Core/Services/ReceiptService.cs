using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public ReceiptService(IRepository<Receipt> receiptRepository, IMapper mapper, IUserContext userContext,
            IRepository<Clinic> clinicRepository)
        {
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _userContext = userContext;
            _clinicRepository = clinicRepository;
        }

        public async Task<ReceiptResponse> GetReceiptById(long id)
        {
            var @spec = new GetReceiptAndPatientAndClinicByReceiptIdSpec(id);
            var receipt = await _receiptRepository.GetBySpecAsync(@spec);
            if (receipt == null)
            {
                throw new ArgumentException($"Receipt is not found with id: {id}");
            }

            var result = new ReceiptResponse
            {
                Id = receipt.Id,
                CreatedAt = receipt.CreatedAt.Format(),
                Description = receipt.Description,
                Total = receipt.Total,
                Code = receipt.Code,
                PatientInformation = _mapper.Map<PatientDto>(receipt.Patient),
                MedicalServices = !string.IsNullOrWhiteSpace(receipt.Services)
                    ? JsonConvert.DeserializeObject<ICollection<ReceiptMedicalServiceDto>>(receipt.Services)
                    : null,
                ClinicInformation = new ClinicInformationResponse
                {
                    AddressCity = receipt.Patient.Clinic.AddressCity,
                    AddressDistrict = receipt.Patient.Clinic.AddressDistrict,
                    AddressStreet = receipt.Patient.Clinic.AddressStreet,
                    AddressDetail = receipt.Patient.Clinic.AddressDetail,
                    Name = receipt.Patient.Clinic.Name,
                    PhoneNumber = receipt.Patient.Clinic.PhoneNumber
                }
            };

            result.TotalInText = result.Total.ConvertToText();
            return result;
        }

        public async Task<long> CreateReceipt(CreateReceiptDto createReceiptDto)
        {
            var receipt = new Receipt
            {
                CreatedAt = DateTime.Now,
                Description = createReceiptDto.Description,
                Code = createReceiptDto.Code,
                PatientId = createReceiptDto.PatientId,
                PatientDoctorVisitFormId = createReceiptDto.PatientDoctorVisitingFormId,
                LabOrderFormId = createReceiptDto.LabOrderFormId,
                Total = createReceiptDto.Total,
                Services = JsonConvert.SerializeObject(createReceiptDto.MedicalServices),
            };
            receipt = await _receiptRepository.AddAsync(receipt);
            return receipt.Id;
        }

        public async Task Delete(long id)
        {
            var receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new ArgumentException($"Receipt is not found with id: {id}");
            }

            await _receiptRepository.DeleteAsync(receipt);
        }

        public async Task<IEnumerable<ReceiptResponse>> GetAll()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllReceiptOfClinicSpec(currentUser.ClinicId);
            var receipts = await _receiptRepository.ListAsync(@spec);
            var result = receipts.Select(x => new ReceiptResponse()
            {
                Description = x.Description,
                Code = x.Code,
                CreatedAt = x.CreatedAt.Format(),
                Id = x.Id,
                MedicalServices = !string.IsNullOrEmpty(x.Services)
                    ? JsonConvert
                        .DeserializeObject<ICollection<ReceiptMedicalServiceDto>>(x.Services)
                    : null,
                PatientInformation = _mapper.Map<PatientDto>(x.Patient),
                Total = x.Total,
                TotalInText = x.Total.ConvertToText(),
            });
            return result;
        }

        public async Task<ReceiptReportResponse> GetReceiptReport(ReceiptReportRequestDto request)
        {
            var currentContext = await _userContext.GetCurrentContext();
            var @spec = new GetReceiptReportSpec(request.StartDate, request.EndDate, currentContext.ClinicId);
            var @clinicSpec = new GetClinicInformationByIdSpec(currentContext.ClinicId);
            var clinic = await _clinicRepository.GetBySpecAsync(@clinicSpec);
            var receipts = await _receiptRepository.ListAsync(@spec);
            if (receipts.Count == 0)
            {
                return new ReceiptReportResponse();
            }

            var dictionary = receipts.ToDictionary(x => x, x =>
                !string.IsNullOrWhiteSpace(x.Services)
                    ? JsonConvert.DeserializeObject<ICollection<ReceiptMedicalServiceDto>>(x.Services)
                    : null);

            var reportMedicalServiceDtos = new List<ReceiptReportMedicalServiceDto>();
            var sampleReceipt = receipts.FirstOrDefault();
            var total = receipts.Sum(x => x.Total);
            foreach (var receipt in dictionary.Keys)
            {
                var receiptMedicalServiceDtos = dictionary[receipt];
                var receiptReportMedicalServiceDtos = receiptMedicalServiceDtos.Select(x =>
                    new ReceiptReportMedicalServiceDto
                    {
                        Description = receipt.Description,
                        Id = receipt.Id,
                        CreatedAt = receipt.CreatedAt.Format(),
                        PatientInformation = _mapper.Map<PatientDto>(receipt.Patient),
                        BasePrice = x.BasePrice,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        ReceiptCode = receipt.Code,
                    });
                reportMedicalServiceDtos.AddRange(receiptReportMedicalServiceDtos);
            }

            var result = new ReceiptReportResponse
            {
                ContainingPatientAddress = request.ContainingPatientAddress,
                ContainingPatientAge = request.ContainingPatientAge,
                ContainingPatientEmail = request.ContainingPatientEmail,
                ContainingPatientName = request.ContainingPatientName,
                ContainingPatientPhoneNumber = request.ContainingPatientPhoneNumber,
                ClinicInformation = new ClinicInformationResponse
                {
                    AddressCity = clinic?.AddressCity,
                    AddressDistrict = clinic?.AddressDistrict,
                    AddressStreet = clinic?.AddressStreet,
                    AddressDetail = clinic?.AddressDetail,
                    Name = clinic?.Name,
                    PhoneNumber = clinic?.PhoneNumber
                },
                MedicalServices = reportMedicalServiceDtos,
                Total = total,
            };
            return result;
        }
    }
}