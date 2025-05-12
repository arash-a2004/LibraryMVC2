using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;

namespace LibrarySystem.Application.Services.Accounting
{
    public class AccountingServices : IAccountingServices
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
            var user = await _accountingRepository.Sign_In(input);

            if(!user.IsActive)
                return null;

            return user;
        }
        public async Task DeleteUser(int userId)
        {
            await _accountingRepository.deleteUser(userId);
        }
        public async Task ChangeUserPassword(int userId, string newPassword)
        {
            await _accountingRepository.ChangeUserPassword(userId, newPassword);
        }
        public async Task ChangeUserRole(int userId, Role changerRole, Role role = Role.Member)
        {
            if (changerRole != Role.Admin)
                throw new System.Exception();
            await _accountingRepository.ChangeUserRole(userId, role);
        }   
    }
}
