using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

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
            var receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new ArgumentException($"Receipt is not found with id: {id}");
            }

            return _mapper.Map<ReceiptResponse>(receipt);
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