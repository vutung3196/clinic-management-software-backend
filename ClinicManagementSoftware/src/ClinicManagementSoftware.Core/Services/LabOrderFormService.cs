using System;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.LabOrderForm;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class LabOrderFormService : ILabOrderFormService
    {
        private readonly IRepository<LabOrderForm> _labOrderFormRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly IUserContext _userContext;

        public LabOrderFormService(IRepository<LabOrderForm> labOrderFormRepository,
            IRepository<LabTest> labTestRepository, IUserContext userContext)
        {
            _labOrderFormRepository = labOrderFormRepository;
            _labTestRepository = labTestRepository;
            _userContext = userContext;
        }

        // edit
        // new api for payment
        public async Task EditLabOrderForm(long id, CreateOrEditLabOrderFormDto request)
        {
            var @spec = new GetLabOrderFormAndLabTestsSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            labOrderForm.CreatedAt = DateTime.UtcNow;
            labOrderForm.Status = request.Status;
            labOrderForm.Description = request.Description;
            labOrderForm = await _labOrderFormRepository.AddAsync(labOrderForm);
            foreach (var test in labOrderForm.LabTests)
            {
                await _labTestRepository.DeleteAsync(test);
            }

            foreach (var test in request.LabTests)
            {
                var labTest = new LabTest
                {
                    CreatedAt = DateTime.UtcNow,
                    Description = test.Description,
                    LabOrderFormId = labOrderForm.Id,
                    MedicalServiceId = test.MedicalServiceId,
                    Status = (byte) EnumLabTestStatus.NotPaid,
                };
                await _labTestRepository.AddAsync(labTest);
            }
        }

        public async Task PayLabOrderForm(long id)
        {
            var @spec = new GetLabOrderFormAndLabTestsSpec(id);
            var labOrderForm = await _labOrderFormRepository.GetBySpecAsync(@spec);
            if (labOrderForm == null)
            {
                throw new ArgumentException($"Cannot find lab order form with id: {id}");
            }

            labOrderForm.Status = (byte) EnumLabOrderFormStatus.Paid;
            foreach (var test in labOrderForm.LabTests)
            {
                await _labTestRepository.DeleteAsync(test);
            }

            foreach (var labTest in labOrderForm.LabTests)
            {
                labTest.Status = (byte) EnumLabTestStatus.WaitingForTesting;
                await _labTestRepository.UpdateAsync(labTest);
            }
        }

        public Task<ClinicInformationResponse> GetLabOrderForm(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ClinicInformationResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task CreateLabOrderForm(CreateOrEditLabOrderFormDto request)
        {
            var currentUser = await _userContext.GetCurrentContext();
            var labOrderForm = new LabOrderForm
            {
                CreatedAt = DateTime.UtcNow,
                DoctorId = currentUser.UserId,
                Code = request.Code,
                Status = (byte) EnumLabOrderFormStatus.NotPaid,
                PatientDoctorVisitingFormId = request.PatientDoctorVisitingFormId,
                PatientHospitalizedProfileId = request.PatientHospitalizedProfileId,
                Description = request.Description
            };
            labOrderForm = await _labOrderFormRepository.AddAsync(labOrderForm);
            foreach (var test in request.LabTests)
            {
                var labTest = new LabTest
                {
                    CreatedAt = DateTime.UtcNow,
                    Description = test.Description,
                    LabOrderFormId = labOrderForm.Id,
                    MedicalServiceId = test.MedicalServiceId,
                    Status = (byte) EnumLabTestStatus.NotPaid,
                };
                await _labTestRepository.AddAsync(labTest);
            }
        }

        public async Task DeleteLabOrderForm(long id)
        {
            var labOrderForm = await _labOrderFormRepository.GetByIdAsync(id);
            if (labOrderForm == null)
                throw new ArgumentException(
                    $"Cannot find lab order form with id: {id}");
            await _labOrderFormRepository.DeleteAsync(labOrderForm);
        }
    }
}