using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Exceptions.Patient;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Services;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Moq;
using NUnit.Framework;

namespace ClinicManagementSoftware.Test
{
    public class PatientServiceTests
    {
        private PatientService _patientService;
        private Mock<IRepository<Patient>> _patientRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IUserContext> _userContextMock;
        private Mock<IPatientDoctorVisitingFormService> _doctorVisitingFormServiceMock;
        private Mock<IPatientHospitalizedProfileService> _patientHospitalizedProfileServiceMock;

        [SetUp]
        public void Setup()
        {
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _mapperMock = new Mock<IMapper>();
            _userContextMock = new Mock<IUserContext>();
            _doctorVisitingFormServiceMock = new Mock<IPatientDoctorVisitingFormService>();
            _patientHospitalizedProfileServiceMock = new Mock<IPatientHospitalizedProfileService>();
            _patientService = new PatientService(_patientRepositoryMock.Object, _mapperMock.Object,
                _userContextMock.Object, _doctorVisitingFormServiceMock.Object,
                _patientHospitalizedProfileServiceMock.Object);
        }

        [Test]
        public async Task AddAsyncTest_ReturnNewPatient_CreateAPatientSuccessfully_ValidInput()
        {
            var request = new CreatePatientDto
            {
                FullName = "Sample",
                DateOfBirth = new DateTime(),
                Gender = "Nam",
                PhoneNumber = "8491203232",
                MedicalInsuranceCode = "12345678912345",
                AddressDetail = "Sample"
            };
            var expectedResult = new PatientDto
            {
                FullName = request.FullName,
                DateOfBirth = new DateTime(),
                Gender = "Nam",
                PhoneNumber = "8491203232",
                MedicalInsuranceCode = "12345678912345",
                AddressDetail = "Sample"
            };
            var samplePatient = new Patient()
            {
                Id = 1,
            };
            _userContextMock.Setup(x => x.GetCurrentContext()).ReturnsAsync(new
                CurrentUserContext(userId: 1, clinicId: 1, "", "", new Role(), 1));


            _patientRepositoryMock.Setup(x =>
                    x.AddAsync(samplePatient, default))
                .ReturnsAsync(new Patient());
            _mapperMock.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>())).Returns(expectedResult);
            var actualResult = await _patientService.AddAsync(request);
            Assert.IsNotNull(actualResult);
            _mapperMock.Verify(x => x.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Once());
            _userContextMock.Verify(x => x.GetCurrentContext(),
                Times.Once());
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>(), default),
                Times.Once());
            Assert.AreEqual(expectedResult.FullName, actualResult.FullName);
            Assert.AreEqual(expectedResult.Gender, actualResult.Gender);
        }

        [Test]
        public void AddAsyncTest_ThrowInvalidGenderException_CreateAPatientFailure_InvalidGender()
        {
            var request = new CreatePatientDto();
            Assert.ThrowsAsync<InvalidGenderException>(() => _patientService.AddAsync(request));
        }

        [Test]
        public void AddAsyncTest_ThrowArgumentNullException_CreateAPatientFailure_InvalidGender()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _patientService.AddAsync(null));
        }

        [Test]
        public void GetByIdAsyncTest_ThrowArgumentNullException_GetAPatientByIdFailure_IdIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _patientService.GetByIdAsync(null));
        }

        [Test]
        public void GetByIdAsyncTest_ThrowPatientNotFoundException_GetAPatientByIdFailure_InvalidId()
        {
            _patientRepositoryMock.Setup(x
                    => x.GetByIdAsync(It.IsAny<long>(), default))
                .ReturnsAsync(It.IsAny<Patient>());
            Assert.ThrowsAsync<PatientNotFoundException>(() =>
                _patientService.GetByIdAsync(2));
        }

        [Test]
        public async Task GetByIdAsyncTest_GetPatientSuccessfully_ValidInput()
        {
            var patient = new Patient()
            {
                Id = 1,
            };
            var expectedResult = new PatientDto()
            {
                FullName = "Ahihi",
                DateOfBirth = new DateTime(),
                Gender = "Nam",
                PhoneNumber = "8491203232",
                MedicalInsuranceCode = "12345678912345",
                AddressDetail = "Sample"
            };
            _patientRepositoryMock.Setup(x
                    => x.GetByIdAsync(It.IsAny<long>(), default))
                .ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>())).Returns(expectedResult);
            var actualResult = await _patientService.GetByIdAsync(1);
            Assert.NotNull(actualResult);
            _mapperMock.Verify(x => x.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Once());
            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<long>(), default),
                Times.Once());
            Assert.AreEqual(expectedResult.FullName, actualResult.FullName);
            Assert.AreEqual(expectedResult.Gender, actualResult.Gender);
        }

        [Test]
        public void DeleteAsyncTest_ThrowPatientNotFoundException_DeletePatientUnSuccessfully()
        {
            _patientRepositoryMock.Setup(x
                    => x.GetByIdAsync(It.IsAny<long>(), default))
                .ReturnsAsync(It.IsAny<Patient>());
            Assert.ThrowsAsync<PatientNotFoundException>(() =>
                _patientService.DeleteAsync(1));
        }

        [Test]
        public void DeleteAsyncTest_DeletePatientSuccessfully()
        {
            _patientRepositoryMock.Setup(x
                    => x.GetByIdAsync(It.IsAny<long>(), default))
                .ReturnsAsync(new Patient());
            _patientHospitalizedProfileServiceMock.Setup(x =>
                x.DeletePatientProfilesByPatientId(It.IsAny<long>()));
            _doctorVisitingFormServiceMock.Setup(x =>
                x.DeleteVisitingFormsByPatientId(It.IsAny<long>()));
            _patientRepositoryMock.Setup(x => x
                .UpdateAsync(It.IsAny<Patient>(), default));
            Assert.DoesNotThrowAsync(() => _patientService.DeleteAsync(2));
        }

        [Test]
        public async Task GetAllAsyncTest_ReturnAllActivePatientsSuccessfully_BlankSearchName()
        {
            const string searchName = "";
            var startDate = DateTime.Now.ResetTimeToStartOfDay();
            var endDate = DateTime.Now.ResetTimeToEndOfDay();
            var samplePatient = new Patient()
            {
                Id = 1,
                FullName = "a",
                CreatedAt = DateTime.UtcNow
            };
            var samplePatientDto = new PatientDto
            {
                Id = 1,
                FullName = "a",
                CreatedAt = DateTime.UtcNow.Format()
            };
            var expectedPatients = new List<Patient> {samplePatient};
            _userContextMock.Setup(x => x.GetCurrentContext()).ReturnsAsync(new
                CurrentUserContext(userId: 1, clinicId: 1, "", "", new Role(), 1));
            _patientRepositoryMock.Setup(x => x
                .ListAsync(It.IsAny<GetPatientsOfClinicFromDateSpec>(),
                    default)).ReturnsAsync(expectedPatients);
            _mapperMock.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>())).Returns(samplePatientDto);
            var result = await _patientService.GetAllAsync(searchName);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPatients.Count, result.Count());
            for (var i = 0; i < expectedPatients.Count; i++)
            {
                Assert.AreEqual(expectedPatients[i].Id, result.ToList().ElementAt(i).Id);
            }
        }

        [Test]
        public async Task GetAllAsyncTest_ReturnAllPatientsContainingSearchNameSuccessfully_NotBlankSearchName()
        {
            const string searchName = "ahihi";
            var startDate = DateTime.Now.ResetTimeToStartOfDay();
            var endDate = DateTime.Now.ResetTimeToEndOfDay();
            var samplePatient = new Patient()
            {
                Id = 1,
                FullName = "ahihiaaa",
                CreatedAt = DateTime.UtcNow
            };
            var samplePatientDto = new PatientDto
            {
                Id = 1,
                FullName = "ahihiaaa",
                CreatedAt = DateTime.UtcNow.Format()
            };
            var expectedPatients = new List<Patient> {samplePatient};
            _userContextMock.Setup(x => x.GetCurrentContext()).ReturnsAsync(new
                CurrentUserContext(userId: 1, clinicId: 1, "", "", new Role(), 1));
            _patientRepositoryMock.Setup(x => x
                .ListAsync(It.IsAny<GetPatientsByNameOfClinicSpec>(),
                    default)).ReturnsAsync(expectedPatients);
            _mapperMock.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>())).Returns(samplePatientDto);
            var result = await _patientService.GetAllAsync(searchName);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPatients.Count, result.Count());
            for (var i = 0; i < expectedPatients.Count; i++)
            {
                Assert.AreEqual(expectedPatients[i].Id, result.ToList().ElementAt(i).Id);
            }
        }
    }
}