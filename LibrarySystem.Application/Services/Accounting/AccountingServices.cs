using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using System.Threading.Tasks;

namespace LibrarySystem.Application.Services.Accounting
{
    public class AccountingServices
    {
        private readonly IAccountingRepository _accountingRepository;
        public AccountingServices(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }
        public async Task SignUp(UserSignUpDto input)
        {
            await _accountingRepository.Sign_Up(input);
        }
        public async Task<User> SignIn(UserSignInDto input)
        {
            return await _accountingRepository.Sign_In(input);
        }
        public async Task DeleteUser(int userId)
        {
            await _accountingRepository.deleteUser(userId);
        }
        public async Task ChangeUserPassword(int userId, string newPassword)
        {
            await _accountingRepository.ChangeUserPassword(userId, newPassword);
        }
        public async Task ChangeUserRole(int userId, Role role = Role.Member)
        {
            await _accountingRepository.ChangeUserRole(userId, role);
        }   
    }
}
