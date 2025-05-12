using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LibrarySystem.Application.Services.Accounting;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using Moq;
using NUnit.Framework;

namespace LibrarySystem.IntegrationTests.Repositories
{
    [TestFixture]
    public class AccountingServicesTests
    {
        private Mock<IAccountingRepository> _accountingRepoMock;
        private AccountingServices _accountingService;

        [SetUp]
        public void Setup()
        {
            _accountingRepoMock = new Mock<IAccountingRepository>();
            _accountingService = new AccountingServices(_accountingRepoMock.Object);
        }

        [Test]
        public async Task SignUp_CallsRepositoryWithCorrectInput()
        {
            var dto = new UserSignUpDto { Username = "testuser", Password = "123456" };

            await _accountingService.SignUp(dto);

            _accountingRepoMock.Verify(repo => repo.Sign_Up(dto), Times.Once);
        }

        [Test]
        public async Task SignIn_ReturnsUser_WhenValidInput()
        {
            var dto = new UserSignInDto { Username = "testuser", Password = "123456" };
            var expectedUser = new User { Id = 1, Username = "testuser" };

            _accountingRepoMock.Setup(r => r.Sign_In(dto)).ReturnsAsync(expectedUser);

            var result = await _accountingService.SignIn(dto);

            Assert.That(result, Is.EqualTo(expectedUser));
        }

        [Test]
        public void ChangeUserRole_ThrowsException_WhenChangerIsNotAdmin()
        {
            Assert.ThrowsAsync<System.Exception>(async () =>
                await _accountingService.ChangeUserRole(1, Role.Member, Role.Member)
            );
        }

        [Test]
        public async Task ChangeUserRole_CallsRepository_WhenChangerIsAdmin()
        {
            await _accountingService.ChangeUserRole(1, Role.Admin, Role.Admin);

            _accountingRepoMock.Verify(r => r.ChangeUserRole(1, Role.Admin), Times.Once);
        }

        [Test]
        public async Task DeleteUser_CallsRepositoryOnce()
        {
            await _accountingService.DeleteUser(2);

            _accountingRepoMock.Verify(r => r.deleteUser(2), Times.Once);
        }

        [Test]
        public async Task ChangeUserPassword_CallsRepositoryWithCorrectInput()
        {
            int userId = 1;
            string newPassword = "newpass";

            await _accountingService.ChangeUserPassword(userId, newPassword);

            _accountingRepoMock.Verify(r => r.ChangeUserPassword(userId, newPassword), Times.Once);
        }

    }
}
