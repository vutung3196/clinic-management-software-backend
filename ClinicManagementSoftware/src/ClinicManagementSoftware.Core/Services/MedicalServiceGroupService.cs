using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalServiceGroupService : IMedicalServiceGroupService
    {
        //private readonly IUserContext _userContext;
        //private readonly IRepository<MedicalServiceGroupForTestSpecialist> _medicalServiceGroupSpecification;
        //private readonly IRepository<MedicalService> _medicalServiceSpecification;


        //public MedicalServiceGroupService(IUserContext userContext,
        //    IRepository<MedicalServiceGroupForTestSpecialist> medicalServiceGroupSpecification,
        //    IRepository<MedicalService> medicalServiceSpecification)
        //{
        //    _userContext = userContext;
        //    _medicalServiceGroupSpecification = medicalServiceGroupSpecification;
        //    _medicalServiceSpecification = medicalServiceSpecification;
        //}


        //public Task<IEnumerable<MedicalServiceGroupDto>> GetAllMedicalServiceGroups()
        //{
        //    throw new ArgumentException();
        //    //var currentUser = await _userContext.GetCurrentContext();
        //    //var @spec = new GetAllMedicalServicesByClinicIdSpec(currentUser.ClinicId);
        //    //var medicalGroups = await _medicalServiceSpecification.ListAsync(@spec);
        //    //var result = medicalGroups.SelectMany(x => x.Medications).OrderByDescending(x => x.CreatedAt);
        //    //return result.Select(x =>
        //    //    new MedicalServiceGroupDto(x.Id, x.Name, x.Description, x.CreatedAt.Format()));
        //}

        //public Task<MedicalServiceGroupDto> CreateMedicalServiceGroup(MedicalServiceGroupDto request)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<MedicalServiceGroupDto> EditMedicalServiceGroup(long id, MedicalServiceGroupDto request)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<MedicalServiceGroupResponseDto> CreateMedicalServiceGroup(MedicalServiceGroupResponseDto request)
        //{
        //    var medicalServiceGroup = new MedicalServiceGroupForTestSpecialist
        //    {
        //        CreatedAt = DateTime.Now,
        //        Description = request.Description,
        //        Name = request.Name,
        //    };

        //    medicalServiceGroup = await _medicalServiceGroupSpecification.AddAsync(medicalServiceGroup);
        //    return new MedicalServiceGroupResponseDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
        //        medicalServiceGroup.Description, medicalServiceGroup.CreatedAt.Format());
        //}

        //public async Task<MedicalServiceGroupResponseDto> EditMedicalServiceGroup(long id, MedicalServiceGroupResponseDto request)
        //{
        //    var medicalServiceGroup = await _medicalServiceGroupSpecification.GetByIdAsync(id);
        //    if (medicalServiceGroup == null)
        //    {
        //        throw new ArgumentException($"Cannot find service group with id: {id}");
        //    }

        //    medicalServiceGroup.UpdatedAt = DateTime.Now;
        //    medicalServiceGroup.Description = request.Description;
        //    medicalServiceGroup.Name = request.Name;

        //    await _medicalServiceGroupSpecification.UpdateAsync(medicalServiceGroup);
        //    return new MedicalServiceGroupResponseDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
        //        medicalServiceGroup.Description,
        //        medicalServiceGroup.CreatedAt.Format());
        //}

        //public async Task DeleteMedicalServiceGroup(long id)
        //{
        //    var medicalService = await _medicalServiceGroupSpecification.GetByIdAsync(id);

        //    if (medicalService == null)
        //    {
        //        throw new ArgumentException($"Cannot find medication service group with id: {id}");
        //    }

        //    await _medicalServiceGroupSpecification.DeleteAsync(medicalService);
        //}

        //Task<IEnumerable<MedicalServiceGroupResponseDto>> IMedicalServiceGroupService.GetAllMedicalServiceGroups()
        //{
        //    throw new NotImplementedException();
        //}
        public Task<IEnumerable<MedicalServiceGroupDto>> GetAllMedicalServiceGroups()
        {
            throw new NotImplementedException();
        }

        public Task<MedicalServiceGroupResponseDto> CreateMedicalServiceGroup(MedicalServiceGroupResponseDto request)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalServiceGroupResponseDto> EditMedicalServiceGroup(long id, MedicalServiceGroupResponseDto request)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalServiceGroupDto> CreateMedicalServiceGroup(MedicalServiceGroupDto request)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalServiceGroupDto> EditMedicalServiceGroup(long id, MedicalServiceGroupDto request)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<MedicalServiceGroupResponseDto>> IMedicalServiceGroupService.GetAllMedicalServiceGroups()
        {
            throw new NotImplementedException();
        }

        public Task DeleteMedicalServiceGroup(long id)
        {
            throw new NotImplementedException();
        }
    }
}