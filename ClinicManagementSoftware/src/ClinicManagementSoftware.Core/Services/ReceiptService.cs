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
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public ReceiptService(IRepository<Receipt> receiptRepository, IMapper mapper, IUserContext userContext)
        {
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _userContext = userContext;
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
                    Address = receipt.Patient.Clinic.Address,
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
                CreatedAt = DateTime.UtcNow,
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
            return receipts.Select(x => _mapper.Map<ReceiptResponse>(x));
        }
    }
}