using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class PatientDoctorVisitingFormService : IPatientDoctorVisitingFormService
    {
        private readonly IRepository<PatientDoctorVisitForm> _patientDoctorVisitingFormRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public PatientDoctorVisitingFormService(
            IRepository<PatientDoctorVisitForm> patientDoctorVisitingFormRepository,
            IUserContext userContext, IMapper mapper)
        {
            _patientDoctorVisitingFormRepository = patientDoctorVisitingFormRepository;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<PatientDoctorVisitingFormDto> GetAll(string byRole)
        {
            // validate role
            if (!ConfigurationConstant.PatientVisitingDoctorFormRoles.Contains(byRole))
            {
                throw new ArgumentException("Invalid role");
            }

            var currentContext = await _userContext.GetCurrentContext();
            var visitingForms = new List<PatientDoctorVisitForm>();
            // get by each role
            if (byRole.Equals("Accountant"))
            {
                // get thanh toán, chưa thanh toán và đang chờ khám
                var accountantSpec = new GetPatientDoctorVisitingFormsByAccountantSpec(currentContext.ClinicId);
                visitingForms = await _patientDoctorVisitingFormRepository.ListAsync(accountantSpec);
            }
            else
            {
            }

            // return result;
            //return visitingForms.Select(x => _)
            return null;
        }
    }
}